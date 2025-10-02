using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Authentication.Queries;

public record GetTokenAuthenticateQuery : IRequest<ApiResponseUtility<string>>
{
    public string UserId { get; }
    public GetTokenAuthenticateQuery(string userId) => UserId = userId;
}