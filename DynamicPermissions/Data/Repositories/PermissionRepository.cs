using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicPermissions.Data.Entities;

namespace DynamicPermissions.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DatabaseContext dbContext;

        public PermissionRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<List<Permission>> GetAll()
        {
            return Task.FromResult(dbContext.Permissions.ToList());
        }
    }
}
