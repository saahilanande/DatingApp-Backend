using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Serives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext dataContext, ITokenService tokenService)
    {
        _context = dataContext;
        _tokenService = tokenService;
    }

    [HttpPost("register")] //api/account/register
    public async Task<ActionResult<DtoUser>> Register(DtoRegister dtoRegister)
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

        return new DtoUser{
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };

    }

    [HttpPost("login")]
    public async Task<ActionResult<DtoUser>> Login(DtoLogin dtoLogin)
    {

        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == dtoLogin.UserName);

        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.passwordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dtoLogin.Password));

        for (int i = 0; i <= computedHash.Length; i++)
        {
            if (computedHash[i] != user.passwordHash[i]) return Unauthorized("invalid Password");
        }

         return new DtoUser{
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };

    }

    public async Task<bool> CheckUser(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username);
    }


}
