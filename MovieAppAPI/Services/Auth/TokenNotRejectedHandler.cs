using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Authorization;

namespace MovieAppAPI.Services.Auth;

public class TokenNotRejectedAuthorizationHandler : AuthorizationHandler<TokenNotRejectedRequirement, HttpContext> {
    private readonly ITokenService _tokenService;

    public TokenNotRejectedAuthorizationHandler(ITokenService tokenService) {
        _tokenService = tokenService;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        TokenNotRejectedRequirement requirement, HttpContext httpContext) {
        if (!httpContext.Request.Headers.ContainsKey("Authorization")) {
            context.Fail(new AuthorizationFailureReason(this, "No authorization header"));
        }

        string authHeader = httpContext.Request.Headers.Authorization;

        if (string.IsNullOrEmpty(authHeader)) {
            context.Fail(new AuthorizationFailureReason(this, "Empty authorization header"));
        }

        const string authType = "bearer";

        if (!authHeader.StartsWith(authType, StringComparison.OrdinalIgnoreCase)) {
            context.Fail(new AuthorizationFailureReason(this, "Token type is invalid"));
        }

        var token = authHeader[authType.Length..];

        if (string.IsNullOrEmpty(token)) {
            context.Fail(new AuthorizationFailureReason(this, "Token not provided"));
        }

        try {
            var isValid = _tokenService.IsTokenValid(token);
            if (isValid.Result) {
                context.Succeed(requirement);
            }
        }
        catch (Exception e) {
            if (e.InnerException != null) ExceptionDispatchInfo.Capture(e.InnerException).Throw();
        }

        return Task.CompletedTask;
    }
}