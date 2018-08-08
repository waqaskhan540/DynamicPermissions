using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DynamicPermissions.Authorization.PermissionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DynamicPermissions.Authorization
{
    public class DynamicPermissionRequirement : IAuthorizationRequirement
    {
        
    }
    public class DynamicPermissionAuthorizationHandler : AuthorizationHandler<DynamicPermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public DynamicPermissionAuthorizationHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override  Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicPermissionRequirement requirement)
        {
            var mvcContext = context.Resource as AuthorizationFilterContext;
            if(mvcContext == null)
            {
                 return Task.CompletedTask;
            }
            
            var actionDescriptor = mvcContext.ActionDescriptor;
            var controller = actionDescriptor.RouteValues["controller"];
            var action = actionDescriptor.RouteValues["action"];

            var user = mvcContext.HttpContext.User;
            if(user == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var roleClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            if(roleClaim == null)
            {
                context.Fail();                
                return Task.CompletedTask;
            }

            if(_permissionService.CanCurrentUserAccess(controller, action, roleClaim.Value).Result)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else
            {
                context.Fail();
                return Task.CompletedTask;
            }
           

        }
    }
}
