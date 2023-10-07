using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseController
{
    private readonly DataContext _context;

    public AccountController(DataContext dataContext)
    {
        _context = dataContext;
    }

    [HttpPost("register")] //api/account/register
    public async Task<ActionResult<AppUser>> Register(DtoRegister dtoRegister)
    {
        using var hmac = new HMACSHA512();

        if (await CheckUser(dtoRegister.UserName)) return BadRequest("Username Taken");

        var user = new AppUser
        {
            UserName = dtoRegister.UserName,
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dtoRegister.Password)),
            passwordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;

    }

    public async Task<bool> CheckUser(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username);
    }


}
