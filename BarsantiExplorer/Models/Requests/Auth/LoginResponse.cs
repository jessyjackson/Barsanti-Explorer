using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Responses;

namespace BarsantiExplorer.Models.Requests.Auth;

public class LoginResponse
{
    public UserResponse User { get; set; }
    public string Token { get; set; }
}