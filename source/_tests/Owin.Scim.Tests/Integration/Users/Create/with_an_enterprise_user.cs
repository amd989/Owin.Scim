namespace Owin.Scim.Tests.Integration.Users.Create
{
    using System.Net;
    using System.Net.Http;

    using Machine.Specifications;

    using Model.Users;

    public class with_an_enterprise_user : when_creating_a_user
    {
        Establish context = () =>
        {
            UserDto = new EnterpriseUser
            {
                UserName = UserNameUtility.GenerateUserName(),
                Enterprise =
                {
                    Department = "Sales"
                }
            };
        };

        It should_return_created = () => Response.StatusCode.ShouldEqual(HttpStatusCode.Created);

        It should_return_the_user = () => Response.Content.ReadAsAsync<User>(ScimJsonMediaTypeFormatter.AsArray()).Result.Id.ShouldNotBeEmpty();
    }
}