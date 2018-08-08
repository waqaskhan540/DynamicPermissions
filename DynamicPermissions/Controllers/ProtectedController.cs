using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicPermissions.Controllers
{
    [Authorize(Policy = "DynamicPermission")]
    [Route("api/[controller]/[action]")]
    public class ProtectedController : Controller
    {
        [HttpGet]
        public IActionResult FreeUser()
        {
            return Content("Hey, you are a free user.");
        }
        [HttpGet]
        public IActionResult PaidUser()
        {
            return Content("Hey, we definitely love you.");
        }
        [HttpGet]
        public IActionResult AdminUser()
        {
            return Content("Oppps.. admin's here..");
        }
        [HttpGet]
        public IActionResult Visitor()
        {
            return Content("That's the best we can give to a visitor..");
        }
    }
}
