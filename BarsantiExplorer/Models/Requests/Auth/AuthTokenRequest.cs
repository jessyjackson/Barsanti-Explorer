using System.ComponentModel.DataAnnotations;

namespace BarsantiExplorer.Models.Requests.Auth
{
    public class AuthTokenRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}