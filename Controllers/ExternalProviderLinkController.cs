using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1;

public class ExternalProviderLinkController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public ExternalProviderLinkController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    
    public async Task<IActionResult> Handler(string? provider, string? ReturnUrl)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        if(provider == null) 
        {
            return NotFound("Do not specify any provider");
        }
        ReturnUrl ??= "/";
        
        var redirectUrl = Url.Action("CallBack", "ExternalProviderLink", values: new {ReturnUrl});
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
        return new ChallengeResult(provider, properties);
    }
    public async Task<IActionResult> CallBack(string? ReturnUrl, string? remoteError)
    {
        var user = await _userManager.GetUserAsync(User);

        if(remoteError != null || user == null)
        {
            return NotFound("Something went wrong");
        }
        var userId = await _userManager.GetUserIdAsync(user);
        var info = await _signInManager.GetExternalLoginInfoAsync(userId);
        if(info == null)
        {
            return NotFound("Unable load external login info");
        }
        var result = await _userManager.AddLoginAsync(user, info);
        if(!result.Succeeded)
        {
            return NotFound("Google Account Has Linked at somewhere");
        }
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        ReturnUrl ??= "/";
        return Redirect(ReturnUrl);
    }
}