using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hesa.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hesa.Portal.Areas.Identity.Pages
{
    #pragma warning disable SA1649 // File name should match first type name
    /// <summary>
    /// The login page model.
    /// </summary>
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<HesaUser> _userManager;
        private readonly SignInManager<HesaUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        /// <param name="signInManager">The sign-in manager.</param>
        /// <param name="logger">App logger.</param>
        /// <param name="userManager">Identity user manager.</param>
        public LoginModel(
            SignInManager<HesaUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<HesaUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets the input model (login fields).
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Gets or sets he return url (after login).
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the error message (if any).
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Handles the login form GET.
        /// </summary>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Completed result.</returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        /// <summary>
        /// Handles the login form POST.
        /// </summary>
        /// <param name="returnUrl">Return URL.</param>
        /// <returns>Completed result.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Locate a user by user name or email.
                var user = await _signInManager.UserManager.FindByNameAsync(Input.UserNameOrEmail);

                if (user is null)
                {
                    user = await _signInManager.UserManager.FindByEmailAsync(Input.UserNameOrEmail);
                }

                if (user is object)
                {
                    // This doesn't count login failures towards account lockout
                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
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
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        #pragma warning disable SA1600 // Elements should be documented

        public class InputModel
        {
            [Required]
            [Display(Name = "Username or Email")]
            public string UserNameOrEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}
