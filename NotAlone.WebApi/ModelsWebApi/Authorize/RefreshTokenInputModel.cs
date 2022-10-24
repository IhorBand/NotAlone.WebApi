namespace NotAlone.WebApi.ModelsWebApi.Authorize
{
    public class RefreshTokenInputModel
    {
        public Guid RefreshToken { get; set; }
        public Guid UserId { get; set; }
    }
}
