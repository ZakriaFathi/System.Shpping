using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Auth.Commands.SingIn;
using Shipping.Application.Features.Auth.Commands.SingUp;
using Shipping.Application.Models.IdentityModel;
using Shipping.Application.Models.UserManagement;
using Shipping.Domain.Models;
using Shipping.Utils.Enums;
using Shipping.Utils.Options;

namespace Shipping.DataAccess.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ISherdUserRepository _sherdUserRepository; 

    private readonly JWT _jwt;

    public AuthRepository(UserManager<User> userManager, IOptions<JWT> jwt, ISherdUserRepository sherdUserRepository)
    {
        _userManager = userManager;
        _sherdUserRepository = sherdUserRepository;
        _jwt = jwt.Value;
    }

    public async Task<Result<string>> SingUp(SingUpRequest request, CancellationToken cancellationToken)
    {
        var identityUser = await _sherdUserRepository.SingUp(new SingUpCommnd()
        {
            LastName = request.LastName,
            FirstName = request.FirstName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            UserName = request.UserName,
            Password = request.Password,
        }, cancellationToken);
        
        if (!identityUser.IsSuccess)
            return Result.Fail(identityUser.Errors.ToList());

        await _sherdUserRepository.InsertCustomerAsync(new InsertAndUpdateUserCommnd()
        {
            UserId = Guid.Parse(identityUser.Value.Id),
            Password = request.Password,
            UserName = request.UserName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Name = request.FirstName + " " + request.LastName,
            UserType = identityUser.Value.UserType
        }, cancellationToken);

        return " تمت عملية انشاء حساب ";
    }

    public async Task<Result<SingInResponse>> SingInByUserName(SingInRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return Result.Fail(new List<string>() { "هذا المستخدم غير موجود" });

        var password = await _userManager.CheckPasswordAsync(user, request.Password);
        if (password == false)
            return Result.Fail(new List<string>() { "كلمة المرور السابقة غير صحيحة" });

        if (user.ActivateState == ActivateState.InActive)
            return Result.Fail(new List<string>() { "هذا المستخدم غير مفعل" });   
        
        var jwtSecurityToken = await CreateJwtToken(user);
        var rolesList = await _userManager.GetRolesAsync(user);

        return new SingInResponse()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName,
            Roles = rolesList.ToList(),
            ExpiresOn = jwtSecurityToken.ValidTo
        };
    }
    
    private async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));
        
        var claims = new[]
            {
                new Claim("userId", user.Id),
                new Claim("userName", user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(_jwt.DurationInHours),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}