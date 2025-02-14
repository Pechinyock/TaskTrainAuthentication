using Microsoft.AspNetCore.Authorization;
using TT.Auth.Constants;
using TT.Core;

namespace TT.Auth;

internal class AdminAccessLayer : AuthorizationHandler<AdminAccessLayer>
                                , IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAccessLayer requirement)
    {
        var claims = context.User.Claims.ToList();
        var accessLayerClaim = context.User.FindFirst(x => x.Type == Policy.AccessLayer.Type);
        if (accessLayerClaim == null)
            return Task.CompletedTask;

        if (!Enum.TryParse(accessLayerClaim.Value, out UserAccesLayerEnum usersAccessLayer)) 
        {
            var reason = new AuthorizationFailureReason(this, "Failed to parse access layer claim");
            context.Fail(reason);
            return Task.CompletedTask;
        }

        if (usersAccessLayer < UserAccesLayerEnum.Administrator)
        {
            var reason = new AuthorizationFailureReason(this, "You don't have permission, bitch");
            context.Fail(reason);
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
