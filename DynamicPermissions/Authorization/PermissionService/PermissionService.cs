using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicPermissions.Data.Repositories;

namespace DynamicPermissions.Authorization.PermissionService
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public async Task<bool> CanCurrentUserAccess(string controller, string action,string role)
        {
            var permission = await _permissionRepository.GetAll();
            return permission.FirstOrDefault(x => x.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase) &&
                                                  x.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase) &&
                                                  x.Role.Equals(role, StringComparison.InvariantCultureIgnoreCase)) != null;
        }
    }
}
