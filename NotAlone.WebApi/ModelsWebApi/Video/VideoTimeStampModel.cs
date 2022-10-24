namespace NotAlone.WebApi.ModelsWebApi.Video
{
    public class VideoTimeStampModel
    {
        public Guid VideoId { get; set; }
        public string TimeStamp { get; set; }
        public bool IsForce { get; set; }
    }
}
