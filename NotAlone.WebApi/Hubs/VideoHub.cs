using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotAlone.WebApi.Hubs.Base;
using NotAlone.WebApi.ModelsWebApi.Video;
using NotAlone.WebApi.Services.Abstractions;
using SignalRSwaggerGen.Attributes;
using System;

namespace NotAlone.WebApi.Hubs
{
    [SignalRHub]
    [Authorize]
    public class VideoHub : UserHubBase
    {
        private readonly ILogger<VideoHub> logger;
        private readonly IVideoPlaylistService videoPlaylistService;
        private readonly IVideoQualityService videoQualityService;
        public VideoHub(
            ILogger<VideoHub> logger,
            IVideoQualityService videoQualityService,
            IVideoPlaylistService videoPlaylistService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.videoQualityService = videoQualityService ?? throw new ArgumentNullException(nameof(videoQualityService));
            this.videoPlaylistService = videoPlaylistService ?? throw new ArgumentNullException(nameof(videoPlaylistService));
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

        public async Task SendVideoAsync(string name)
        {
            var video = await this.videoPlaylistService.InsertVideoToPlaylistAsync(new ModelsDB.VideoPlaylist()
            {
                CreatedDateUtc = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Name = name
            });

            await this.Clients.All.SendAsync("ReceiveVideo", 
                new VideoModel() { 
                    Id = video.Id,
                    Name = name
                }).ConfigureAwait(false);
        }

        public async Task SendVideoQualityAsync(string videoId, string url, string name)
        {
            var videoQuallity = await this.videoQualityService.InsertVideoQualityAsync(new ModelsDB.VideoQuality()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url,
                VideoId = new Guid(videoId),
                CreatedDateUtc = DateTime.UtcNow
            });

            var model = new VideoQualityModel()
            {
                Id = videoQuallity.Id,
                Name = name,
                Url = url,
                VideoId = new Guid(videoId)
            };

            await this.Clients.All.SendAsync("ReceiveVideoQuality", model).ConfigureAwait(false);
        }

        public async Task SendNewVideoTimeStampAsync(string videoId, string timestamp, bool isForce)
        {
            var model = new VideoTimeStampModel()
            {
                VideoId = new Guid(videoId),
                TimeStamp = timestamp,
                IsForce = isForce
            };

            await this.Clients.All.SendAsync("ReceiveNewVideoTimeStamp", model).ConfigureAwait(false);
        }

        public async Task SendСhangeVideoAsync(string videoId, string timestamp)
        {
            var model = new VideoTimeStampModel()
            {
                VideoId = new Guid(videoId),
                TimeStamp = timestamp,
                IsForce = true
            };

            await this.Clients.All.SendAsync("ReceiveChangeVideo", model).ConfigureAwait(false);
        }

        public async Task SendStartVideoAsync(string videoId, string timestamp)
        {
            var model = new VideoTimeStampModel()
            {
                VideoId = new Guid(videoId),
                TimeStamp = timestamp,
                IsForce = true
            };

            await this.Clients.All.SendAsync("ReceiveStartVideo", model).ConfigureAwait(false);
        }

        public async Task SendStopVideoAsync(string videoId, string timestamp)
        {
            var model = new VideoTimeStampModel()
            {
                VideoId = new Guid(videoId),
                TimeStamp = timestamp,
                IsForce = true
            };

            await this.Clients.All.SendAsync("ReceiveStopVideo", model).ConfigureAwait(false);
        }

        public async Task SendUserStatusAsync(int userStatus, string videoTimeStamp, string additionalData)
        {
            await this.Clients.AllExcept(this.Context.ConnectionId).SendAsync("ReceiveUserStatus", new UserVideoStatus() { 
                Status = userStatus,
                UserId = this.UserId,
                UserName = this.UserName,
                VideoTimeStamp = videoTimeStamp,
                AdditionalData = additionalData
            }).ConfigureAwait(false);
        }
    }
}
