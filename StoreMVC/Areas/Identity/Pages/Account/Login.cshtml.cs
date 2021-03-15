using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StoreModels;
using StoreBL;
using Microsoft.AspNetCore.Http;

namespace StoreMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<StoreMVCUser> _userManager;
        private readonly SignInManager<StoreMVCUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly CartBL cartBL;
        public LoginModel(SignInManager<StoreMVCUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<StoreMVCUser> userManager,
            CartBL cartBL)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this.cartBL = cartBL;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var appUser = _signInManager.UserManager.Users.SingleOrDefault(r => r.Email == Input.Email);
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
                    var claims = claimsPrincipal.Claims.ToList();
                    
                    _logger.LogInformation("User logged in.");
                    string cartIds = Request.Cookies["customerId"];
                    if (cartIds != null)
                    {
                        if (!cartIds.Equals(""))
                        {
                            CookieOptions option = new CookieOptions();
                            option.Expires = DateTime.Now.AddYears(-100);
                            Response.Cookies.Append("customerId", appUser.Id, option);
                            cartBL.UpdateCustomerInCart(cartIds, appUser.Id);
                        }
                    }
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
