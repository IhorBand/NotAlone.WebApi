using NotAlone.WebApi.ModelsDB;

namespace NotAlone.WebApi.Services.Abstractions
{
    public interface IVideoPlaylistService
    {
        public Task<List<VideoPlaylist>> GetVideosFromPlaylistAsync();
        public Task<VideoPlaylist> InsertVideoToPlaylistAsync(VideoPlaylist videoPlaylist);
        public Task DeleteVideoFromPlaylistAsync(Guid Id);
    }
}
