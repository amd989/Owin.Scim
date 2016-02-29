﻿namespace Owin.Scim.Services
{
    using System.Threading.Tasks;

    using Microsoft.FSharp.Core;

    using Model.Groups;

    public interface IGroupService
    {
        Task<IScimResponse<Group>> CreateGroup(Group group);

        Task<IScimResponse<Group>> RetrieveGroup(string groupId);

        Task<IScimResponse<Group>> UpdateGroup(Group group);

        Task<IScimResponse<Unit>> DeleteGroup(string groupId);

    }
}