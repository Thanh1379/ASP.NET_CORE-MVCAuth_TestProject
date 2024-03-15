using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCAuth1.Models;

[BindProperties]
public class Message
{
    public Message()
    {
        SendingTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int MessageId {set; get;}
    [Required]
    [DisplayName("content")]
    public string Content {set; get;}
    public string userSendId;
    [ForeignKey("userSendId")]
    [Required]
    public IdentityUser UserSend{set; get;}
    public string userReceiveId;
    [ForeignKey("userReceiveId")]
    public IdentityUser UserReceive{set; get;}
    [DisplayName("sendingTime")]
    public long SendingTime {set; get;}
}