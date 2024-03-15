using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MVCAuth1.Data;
using MVCAuth1.Models;
using Neo4j.Driver;
using StackExchange.Redis;
namespace MVCAuth1.Hubs
{
    public class ChatHub : Hub
    {   
        private readonly IDatabase database;
        private readonly ApplicationDbContext dbContext;
        public ChatHub(ConnectionMultiplexer connectionMultiplexer, ApplicationDbContext _dbContext)
        {
            database = connectionMultiplexer.GetDatabase();
            dbContext = _dbContext;
        }
        [Authorize]
        public async Task SendMessage(string userReceive, string message)
        {
            string userSend = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            string? userName = Context.User?.Identity?.Name;
            Message messageToSend = new Message();
            messageToSend.userSendId = userSend;
            messageToSend.userReceiveId = userReceive;
            messageToSend.Content = message;
            await dbContext.Messages.AddAsync(messageToSend);
            await dbContext.SaveChangesAsync();
            await Clients.User(userReceive).SendAsync("ReceiveMessage",userName, message);
            await Clients.User(userSend).SendAsync("ReceiveMessage",userName, message);
        }
        [Authorize]
        public override Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"Connection Id: {connectionId} UserId: {userId}");
            if(userId != null)
            {
                database.StringSet(userId, connectionId);
            }
            return base.OnConnectedAsync();
        }
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId != null)
            {
                string? connectionId = await database.StringGetAsync(userId);
                Console.WriteLine($"Connection Id: {connectionId} UserId: {userId}");
                await database.KeyDeleteAsync(userId);
            }
            return base.OnConnectedAsync();
        }
    }
}