using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;

namespace MovieAppAPI.Controllers; 

public class ControllerExtractToken : ControllerBase {
    /// <exception cref="InvalidTokenException">Throws an exception when there is no NameIdentifier claim type in token</exception>
    protected Guid GetUserIdFromToken() {
        var id = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (id is null) {
            throw ExceptionHelper.InvalidTokenException("Token doesn't have valid claim type");
        }
        
        return Guid.Parse(id);
    }
}