using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1.Models;

[BindProperties]
public class UIDInputModel
{
    [Required]
    public required string FriendUID{set; get;}
}