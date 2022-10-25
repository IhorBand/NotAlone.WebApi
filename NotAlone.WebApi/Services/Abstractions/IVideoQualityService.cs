using NotAlone.WebApi.ModelsDB;

namespace NotAlone.WebApi.Services.Abstractions
{
    public interface IVideoQualityService
    {
        public Task<List<VideoQuality>> GetQualityVideosByVideoIdAsync(Guid videoId);
        public Task<VideoQuality> InsertVideoQualityAsync(VideoQuality videoQuality);
        public Task DeleteVideoFromPlaylistAsync(Guid Id);
    }
}
