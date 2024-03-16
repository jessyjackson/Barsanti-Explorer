using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Entities
{
    public class User: BaseEntity
    {
        [MaxLength(100)]
        public string Email { get; set; }
        
        [MaxLength(64)]
        public string Password { get; set; }
        
        public int? TelegramId { get; set; }
    }
}
