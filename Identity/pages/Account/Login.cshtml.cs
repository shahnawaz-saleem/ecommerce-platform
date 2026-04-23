using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Error { get; set; }
        public string SuccessMessage { get; set; }
       
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _signInManager.PasswordSignInAsync(
                Username, Password, false, false);

            if (result.Succeeded)
            {
                return Redirect("/");
            }

            Error = "Invalid credentials";
            return Page();
        }
    }
}
