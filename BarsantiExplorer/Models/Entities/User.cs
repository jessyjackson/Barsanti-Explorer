using System.ComponentModel.DataAnnotations;
using BarsantiExplorer.Models.Responses;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
        
        public UserResponse MapToUserResponse()
        {
            return new UserResponse
            {
                Email = Email,
                TelegramId = TelegramId
            };
        } 
    }
}
