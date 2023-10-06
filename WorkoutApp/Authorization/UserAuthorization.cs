using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WorkoutApp.Models;

namespace WorkoutApp.Authorization
{
    public class UserAuthorization : AuthorizationHandler<ResourceOperationRequirement, User>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, User resource)
        {
            var LoggedUserId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.UserId == int.Parse(LoggedUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
