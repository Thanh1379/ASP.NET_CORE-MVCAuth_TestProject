using Microsoft.AspNetCore.SignalR;
namespace MVCAuth1.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(string type, string user, string message);
        Task SendAsync(string typt, string user, string message);
    }
}