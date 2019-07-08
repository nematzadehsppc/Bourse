using Back.DAL.Models;
using System;

namespace BourseApi.Contract
{
    /// <summary>
    /// تولید و مدیریت توکنهای مربوط به تأیید هویت کاربر
    /// </summary>
    public interface ITokenServiceContract
    {
        /// <summary>
        /// تولید توکن امنیتی و همینطور یک رشته تصادفی برای به‌روزآوری توکن
        /// </summary>
        /// <param name="userInfo">اطلاعات کاربر</param>
        /// <param name="sessionId">شناسه جلسه کاربر</param>
        /// <returns>زوج شامل رشته توکن امنیتی و رشته تصادفی</returns>
        string GenerateToken(User user, Guid sessionId);

        /// <summary>
        /// به‌روزآوری توکن اکسپایر شده
        /// </summary>
        /// <param name="token">رشته توکن اکسپایر شده</param>
        /// <param name="exceptionStr"></param>
        /// <returns>زوج شامل رشته توکن امنیتی و رشته تصادفی</returns>
        string RegenerateToken(string token, out string exceptionStr);

        /// <summary>
        /// استخراج اطلاعات کاربر از توکن امنیتی تولید شده
        /// </summary>
        /// <param name="token">رشته توکن</param>
        /// <returns>اطلاعات کاربر</returns>
        User GetUserServiceLoginFromToken(string token);
    }
}
