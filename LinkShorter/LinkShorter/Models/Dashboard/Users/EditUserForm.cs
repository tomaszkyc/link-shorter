using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LinkShorter.Models.Dashboard.Users
{
    
    public class EditUserForm
    {
        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string[] ActualUserRoles { get; set; }

        [BindProperty]
        [DisplayName("Is account locked?")]
        public bool AccountLocked { get; set; }

        public EditUserForm(IdentityUser user)
        {
            Id = user.Id;
            Username = user.UserName;
            Email = user.Email;
            AccountLocked = (DateTimeOffset.UtcNow < user.LockoutEnd);
        }
        public EditUserForm()
        {

        }

    }
}
