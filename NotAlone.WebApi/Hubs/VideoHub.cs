using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotAlone.WebApi.Hubs.Base;
using NotAlone.WebApi.ModelsWebApi.Video;
using SignalRSwaggerGen.Attributes;
using System;

namespace NotAlone.WebApi.Hubs
{
    [SignalRHub]
    [Authorize]
    public class VideoHub : UserHubBase
    {
        private readonly ILogger<VideoHub> logger;
        public VideoHub(
            ILogger<VideoHub> logger)
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

        public async Task SendVideoAsync(string url, string name)
        {
            var model = new VideoModel()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url
            };

            await this.Clients.All.SendAsync("ReceiveVideo", model).ConfigureAwait(false);
        }

        public async Task SendVideoQualityAsync(string videoId, string url, string name)
        {
            var model = new VideoQualityModel()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url,
                VideoId = new Guid(videoId)
            };

            await this.Clients.All.SendAsync("ReceiveVideoQuality", model).ConfigureAwait(false);
        }

        public async Task SendNewVideoTimeStampAsync(string videoId, string timestamp)
        {
            var model = new VideoTimeStampModel()
            {
                VideoId = new Guid(videoId),
                TimeStamp = timestamp
            };

            await this.Clients.All.SendAsync("ReceiveNewVideoTimeStamp", model).ConfigureAwait(false);
        }

        public async Task SendStartVideoAsync(string videoId, string timestamp)
        {
            var model = new VideoTimeStampModel()
            {
                VideoId = new Guid(videoId),
                TimeStamp = timestamp
            };

            await this.Clients.All.SendAsync("ReceiveStartVideo", model).ConfigureAwait(false);
        }

        public async Task SendStopVideoAsync(string videoId, string timestamp)
        {
            await this.Clients.All.SendAsync("ReceiveStopVideo").ConfigureAwait(false);
        }
    }
}
