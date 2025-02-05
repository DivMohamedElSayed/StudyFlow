namespace StudyFlow.API.Repository.Implementations;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expireIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub,user.Id),
            new(JwtRegisteredClaimNames.GivenName,user.FirstName!),
            new(JwtRegisteredClaimNames.FamilyName,user.LastName!),
            new(JwtRegisteredClaimNames.Email,user.Email!),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new("ThemePreference",user.ThemePreference!)
        ];
        var encodingOptions = Encoding.UTF8.GetBytes(_options.AccessToken);
        var symmetricSecurityKey = new SymmetricSecurityKey(encodingOptions);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _options?.Issuer,
            audience: _options?.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options!.ExpireIn),
            signingCredentials: signingCredentials);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return (jwtSecurityTokenHandler, _options.ExpireIn * 60);
    }

    public string? ValidateToken(string token)
    {
        var encodingOptions = Encoding.UTF8.GetBytes(_options.AccessToken);
        var symmetricSecurityKey = new SymmetricSecurityKey(encodingOptions);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        try
        {
            jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken securityToken);
            var jwtSecurityToken = (securityToken as JwtSecurityToken);
            return jwtSecurityToken!.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }
}