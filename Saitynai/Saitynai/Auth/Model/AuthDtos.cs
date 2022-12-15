using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace Saitynai.Auth.Model;

public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);

public record LoginDto(string UserName, string Password);

public record UserDto(string Id, string UserName, string Email);

public class SuccessfulLoginDto
{
    public string UserId {get; set;}
    public string UserTitle {get; set;}
    public string AccessToken {get; set;}

    public SuccessfulLoginDto(string userId, string userTitle, string accessToken)
    {
        UserId = userId;
        UserTitle = userTitle;
        AccessToken = accessToken;
    }
}
