using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC.Areas.Identity.Data;
using System.Security.Claims;

namespace MVC.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ChatHub(ApplicationDbContext context, IHttpContextAccessor httpContext) 
        { 
            _context = context;
            _httpContext = httpContext;
        }
        
        public async Task SendMessageToGroup(string message, string dateTime)
        {
            var UserId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(UserId);
            var userName = user.Name;
            var email = user.Email;
            var url = _httpContext.HttpContext.Request.Path.ToString().Split("/");
            var group = url[url.Length - 1];

            await Clients.Group(group).SendAsync("ReceiveMessage", userName, email, message, dateTime);
        }

        public async Task JoinGroup(string group)
        {
            var UserId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.Find(UserId);
            var userName = user.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            Clients.Group(group).SendAsync("ReceiveLog", userName, "joined");
        }
    }
}
