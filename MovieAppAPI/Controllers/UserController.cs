using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Entities;

namespace MovieAppAPI.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<List<User>>> Get() {
        var users = new List<User> {
            new(
                "Jjjjerker", ComputeSha256Hash("12345"),
                "someMail@mail.com", gender: Gender.Male,
                birthDate: new DateTime(2003, 8, 15)),
            new(
                "Mmmmmmasturbateress", ComputeSha256Hash("complexPassword124_2@"),
                "anotherMail@mail.com", gender: Gender.Female,
                birthDate: new DateTime(2001, 3, 11)
            )
        };

        return Ok(users);
    }

    private static string ComputeSha256Hash(string rawString) {
        var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawString));
        var builder = new StringBuilder(bytes.Length);

        foreach (var b in bytes) builder.Append(b.ToString("X"));

        return builder.ToString();
    }
}