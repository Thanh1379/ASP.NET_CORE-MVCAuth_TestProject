using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1.Controllers;

public class ExternalLoginController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly UserManager<IdentityUser> _userManager;
    public ExternalLoginController(
        SignInManager<IdentityUser> signInManager, 
        IUserStore<IdentityUser> userStore,
        UserManager<IdentityUser> userManager
        )
    {
        _signInManager = signInManager;
        _userStore = userStore;
        _userManager = userManager;
    }
    
    public IActionResult Handler(string? provider, string? ReturnUrl)
    {
        if(provider == null)
        {
            return Redirect("/Account/Login");
        }
        string redirectUrl = Url.Action("CallBack", "ExternalLogin", values: new{ReturnUrl}) ?? "/";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }
    public async Task<IActionResult> CallBack(string? ReturnUrl, string? remoteError = null)
    {
        ReturnUrl ??= "/";
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if(remoteError != null || info == null)
        {
            return Redirect(Url.Action("Login", "Account", values: new {ReturnUrl}) ?? "/");
        }
        //Login Immediately when user account has logged by external provider
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if(result.Succeeded)
        {
            return Redirect(ReturnUrl);
        }
        else if(result.IsLockedOut)
        {
            return Redirect("/Account/LockOut");
        }
        else
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new IdentityUser();
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            var createUserResult = await _userManager.CreateAsync(user);
            if(createUserResult.Succeeded)
            {
                var addExternalLoginResult = await _userManager.AddLoginAsync(user, info);
                if(addExternalLoginResult.Succeeded) 
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect(ReturnUrl);
                }
            }
        }
        return Redirect("/");
    }
}