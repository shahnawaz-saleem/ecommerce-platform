using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.pages.Account
{
    public class Register : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Register(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public RoleTYPE Role { get; set; }
        public string SuccessMessage { get; set; }

        public string Error { get; set; }
       
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,FullName=model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, model.Role.ToString());

            return Page();
        }
    }
}
