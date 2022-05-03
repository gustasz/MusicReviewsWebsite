using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicReviewsWebsite.Models;

namespace MusicReviewsWebsite.Pages.Users
{
    [Authorize(Roles = "Admin,Moderator")]
    public class BanUserModel : PageModel
    {
        private readonly MusicReviewsWebsite.Data.MusicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BanUserModel(MusicReviewsWebsite.Data.MusicContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public ApplicationUser ApplicationUser { get; set; }
        [BindProperty]
        public DateTimeOffset? BanUserUntil { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (userId == null)
                return NotFound();
            ApplicationUser = await _context.Users.FindAsync(userId);
            if (ApplicationUser == null)
                return NotFound();
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            ApplicationUser = await _context.Users.FindAsync(userId);
            if (ApplicationUser == null)
                return Page();
            if (BanUserUntil != null)
            {
                await _userManager.SetLockoutEndDateAsync(ApplicationUser, BanUserUntil);
                await _userManager.UpdateSecurityStampAsync(ApplicationUser);
            }
            return RedirectToPage("Profile", new { name = ApplicationUser.Name });

        }
    }
}
