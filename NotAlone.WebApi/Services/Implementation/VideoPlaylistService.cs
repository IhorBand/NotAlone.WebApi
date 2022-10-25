using Microsoft.EntityFrameworkCore;
using NotAlone.WebApi.Infrastructure.DB;
using NotAlone.WebApi.Infrastructure.Models.Configuration;
using NotAlone.WebApi.ModelsDB;
using NotAlone.WebApi.ModelsWebApi.Authorize;
using NotAlone.WebApi.Services.Abstractions;

namespace NotAlone.WebApi.Services.Implementation
{
    public class VideoPlaylistService : IVideoPlaylistService
    {
        private readonly ILogger<VideoPlaylistService> logger;
        private readonly IUserService userService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly NotAloneDbContext dbContext;
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        public VideoPlaylistService(
            ILogger<VideoPlaylistService> logger,
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

        public async Task<List<VideoPlaylist>> GetVideosFromPlaylistAsync()
        {
            var videoPlaylist = await this.dbContext.VideoPlaylists.ToListAsync();
            return videoPlaylist;
        }

        public async Task<VideoPlaylist> InsertVideoToPlaylistAsync(VideoPlaylist videoPlaylist)
        {
            videoPlaylist.Id = Guid.NewGuid();
            await this.dbContext.VideoPlaylists.AddAsync(videoPlaylist);
            await this.dbContext.SaveChangesAsync();
            return videoPlaylist;
        }

        public async Task DeleteVideoFromPlaylistAsync(Guid Id)
        {
            var video = await this.dbContext.VideoPlaylists.Where(vp => vp.Id == Id).FirstOrDefaultAsync();
            if (video != null && video.Id != Guid.Empty)
            {
                this.dbContext.VideoPlaylists.Remove(video);
                this.dbContext.SaveChanges();
            }
        }
    }
}
