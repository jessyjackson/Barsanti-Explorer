using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Entities
{
    public class User: BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }
        
        [MaxLength(64)]
        [Required]
        public string Password { get; set; }
        
        public int? TelegramId { get; set; }
    }
}
