using System.Security.Cryptography;
using System.Text;
using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseController
{
    private readonly DataContext _context;

    public AccountController(DataContext dataContext)
    {
        _context = dataContext;
    }

    [HttpPost("register")] //api/account/register
    public async Task<ActionResult<AppUser>> Register(string username, string password)
    {
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = username,
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            passwordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;

    }


}
