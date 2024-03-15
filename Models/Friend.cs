using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1.Models;

[BindProperties]
public class Friend
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FriendId{set; get;}
    [ForeignKey("user1Id")]
    public required IdentityUser User1 {set; get;}
    [ForeignKey("user2Id")]
    public required IdentityUser User2 {set; get;}
}