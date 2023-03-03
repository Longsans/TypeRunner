using System.Security.Claims;

namespace TypeRunnerBE.Services.Auth
{
    public interface ITokenBasedAuthService<TTokenType>
    {
        TTokenType CreateAccessToken(params Claim[] claims);
        TTokenType CreateRefreshToken(params Claim[] claims);
    }
}
