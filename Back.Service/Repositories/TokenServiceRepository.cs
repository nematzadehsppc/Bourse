using Back.DAL.Models;
using BourseApi.Contract;
using BourseService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BourseApi.Repositories
{
    /// <summary>
    /// تولید و مدیریت توکنهای مربوط به تأیید هویت کاربر
    /// </summary>
    public class TokenServiceRepository : ITokenServiceContract
    {
        #region Interface Implementaion

        /// <summary>
        /// تولید توکن بر اساس کاربر
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="sessionId">شناسه جلسه کاربر</param>
        /// <returns></returns>
        public string GenerateToken(User user, Guid sessionId)
        {
            var claims = new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("EMail", user.Email),
                //new Claim("AccessLevel", user.AccessLevel.ToString()),
                new Claim("SessionId", sessionId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "SPPC",                
                audience: "Everyone",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(SecurityParameters.DefaultTokenExpirationInSeconds),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityParameters.Secret)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        /// <summary>
        /// به‌روزآوری توکن اکسپایر شده
        /// </summary>
        /// <param name="token">رشته توکن اکسپایر شده</param>
        /// <param name="exceptionStr"></param>
        /// <returns>زوج شامل رشته توکن امنیتی و رشته تصادفی</returns>
        public string RegenerateToken(string token, out string exceptionStr)
        {
            exceptionStr = "";
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var username = principal.Identity.Name; //this is mapped to the Name claim by default
                var sessionId = principal.Claims.FirstOrDefault(c => c.Type == "SessionId").Value;

                //check refreshToken here to see if it is valid or not
                //check user here, if is not valid return BadRequest();

                return GenerateToken(GetUserServiceLoginFromToken(token), new Guid(sessionId));
            }
            catch(Exception exp)
            {
                exceptionStr = exp.ToString();
                return null;
            }

        }


        /// <summary>
        /// استخراج اطلاعات کاربر از توکن امنیتی تولید شده
        /// </summary>
        /// <param name="token">رشته توکن</param>
        /// <returns>اطلاعات کاربر</returns>
        public User GetUserServiceLoginFromToken(string token)
        {

            var principal = GetPrincipalFromExpiredToken(token);
            return new User()
            {
                Id = Int32.Parse(principal.Claims.FirstOrDefault(c => c.Type == "Id").Value),
                Name = principal.Identity.Name,
                Email = principal.Claims.FirstOrDefault(c => c.Type == "Email").Value,
                //AccessLevel = Int32.Parse(principal.Claims.FirstOrDefault(c => c.Type == "AccessLevel").Value),
            };
        }

        #endregion

        #region Internals


        /// <summary>
        /// استخراج اطلاعات از توکن اکسپایر شده
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = "Everyone",
                ValidateIssuer = true,
                ValidIssuer = "OMIDAN",

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityParameters.Secret)),

                ValidateLifetime = false, //مقداردهی این پارامتر با false مهم است

                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        #endregion

    }
}
