using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MovieAppAPI.Services.Auth;

public class TokenNotRejectedAuthorizationHandler : AuthorizationHandler<TokenNotRejectedRequirement, HttpContext> {
    private readonly ITokenService _tokenService;
    private const string TokenType = "access_token";

    public TokenNotRejectedAuthorizationHandler(ITokenService tokenService) {
        _tokenService = tokenService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        TokenNotRejectedRequirement requirement, HttpContext httpContext) {
        var token = await httpContext.GetTokenAsync(TokenType);

        if (token is null) {
            context.Fail();
        }
        else {
            try {
                var isValid = _tokenService.IsTokenValid(token);
                if (isValid.Result) {
                    context.Succeed(requirement);
                }
                else {
                    context.Fail(new AuthorizationFailureReason(this, "Provided token was rejected"));
                }
            }
            catch (ArgumentException e) {
                // if (e.InnerException != null) ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                context.Fail(new AuthorizationFailureReason(this, "Provided token is invalid"));
            }
        }

        // if (!httpContext.Request.Headers.ContainsKey("Authorization")) {
        //     context.Fail(new AuthorizationFailureReason(this, "No authorization header"));
        //     httpContext.Response.StatusCode = 401;
        //     return Task.CompletedTask;
        // }
        //
        // string authHeader = httpContext.Request.Headers.Authorization;
        //
        // if (string.IsNullOrEmpty(authHeader)) {
        //     context.Fail(new AuthorizationFailureReason(this, "Empty authorization header"));
        //     httpContext.Response.StatusCode = 401;
        //     return Task.CompletedTask;
        // }
        //
        // const string authType = "bearer";
        //
        // if (!authHeader.StartsWith(authType, StringComparison.OrdinalIgnoreCase)) {
        //     context.Fail(new AuthorizationFailureReason(this, "Token type is invalid"));
        //     httpContext.Response.StatusCode = 401;
        //     return Task.CompletedTask;
        // }
        //
        // var token = authHeader[authType.Length..];
        //
        // if (string.IsNullOrEmpty(token)) {
        //     context.Fail(new AuthorizationFailureReason(this, "Token not provided"));
        //     httpContext.Response.StatusCode = 401;
        //     return Task.CompletedTask;
        // }
        //
        // try {
        //     var isValid = _tokenService.IsTokenValid(token);
        //     if (isValid.Result) {
        //         context.Succeed(requirement);
        //     }
        //     else {
        //         context.Fail(new AuthorizationFailureReason(this, "Provided token was rejected"));
        //         httpContext.Response.StatusCode = 401;
        //     }
        // }
        // catch (ArgumentException e) {
        //     // if (e.InnerException != null) ExceptionDispatchInfo.Capture(e.InnerException).Throw();
        //     context.Fail(new AuthorizationFailureReason(this, "Provided token is invalid"));
        //     httpContext.Response.StatusCode = 401;
        // }
        //
        // return Task.CompletedTask;
    }
}