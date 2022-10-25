using Microsoft.EntityFrameworkCore;
using NotAlone.WebApi.Infrastructure.DB;
using NotAlone.WebApi.Infrastructure.Models.Configuration;
using NotAlone.WebApi.ModelsDB;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Services.Implementation
{
    public class VideoQualityService : IVideoQualityService
    {
        private readonly ILogger<VideoQualityService> logger;
        private readonly IUserService userService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly NotAloneDbContext dbContext;
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        public VideoQualityService(
            ILogger<VideoQualityService> logger,
            IUserService userService,
            IRefreshTokenService refreshTokenService,
            NotAloneDbContext dbContext,
            JwtTokenConfiguration jwtTokenConfiguration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.jwtTokenConfiguration = jwtTokenConfiguration ?? throw new ArgumentNullException(nameof(jwtTokenConfiguration));
        }

        public async Task<List<VideoQuality>> GetQualityVideosByVideoIdAsync(Guid videoId)
        {
            var videoQualities = await this.dbContext.VideoQualities.Where(vq => vq.VideoId == videoId).ToListAsync();
            return videoQualities;
        }

        public async Task<VideoQuality> InsertVideoQualityAsync(VideoQuality videoQuality)
        {
            videoQuality.Id = Guid.NewGuid();
            await this.dbContext.VideoQualities.AddAsync(videoQuality);
            await this.dbContext.SaveChangesAsync();
            return videoQuality;
        }

        public async Task DeleteVideoFromPlaylistAsync(Guid Id)
        {
            var video = await this.dbContext.VideoQualities.Where(vp => vp.Id == Id).FirstOrDefaultAsync();
            if (video != null && video.Id != Guid.Empty)
            {
                this.dbContext.VideoQualities.Remove(video);
                this.dbContext.SaveChanges();
            }
        }
    }
}
