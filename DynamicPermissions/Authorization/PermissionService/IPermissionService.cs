using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicPermissions.Authorization.PermissionService
{
    public interface IPermissionService
    {
        Task<bool> CanCurrentUserAccess(string controller, string action,string role);
    }
}
