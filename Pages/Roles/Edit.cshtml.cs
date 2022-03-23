using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicReviewsWebsite.Data;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly MusicContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EditModel(MusicContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public UserVM UserVM { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var allRoles = _roleManager.Roles.ToList();
            var roles = await _userManager.GetRolesAsync(user);

            var roleCheckboxList = new List<RoleInfo>();
            foreach(var role in allRoles)
            {
                roleCheckboxList.Add(new RoleInfo { Name = role.Name, Selected = roles.Contains(role.Name) ? true : false });
            }
            UserVM = new UserVM
            {
                Id = user.Id,
                Name = user.UserName,
                Roles = roleCheckboxList
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            foreach (var role in UserVM.Roles)
            {
                if (role.Selected)
                    await _userManager.AddToRoleAsync(user, role.Name);
                else
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            return RedirectToPage("./Index");
        }

    }
}
