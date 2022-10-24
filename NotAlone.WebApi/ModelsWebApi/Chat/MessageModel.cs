namespace NotAlone.WebApi.ModelsWebApi.Chat
{
    public class MessageModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
