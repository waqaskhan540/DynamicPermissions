using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicPermissions.Data.Entities;

namespace DynamicPermissions.Data.Repositories
{
    public interface IPermissionRepository
    {
        //we could make it more specific
        Task<List<Permission>> GetAll();
    }
}
