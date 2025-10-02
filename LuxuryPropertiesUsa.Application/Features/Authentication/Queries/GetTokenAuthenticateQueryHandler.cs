using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LuxuryPropertiesUsa.Application.Features.Authentication.Queries;

public class GetTokenAuthenticateQueryHandler(IConfiguration config) : IRequestHandler<GetTokenAuthenticateQuery, ApiResponseUtility<string>>
{
    /// <summary>
    /// This method is responsible for generating a token to authenticate and consume the APIs.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Token to consume the APIs.</returns>
    public Task<ApiResponseUtility<string>> Handle(GetTokenAuthenticateQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("SecretsValues:AuthKey").Value ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.UserId),
            };

            var token = new JwtSecurityToken(config.GetSection("SecretsValues:AuthIssuer").Value,
                config.GetSection("SecretsValues:AuthAudience").Value,
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt16(config.GetSection("SecretsValues:MinExpire").Value ?? "10")),
                signingCredentials: credentials);

            var response = new ApiResponseUtility<string>(new JwtSecurityTokenHandler().WriteToken(token))
            {
                IsSuccess = true,
                ReturnMessage = "The token has been successfully generated."
            };

            return Task.FromResult(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponseUtility<string>(ex.InnerException == null ? ex.Message : ex.InnerException.Message)
            {
                IsSuccess = false,
                ReturnMessage = "Error getting token."
            };
            
            return Task.FromResult(response);
        }
    }
}