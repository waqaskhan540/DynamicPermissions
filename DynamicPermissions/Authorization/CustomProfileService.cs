using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;

namespace DynamicPermissions.Authorization
{
    public class CustomProfileService : IProfileService
    {
        private readonly TestUserStore userStore;

        public CustomProfileService(TestUserStore userStore)
        {
            this.userStore = userStore;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
