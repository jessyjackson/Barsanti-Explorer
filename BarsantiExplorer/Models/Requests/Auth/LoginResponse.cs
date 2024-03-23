using BarsantiExplorer.Models.Entities;

namespace BarsantiExplorer.Models.Requests.Auth;

public class LoginResponse
{
    public User User { get; set; }
    public string Token { get; set; }
}