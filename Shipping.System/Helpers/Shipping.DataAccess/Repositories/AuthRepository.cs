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
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityRepository _identityRepository; 
    private readonly IUserRepository _userRepository;
    private readonly JWT _jwt;

    public AuthRepository(UserManager<AppUser> userManager, IOptions<JWT> jwt, IIdentityRepository identityRepository, IUserRepository userRepository)
    {
        _userManager = userManager;
        _identityRepository = identityRepository;
        _userRepository = userRepository;
        _jwt = jwt.Value;
    }

    public async Task<Result<string>> SingUp(SingUpRequest request, CancellationToken cancellationToken)
    {
        var identityUser = await _identityRepository.SingUp(new SingUpCommnd()
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

        var userProfile = await _userRepository.InsertUserAsync(new InsertAndUpdateUserCommnd()
        {
            UserId = Guid.Parse(identityUser.Value.Id),
            UserName = identityUser.Value.UserName,
            UserType = identityUser.Value.UserType,
            Password = request.Password,
        }, cancellationToken);
        
        if (!userProfile.IsSuccess)
            return Result.Fail(userProfile.Errors.ToList());

        var customer = await _userRepository.InsertCustomerAsync(new InsertAndUpdateCustomerCommnd()
        {
            UserId = Guid.Parse(userProfile.Value),
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Name = request.FirstName + " " + request.LastName,
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
    
    private async Task<JwtSecurityToken> CreateJwtToken(AppUser appUser)
    {
        var userClaims = await _userManager.GetClaimsAsync(appUser);
        var roles = await _userManager.GetRolesAsync(appUser);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));
        
        var claims = new[]
            {
                new Claim("userId", appUser.Id),
                new Claim("userName", appUser.UserName)
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