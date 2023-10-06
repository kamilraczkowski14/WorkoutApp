using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WorkoutApp.Models;

namespace WorkoutApp.Authorization
{
    public class WorkoutPlanAuthorization : AuthorizationHandler<ResourceOperationRequirement, WorkoutPlan>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, WorkoutPlan resource)
        {
            var LoggedUserId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //var user = _context.Users.FirstOrDefault(u => u.UserId == int.Parse(LoggedUserId));

            if (resource.UserId == int.Parse(LoggedUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
