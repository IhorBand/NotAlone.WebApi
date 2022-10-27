using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotAlone.WebApi.Controllers.Base;
using NotAlone.WebApi.ModelsWebApi.Authorize;
using NotAlone.WebApi.ModelsWebApi.Video;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class VideoController : UserControllerBase
    {

        private readonly ILogger<VideoController> logger;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly IVideoPlaylistService videoPlaylistService;
        private readonly IVideoQualityService videoQualityService;

        public VideoController(
            ILogger<VideoController> logger,
            IMapper mapper,
            ITokenService tokenService,
            IVideoPlaylistService videoPlaylistService,
            IVideoQualityService videoQualityService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.videoPlaylistService = videoPlaylistService;
            this.videoQualityService = videoQualityService;
        }

        [HttpGet("Videos")]
        public async Task<IActionResult> GetVideos()
        {
            var videos = await this.videoPlaylistService.GetVideosFromPlaylistAsync().ConfigureAwait(false);

            var model = new List<VideoModel>();

            foreach(var video in videos)
            {
                model.Add(new VideoModel() { 
                    Id = video.Id,
                    Name = video.Name
                });
            }

            return this.Ok(model);
        }

        [HttpGet("Video/{videoId}/Qualities")]
        public async Task<IActionResult> GetQualities([FromRoute(Name = "videoId")] Guid videoId)
        {
            var qualities = await this.videoQualityService.GetQualityVideosByVideoIdAsync(videoId).ConfigureAwait(false);

            var model = new List<VideoQualityModel>();

            foreach (var quality in qualities)
            {
                model.Add(new VideoQualityModel()
                {
                    Id = quality.Id,
                    Name = quality.Name,
                    Url = quality.Url,
                    VideoId = quality.VideoId
                });
            }

            return this.Ok(model);
        }

        [HttpGet("Video/Qualities")]
        public async Task<IActionResult> GetAllQualities()
        {
            var qualities = await this.videoQualityService.GetAllQualitiesAsync().ConfigureAwait(false);

            var model = new List<VideoQualityModel>();

            foreach (var quality in qualities)
            {
                model.Add(new VideoQualityModel()
                {
                    Id = quality.Id,
                    Name = quality.Name,
                    Url = quality.Url,
                    VideoId = quality.VideoId
                });
            }

            return this.Ok(model);
        }
    }
}
