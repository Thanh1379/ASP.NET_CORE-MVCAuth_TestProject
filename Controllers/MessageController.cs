using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCAuth1.Data;
using MVCAuth1.Models;

namespace MVCAuth1.Controllers;

[Authorize]
public class MessageController : Controller
{
    private readonly ILogger<MessageController> logger;
    private readonly ApplicationDbContext dbContext;
    public MessageController(ILogger<MessageController> _logger, ApplicationDbContext _dbContext)
    {
        logger = _logger;
        dbContext = _dbContext;
    }
    [HttpGet]
    public IActionResult GetID()
    {
        return View();
    }
    [HttpPost]
    public IActionResult GetID(UIDInputModel uIDInputModel)
    {
        string friendUID = uIDInputModel.FriendUID ?? "fuck";
        
        return Redirect($"/Message/Chat?friendUID={friendUID}");
    }
    public async Task<IActionResult> Chat(string friendUID)
    {
        string? uid = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(uid == null || friendUID == null)
        {
            return NotFound();
        }
        List<Message> messages = (from message in dbContext.Messages where (message.UserSend.Id == uid && message.UserReceive.Id  == friendUID) || (message.UserReceive.Id == uid && message.UserSend.Id  == friendUID) select message).ToList();
        foreach(var message in messages)
        {   
            var entry = dbContext.Entry(message);
            await entry.Reference((message) => message.UserSend).LoadAsync();
        }
        
        Console.WriteLine("Call Here is Post");
        return View(messages);
    }
}