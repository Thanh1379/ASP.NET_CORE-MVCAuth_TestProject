using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1.Models;

[BindProperties]
public class RegisterModel
{
    [Required]
    [Display(Name = "Name", Prompt = "Your Full Name")]
    public required string Name {set; get;}

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 3)]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Your Password")]
    public required string Password {set; get;}

    [DataType(DataType.Password)]
    [Display(Name = "Retype Password", Prompt = "Retype Your Password")]
    [Compare("Password", ErrorMessage = "The password and The Retype Password do not match")]
    public required string RetypePassword {set; get;}
}