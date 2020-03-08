using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShorter.Models.Dashboard.Users;
using LinkShorter.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinkShorter.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Dashboard/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ILogger<UserController> logger,
                            UserManager<IdentityUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }




        // GET: /<controller>/
        public async Task<IActionResult> Index(int? pageId, int? pageSize)
        {
            ViewData["ActivePage"] = "Users";

            if (pageId == null)
            {
                pageId = 0;
            }

            if (pageSize == null)
            {
                pageSize = 10;
            }
            var users = await _userManager.Users.ToListAsync();

            var paginatedUsers = new PaginatedList<IdentityUser>(users.AsQueryable(), (int)pageId, (int)pageSize);

            return View("~/Views/Dashboard/Users/Index.cshtml", paginatedUsers);
        }

        [Route("Edit/{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            EditUserForm editUserForm = new EditUserForm(user);


            return View("~/Views/Dashboard/Users/Edit.cshtml", editUserForm);
        }

        [HttpGet]
        [Route("api/v1/user/roles/available")]
        public async Task<IActionResult> GetUserAvailableRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User with given id doesn't exist");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            
            var availableRoles = _roleManager.Roles.Where(role => !userRoles.Contains(role.Name));

            return Ok(availableRoles);
        }

        [HttpGet]
        [Route("api/v1/user/roles/actual")]
        public async Task<IActionResult> GetUserActualRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User with given id doesn't exist");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var actualUserRoles = _roleManager.Roles.Where(role => userRoles.Contains(role.Name));

            return Ok(actualUserRoles);
        }


        [HttpPost]
        [Route("Edit/{userId}")]
        public async Task<IActionResult> Edit(string userId, [FromForm] EditUserForm editUserForm)
        {
            var user = await _userManager.FindByIdAsync(userId);
            

            if (user == null)
            {
                return NotFound("User with given id doesn't exist");
            }
            //update email
            user.Email = editUserForm.Email;

            //lock account if checkbox is checked
            if ( editUserForm.AccountLocked )
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;
            }
            
            
            await _userManager.UpdateAsync(user);

            EditUserForm oldForm = new EditUserForm(user);
            return View("~/Views/Dashboard/Users/Edit.cshtml", oldForm);
        }

        [HttpPost]
        [Route("api/v1/user/roles/edit")]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> rolesToGrant) 
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User with given id doesn't exist");
            }

            //revoke roles
            var rolesToRevoke = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user ,rolesToRevoke);

            foreach(var roleId in rolesToGrant)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if ( role == null )
                {
                    return NotFound(String.Format("Given role id doesn't exist: {0}", roleId));
                }

                //if user doesn't have role
                if ( !await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                
            }


            return Ok("Access granted");
        }
    }
}
