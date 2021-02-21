using Bathhouse.Entities;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bathhouse.Identity.Api.Services
{
  public class ProfileService : IProfileService
  {
    private readonly UserManager<Employee> _userManager;

    public ProfileService(UserManager<Employee> userManager)
    {
      _userManager = userManager;
    }

    async public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      var subject = context.Subject ?? throw new ArgumentNullException(nameof(context));

      var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

      var user = await _userManager.FindByIdAsync(subjectId);
      if (user == null)
        throw new ArgumentException("Invalid subject identifier");

      var claims = GetClaimsFromUser(user);
      context.IssuedClaims.AddRange(claims.ToList());
    }

    async public Task IsActiveAsync(IsActiveContext context)
    {
      var subject = context.Subject ?? throw new ArgumentNullException(nameof(context));

      var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
      var user = await _userManager.FindByIdAsync(subjectId);

      context.IsActive = false;

      if (user != null)
      {
        if (_userManager.SupportsUserSecurityStamp)
        {
          var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
          if (security_stamp != null)
          {
            var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
            if (db_security_stamp != security_stamp)
              return;
          }
        }

        context.IsActive =
            !user.LockoutEnabled ||
            !user.LockoutEnd.HasValue ||
            user.LockoutEnd <= DateTime.Now;
      }
    }

    private IEnumerable<Claim> GetClaimsFromUser(Employee user)
    {
      var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.ShortName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim("lastname", user.LastName),

            };

      claims.AddRange(_userManager.GetRolesAsync(user).Result
          .Select(r=> new Claim(JwtClaimTypes.Role, r)));

      if (!string.IsNullOrWhiteSpace(user.FirstName))
        claims.Add(new Claim("firstname", user.FirstName));

      if (!string.IsNullOrWhiteSpace(user.FirstName))
        claims.Add(new Claim(JwtClaimTypes.MiddleName, user.MiddleName));

      if (!string.IsNullOrWhiteSpace(user.UserName))
        claims.Add(new Claim("username", user.UserName));

      if (!string.IsNullOrWhiteSpace(user.FullName))
        claims.Add(new Claim("name", user.FullName));

      if (user.DoB is not null)
        claims.Add(new Claim(JwtClaimTypes.BirthDate, user.DoB.Value.ToShortDateString()));

      if (_userManager.SupportsUserEmail)
      {
        claims.AddRange(new[]
        {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
      }

      if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
      {
        claims.AddRange(new[]
        {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
      }

      return claims;
    }
  }
}
