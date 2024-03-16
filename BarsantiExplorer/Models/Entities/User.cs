namespace BarsantiExplorer.Models.Entities
{
    public class User: BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int TelegramId { get; set; }
    }
}
