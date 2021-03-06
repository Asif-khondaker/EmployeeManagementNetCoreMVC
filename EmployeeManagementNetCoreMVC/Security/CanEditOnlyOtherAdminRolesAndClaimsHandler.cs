using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagementNetCoreMVC.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : 
        AuthorizationHandler<ManageAdminRolesAndClaimsRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirment requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if(authFilterContext == null)
            {
                return Task.CompletedTask;
            }
            string loggedAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string adminIdBeingEditted = authFilterContext.HttpContext.Request.Query["userId"];
            if(context.User.IsInRole("Admin") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
                adminIdBeingEditted.ToLower() != loggedAdminId.ToLower())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
