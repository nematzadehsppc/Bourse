using BourseApi.Contract;
using Back.DAL.Models;
using System;
using Back.DAL.Context;
using System.Linq;
using System.Security.Cryptography;

namespace BourseApi.Repositories
{
    public class AuthenticationRepository : IAuthenticationContract
    {
        private UAppContext _dbContext;

        public AuthenticationRepository(UAppContext context)
        {
            _dbContext = context;
        }

        public UserSession AddUserSession(UserSession userSession, out string exceptionStr)
        {
            throw new NotImplementedException();
        }

        public Tuple<AuthenticationResult, User> AuthenticateUser(LoginViewModel loginModel, string clientIPAddress)
        {
            if (loginModel.Username == null || loginModel.Password == null || loginModel.ServiceAccessType == ServiceAccesType.None)
                return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.InvalidInputParams), null);

            User _user = _dbContext.Users.FirstOrDefault(user => user.Name == loginModel.Username && user.Password == loginModel.Password);

            if (_user != null)
            {
                byte[] bHashPassword = MD5.Create().ComputeHash(new System.Text.ASCIIEncoding().GetBytes(_user.Password));
                string hashPassword = "";
                for (int index = 0; index < bHashPassword.Length; index++)
                {
                    hashPassword = string.Format("{0:X2}", bHashPassword[index]);
                }

                if (hashPassword == _user.Password)
                {
                    return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.AuthenticationSuccess), _user);
                }

                return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.AuthenticationFailure), _user);
            }

            return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.AuthenticationFailure), null);
        }

        public string GetAuthenticationResultMessage(AuthenticationResultCode authResult, string langCode)
        {
            langCode = langCode.ToLower();
            switch (authResult)
            {
                case AuthenticationResultCode.AuthenticationFailure:
                    if (langCode == "fa-ir")
                        return "نام کاربری یا کلمهٔ عبور اشتباه است.";
                    else
                        return "Username and password combination did not work!";

                case AuthenticationResultCode.AuthenticationSuccess:
                    if (langCode == "fa-ir")
                        return "ورود موفقیت آمیز بود.";
                    else
                        return "Authentication succeeded.";

                case AuthenticationResultCode.DbConnectionError:
                    if (langCode == "fa-ir")
                        return "خطای اتصال سرویس به پایگاه داده‌ها. لطفا بررسی کنید که رشته اتصال ConnectionStrings.SysConnection در appsettings.json رشته صحیحی برگرداند.";
                    else
                        return "Database connection error. Please make sure ConnectionStrings.SysConnection in appsettings.json points to a valid and online database server.";

                case AuthenticationResultCode.InvalidInputParams:
                    if (langCode == "fa-ir")
                        return "خطای برنامه‌نویسی: پارامترهای ورودی متد ناصحیح است.";
                    else
                        return "Invalid input parameters for api.";

                case AuthenticationResultCode.InvalidServiceAccessType:
                    if (langCode == "fa-ir")
                        return "کاربر مورد نظر به سرویس درخواست شده دسترسی ندارد.";
                    else
                        return "User has not required access.";

                case AuthenticationResultCode.UnknownServerError:
                    if (langCode == "fa-ir")
                        return "خطای پردازش نشده.";
                    else
                        return "Uknown error";
            }
            return "Invalid result code.";
        }

        public Guid GetNextSessionId(int userId)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int userid)
        {
            throw new NotImplementedException();
        }

        public UserSession GetUserSession(int userid, Guid sessionId, out string exceptionStr)
        {
            throw new NotImplementedException();
        }

        public UserSession[] GetUserSessions(int userid, out string exceptionStr)
        {
            throw new NotImplementedException();
        }

        public bool IsAdmin(int nUserId)
        {
            throw new NotImplementedException();
        }

        public bool LogEvent(EventLog eventLog)
        {
            throw new NotImplementedException();
        }

        public bool LogUserSessionRenewal(User user, string clientIPAddress, out string exceptionStr)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserSession(UserSession userSession, out string exceptionStr)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserSessions(int userid, out string exceptionStr)
        {
            throw new NotImplementedException();
        }
    }
}