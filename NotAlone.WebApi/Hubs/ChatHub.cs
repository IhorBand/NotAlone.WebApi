using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotAlone.WebApi.Hubs.Base;
using NotAlone.WebApi.ModelsWebApi.Chat;
using SignalRSwaggerGen.Attributes;

namespace NotAlone.WebApi.Hubs
{
    [SignalRHub]
    [Authorize]
    public class ChatHub : UserHubBase
    {
        private readonly ILogger<ChatHub> logger;
        public ChatHub(
            ILogger<ChatHub> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [SignalRHidden]
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        [SignalRHidden]
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageAsync(string message)
        {
            var messageModel = new MessageModel()
            {
                Message = message,
                UserId = this.UserId,
                UserName = this.UserName
            };

            await this.Clients.All.SendAsync("ReceiveMessage", messageModel).ConfigureAwait(false);
        }
    }
}
