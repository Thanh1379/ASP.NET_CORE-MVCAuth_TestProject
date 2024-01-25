using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1.Models;

[BindProperties]
public class LoginModel
{
    [Required]
    [Display(Name = "Name", Prompt = "Your Full Name")]
    public string Name {set; get;} = "";

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 3)]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Your Password")]
    public string Password {set; get;} = "";
    [Display(Name = "Remember Me")]
    public bool RememberMe {set; get;}
    public string? ReturnUrl {set; get;}
    public IList<AuthenticationScheme>? ExternalLogins {get; set;}
}