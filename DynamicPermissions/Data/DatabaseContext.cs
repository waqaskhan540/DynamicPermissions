using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicPermissions.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DynamicPermissions.Data
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
