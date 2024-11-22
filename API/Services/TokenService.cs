﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config): ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new ("Cannot access TokenKey from appsettings");
        if(tokenKey.Length < 64) throw new Exception("Invalid token key length, need to be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.UserName), //Se simplificó de new Claim() no se necesita especificar en las nuevas versiones

        };
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}