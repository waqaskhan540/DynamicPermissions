using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicPermissions.Data.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Role { get; set; }

        public Permission(int id,string controller,string action,string role)
        {
            Id = id;
            Controller = controller;
            Action = action;
            Role = role;

        }
    }
}
