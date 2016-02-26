﻿namespace Owin.Scim.Services
{
    using System;
    using System.Net;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Canonicalization;

    using Configuration;

    using ErrorHandling;

    using Extensions;

    using Microsoft.FSharp.Core;

    using Model;
    using Model.Users;

    using Repository;

    using Security;

    using Validation;

    public class UserService : ServiceBase, IUserService
    {
        private readonly DefaultCanonicalizationService _CanonicalizationService;

        private readonly IUserRepository _UserRepository;

        private readonly IManagePasswords _PasswordManager;

        private readonly IResourceValidatorFactory _ResourceValidatorFactory;

        public UserService(
            ScimServerConfiguration scimServerConfiguration,
            DefaultCanonicalizationService canonicalizationService,
            IUserRepository userRepository,
            IManagePasswords passwordManager,
            IResourceValidatorFactory resourceValidatorFactory)
            : base(scimServerConfiguration)
        {
            _CanonicalizationService = canonicalizationService;
            _UserRepository = userRepository;
            _PasswordManager = passwordManager;
            _ResourceValidatorFactory = resourceValidatorFactory;
        }

        public async Task<IScimResponse<User>> CreateUser(User user)
        {
            await CanonicalizeUser(user);

            var validator = await _ResourceValidatorFactory.CreateValidator(user);
            var validationResult = (await validator.ValidateAsync(user, ruleSet: RuleSetConstants.Create)).ToScimValidationResult();

            if (!validationResult)
                return new ScimErrorResponse<User>(validationResult.Errors.First());
            
            var createdDate = DateTime.UtcNow;
            user.Meta = new ResourceMetadata(ScimConstants.ResourceTypes.User)
            {
                Created = createdDate,
                LastModified = createdDate
            };

            var userRecord = await _UserRepository.CreateUser(user);

            SetResourceVersion(user);

            return new ScimDataResponse<User>(userRecord);
        }

        public async Task<IScimResponse<User>> RetrieveUser(string userId)
        {
            var userRecord = SetResourceVersion(await _UserRepository.GetUser(userId));
            if (userRecord == null)
                return new ScimErrorResponse<User>(
                    new ScimError(
                        HttpStatusCode.NotFound,
                        detail: ErrorDetail.NotFound(userId)));

            return new ScimDataResponse<User>(userRecord);
        }

        public async Task<IScimResponse<User>> UpdateUser(User user)
        {
            var userRecord = await _UserRepository.GetUser(user.Id);
            if (userRecord == null)
            {
                return new ScimErrorResponse<User>(
                    new ScimError(
                        HttpStatusCode.NotFound,
                        detail: ErrorDetail.NotFound(user.Id)));
            }

            user.Meta = new ResourceMetadata(ScimConstants.ResourceTypes.User)
            {
                Created = userRecord.Meta.Created,
                LastModified = userRecord.Meta.LastModified
            };

            await CanonicalizeUser(user);

            var validator = await _ResourceValidatorFactory.CreateValidator(user);
            var validationResult = (await validator.ValidateAsync(user, userRecord, ruleSet: RuleSetConstants.Update))
                .ToScimValidationResult();

            if (!validationResult)
                return new ScimErrorResponse<User>(validationResult.Errors.First());

            // TODO: (DG) support password change properly, according to service prov config.
            if (!string.IsNullOrWhiteSpace(userRecord.Password))
            {
                userRecord.Password = _PasswordManager.CreateHash(
                    Encoding.UTF8.GetString(Encoding.Unicode.GetBytes(user.Password.Trim())));
            }

            SetResourceVersion(user);

            // if both versions are equal, bypass persistence
            if (user.Meta.Version.Equals(userRecord.Meta.Version))
                return new ScimDataResponse<User>(user); // TODO: (DG) is this proper behavior?

            user.Meta.LastModified = DateTime.UtcNow;

            await _UserRepository.UpdateUser(user);

            return new ScimDataResponse<User>(user);
        }

        public async Task<IScimResponse<Unit>> DeleteUser(string userId)
        {
            var result = await _UserRepository.DeleteUser(userId);
            if (result == null)
                return new ScimErrorResponse<Unit>(
                    new ScimError(
                        HttpStatusCode.NotFound,
                        detail: ErrorDetail.NotFound(userId)));

            return new ScimDataResponse<Unit>(default(Unit));
        }

        protected virtual Task CanonicalizeUser(User user)
        {
            _CanonicalizationService.Canonicalize(user, ScimServerConfiguration.GetScimTypeDefinition(typeof(User)));

            return Task.FromResult(0);
        }
    }
}