﻿namespace Owin.Scim.Tests.Services.UserService.Create
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;

    using Configuration;

    using FakeItEasy;

    using Machine.Specifications;

    using Mappings;

    using Model;
    using Model.Users;

    using Repository;

    using Scim.Services;
    using Scim.Validation.Users;

    using Security;

    public abstract class when_creating_a_user
    {
        Establish context = () =>
        {
            Mapper.Initialize(c => new UserMapping().Configure(Mapper.Configuration));

            UserRepository = A.Fake<IUserRepository>();
            PasswordManager = A.Fake<IManagePasswords>();
            PasswordComplexityVerifier = A.Fake<IVerifyPasswordComplexity>();

            A.CallTo(() => UserRepository.IsUserNameAvailable(A<string>._))
                .Returns(true);
            
            A.CallTo(() => UserRepository.CreateUser(A<User>._))
                .ReturnsLazily(c =>
                {
                    var user = (User) c.Arguments[0];
                    user.Id = Guid.NewGuid().ToString("N");

                    return Task.FromResult(user);
                });

            _UserService = new UserService(
                new ScimServerConfiguration(), 
                UserRepository, 
                PasswordManager, 
                new UserValidator(UserRepository, PasswordComplexityVerifier, PasswordManager));
        };

        Because of = async () => Result = await _UserService.CreateUser(ClientUserDto).AwaitResponse().AsTask;

        protected static User ClientUserDto;

        protected static IUserRepository UserRepository;

        protected static IManagePasswords PasswordManager;

        protected static IVerifyPasswordComplexity PasswordComplexityVerifier;

        protected static IScimResponse<User> Result;

        private static IUserService _UserService;
    }
}