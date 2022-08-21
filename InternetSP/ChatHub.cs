using InternetSP.Controllers;
using InternetSP.Models;
using Microsoft.AspNetCore.SignalR;

namespace InternetSP
{
    public class ChatHub:Hub
    {
        private readonly InternetSPContext _context;
        private readonly static ConnectionMapping _connections = new ConnectionMapping();
        public ChatHub(InternetSPContext context)
        {
            _context = context;
        }
        public override Task OnConnectedAsync()
        {
            var orderId = Convert.ToInt32(Context.GetHttpContext().Request.Query["orderId"]);
            _connections.Add(orderId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var orderId = Convert.ToInt32(Context.GetHttpContext().Request.Query["orderId"]);
            _connections.Remove(orderId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var user = new CommonController(_context).GetUserId(Context.GetHttpContext().Request);
            var orderId = Convert.ToInt32(Context.GetHttpContext().Request.Query["orderId"]);
            var connections = _connections.GetConnections(orderId);
            foreach (var item in connections)
            {
                await Clients.Client(item).SendAsync("ReceiveMessage", user.Name, message);
            }

            _context.Messages.Add(new Message
            {
                SenderId = user.Id,
                ReceiverId = orderId,
                DateTime = DateTime.UtcNow.AddHours(5),
                Text = message
            });
            await _context.SaveChangesAsync();
        }
    }
}
