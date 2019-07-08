using Back.DAL.Models;
using System;

namespace BourseApi.Contract
{
    /// <summary>
    /// لایه دسترسی به داده روی دیتابیس __Sys__
    /// </summary>
    public interface IAuthenticationContract
    {
        /// <summary>
        /// تأیید هویت
        /// </summary>
        /// <param name="loginModel">اطلاعات کاربر</param>
        /// <param name="clientIPAddress">آی پی کلاینت</param>
        /// <returns>اطلاعات کاربر</returns>
        Tuple<AuthenticationResult, User> AuthenticateUser(LoginViewModel loginModel, string clientIPAddress);

        /// <summary>
        /// رشته معادل نتیجه تأیید هویت را بر می گرداند
        /// </summary>
        /// <param name="authResult">کد نتیجه</param>
        /// <param name="langCode">کد زبان</param>
        /// <returns></returns>
        string GetAuthenticationResultMessage(AuthenticationResultCode authResult, string langCode);

        /// <summary>
        /// استخراج اطلاعات کاربر
        /// </summary>
        /// <param name="userid">شناسه کاربر</param>
        /// <returns>اطلاعات کاربر</returns>
        User GetUser(int userid);
        
        /// <summary>
        /// استخراج اطلاعات توکن کاربر از روی توکن به روزآوری
        /// </summary>
        /// <param name="userid">شناسه کاربر</param>
        /// <param name="sessionId">شناسه سشن</param>
        /// <param name="exceptionStr"></param>
        /// <returns>اگر session ی متناظر توکن وجود داشته باشه اطلاعات متناظر بر می گردد</returns>
        UserSession GetUserSession(int userid, Guid sessionId, out string exceptionStr);


        /// <summary>
        /// فهرست تمام سشنهای فعال کاربر
        /// </summary>
        /// <param name="userid">شناسه کاربر</param>
        /// <param name="exceptionStr"></param>
        /// <returns>فهرست تمام سشنهای فعال کاربر</returns>
        UserSession[] GetUserSessions(int userid, out string exceptionStr);

        /// <summary>
        /// شناسه بعدی جلسه کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>شناسه</returns>
        Guid GetNextSessionId(int userId);


        /// <summary>
        /// ذخیره اطلاعات جلسه کاربر
        /// </summary>
        /// <param name="tadbirUserSession">اطلاعات جلسه کاربر</param>
        /// <param name="exceptionStr"></param>
        /// <returns>نتیجه عملیات</returns>
        UserSession AddUserSession(UserSession tadbirUserSession, out string exceptionStr);

        /// <summary>
        /// ثبت رویداد تمدید جلسه کاربر
        /// </summary>
        /// <param name="userServiceLogin"></param>
        /// <param name="clientIPAddress"></param>
        /// <param name="exceptionStr"></param>
        /// <returns></returns>
        bool LogUserSessionRenewal(User user, string clientIPAddress, out string exceptionStr);

        /// <summary>
        /// حذف اطلاعات جلسه ورود کاربر
        /// </summary>
        /// <param name="tadbirUserSession">اطلاعات جلسه کاربر</param>
        /// <param name="exceptionStr"></param>
        /// <returns>نتیجه عملیات</returns>
        bool RemoveUserSession(UserSession tadbirUserSession, out string exceptionStr);

        /// <summary>
        /// حذف همه جلسات کاربر
        /// </summary>
        /// <param name="userid">شناسه کاربر</param>
        /// <param name="exceptionStr"></param>
        /// <returns>نتیجه عملیات</returns>
        bool RemoveUserSessions(int userid, out string exceptionStr);


        /// <summary>
        /// اضافه کردن رویداد جدید به جدول __EventLog__ تدبیر
        /// </summary>
        /// <param name="eventLog">مشخصات رویداد</param>
        /// <returns>true if successful</returns>
        bool LogEvent(EventLog eventLog);

        /// <summary>
        /// آیا کاربر ادمین است
        /// </summary>
        /// <param name="nUserId">شناسه کاربر</param>
        /// <returns>true if is admin / false if not</returns>
        bool IsAdmin(int nUserId);
    }
}
