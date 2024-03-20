using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BarsantiExplorer.Services
{
    public class JwtOptions
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SigningKey { get; set; }
        public int ExpirationHours { get; set; }

        internal SecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SigningKey));
        }
    }
}
