using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCAuth1.Models;

namespace MVCAuth1.Controllers;
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserStore<IdentityUser> _userStore;
    public AccountController(
        ILogger<AccountController> _logger,
        UserManager<IdentityUser> _userManager,
        SignInManager<IdentityUser> _signInManager,
        IUserStore<IdentityUser> _userStore
        )
    {
        this._logger = _logger;
        this._userManager = _userManager;
        this._signInManager = _signInManager;
        this._userStore = _userStore;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel data) {
        
        if(ModelState.IsValid)
        {
            IdentityUser user = new IdentityUser();
            await _userStore.SetUserNameAsync(user, data.Name, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, data.Password);
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        
        return View();
    }
    public async Task<IActionResult> Login(string? ReturnUrl)
    {
        LoginModel data = new LoginModel();
        data.ReturnUrl = ReturnUrl;
        data.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel data, string? ReturnUrl)
    {
        ReturnUrl ??= "/";
        if(ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(data.Name, data.Password, data.RememberMe, lockoutOnFailure: true);
            if(result.Succeeded)
            {
                _logger.LogInformation("Userlogged in");
                return LocalRedirect(ReturnUrl);
            }
            if(result.RequiresTwoFactor)
            {
                
            }
            if(result.IsLockedOut)
            {
                _logger.LogInformation("User is locked");
                return Redirect("/Account/LockOut");
            }

            ModelState.AddModelError(string.Empty, "You password or username is not valid");
        }
        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> LogOut(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        if(returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        return LocalRedirect("/");
    }
    public IActionResult LockOut()
    {
        return View();
    }
}