using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.pages.Account
{
    public class Login : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Login(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Error { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _signInManager.PasswordSignInAsync(
                Username, Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(ReturnUrl ?? "/");
            }

            Error = "Invalid credentials";
            return Page();
        }
    }
}
