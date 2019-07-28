using BourseApi.Contract;
using Back.DAL.Models;
using Back.DAL.ViewModel;
using System;
using Back.DAL.Context;
using System.Linq;
using System.Security.Cryptography;
using BourseService;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace BourseApi.Repositories
{
    public class AuthenticationRepository : IAuthenticationContract
    {
        private UAppContext _dbContext;
        private IUserOptionContract _userOptionContract;

        public AuthenticationRepository(UAppContext context, IUserOptionContract userOptionContract)
        {
            _dbContext = context;
            _userOptionContract = userOptionContract;
        }

        public UserSession AddUserSession(UserSession userSession, out string exceptionStr)
        {
            Session session = GetUserTokenStore(userSession.UserId, out exceptionStr);
            if (session == null)
                return null;


            List<UserSession> userSessions = ReadStoredUserSessions(session.UserSession);

            if (userSession.SessionId == Guid.Empty)
            {
                userSession.SessionId = Guid.NewGuid();
                while (userSessions.Find(u => u.SessionId == userSession.SessionId) != null)
                {
                    userSession.SessionId = Guid.NewGuid();
                }
            }

            userSessions.Add(userSession);

            session.UserSession = StoreUserSessions(userSessions);

            if (!UpdateSessionContent(session, out exceptionStr))
                return null;

            return userSession;
        }

        private Boolean UpdateSessionContent(Session session, out string exceptionStr)
        {
            exceptionStr = "";
            try
            {
                session.MCheckSum = GetHashValue(session.UserSession);
                session.FileSize = session.UserSession.LongLength;

                _dbContext.Sessions.Update(session);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception exp)
            {
                exceptionStr = exp.ToString();
                return false;
            }
        }

        /// <summary>
        /// استخراج مقدار هش شده مربوط به تصویر
        /// Added By Saeedeh Taheri
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        private string GetHashValue(byte[] imageData)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(imageData);

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("X2"));
                }
            }
            return sBuilder.ToString();
        }

        public Tuple<AuthenticationResult, User> AuthenticateUser(LoginViewModel loginModel, string clientIPAddress)
        {
            if (loginModel.Username == null || loginModel.Password == null || loginModel.ServiceAccessType == ServiceAccesType.None)
                return new Tuple<AuthenticationResult, User>(new AuthenticationResult(AuthenticationResultCode.InvalidInputParams), null);

            User _user = _dbContext.Users.FirstOrDefault(user => user.UserName == loginModel.Username && user.Password == SecurityParameters.MD5Encryption(loginModel.Password));

            if (_user != null)
            {
                byte[] bHashPassword = MD5.Create().ComputeHash(new System.Text.ASCIIEncoding().GetBytes(loginModel.Password));
                string hashPassword = "";
                for (int index = 0; index < bHashPassword.Length; index++)
                {
                    hashPassword += string.Format("{0:X2}", bHashPassword[index]);
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
            Session userTokenStore = GetUserTokenStore(userId, out string exceptionStr);
            if (userTokenStore == null)
                return Guid.Empty;

            Guid id = Guid.NewGuid();
            List<UserSession> userSessions = ReadStoredUserSessions(userTokenStore.UserSession);
            while (userSessions.Find(u => u.SessionId == id) != null)
            {
                id = Guid.NewGuid();
            }

            return id;
        }

        private Session GetUserTokenStore(int userId, out string exceptionStr)
        {
            exceptionStr = "";

            int sessionId = 0;
            Guid userOptGuid = Guid.Empty;
            string optionValue = GetUserOptionValue(userId, UserOPT.USER_OPT_LOGINSESSIONSIMAGEGUID, false);
            if (optionValue != null)
            {
                string[] optValueInfoParts = optionValue.Split('$', StringSplitOptions.RemoveEmptyEntries);
                if (optValueInfoParts.Length != 2)
                    optionValue = null;
                else
                {
                    if (!Int32.TryParse(optValueInfoParts[0], out sessionId))
                        optionValue = null;
                    else
                        if (!Guid.TryParse(optValueInfoParts[1], out userOptGuid))
                        optionValue = null;
                }
            }

            if (optionValue == null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<UserSession>));


                byte[] baSessions;
                using (MemoryStream writer = new MemoryStream())
                {
                    xmlSerializer.Serialize(writer, new List<UserSession>());
                    baSessions = writer.GetBuffer();
                }

                Session newTokenInfo = new Session()
                {
                    UserSession = baSessions,
                    UserId = userId
                };


                if (newTokenInfo.SessionId == null || newTokenInfo.SessionId == Guid.Empty)
                {
                    newTokenInfo.SessionId = Guid.NewGuid();
                }
                newTokenInfo.MCheckSum = GetHashValue(newTokenInfo.UserSession);
                newTokenInfo.FileSize = newTokenInfo.UserSession.LongLength;

                _dbContext.Sessions.Add(newTokenInfo);
                _dbContext.SaveChanges();

                Session userTokenStore = newTokenInfo;
                if (userTokenStore != null)
                {
                    SetUserOptionValue(userId, UserOPT.USER_OPT_LOGINSESSIONSIMAGEGUID, userTokenStore.Id.ToString() + "$" + userTokenStore.SessionId.ToString(), false);
                    return userTokenStore;
                }
                return null;
            }


            Session session = _dbContext.Sessions.FirstOrDefault(s => s.Id == sessionId && s.SessionId == userOptGuid);

            if (session != null)
                return session;
            else
                return null;
        }

        /// <summary>
        /// بازگشت مقدار تنظیمات مربوط به کاربر ذخیره شده در دیتابیس __Sys__
        /// </summary>
        /// <param name="userid">شناسه کاربر</param>
        /// <param name="tadbirWebOption"></param>
        ///<param name="samanString"></param>
        /// <returns></returns>
        public string GetUserOptionValue(int userId, UserOPT userOption, bool samanString)
        {
            UserOption _userOption = _userOptionContract.FindByUserId(userId);
            //UserOption _userOption = _dbContext.UserOptions.Find(userId);
            if (_userOption != null)
                return _userOption.OptionValue;

            return null;
        }


        /// <summary>
        /// ذخیره تنظیمات وب کاربر
        /// </summary>
        /// <param name="userid">شناسه کاربر</param>
        /// <param name="tadbirWebOption">شناسه تنظیمات</param>
        /// <param name="webOptionValue"></param>
        /// <param name="samanString">اگر لازم است تبدیل کدپیج روی مقدار انجام شد این پارامتر را true ارسال می‌کنیم</param>
        /// <returns></returns>
        public bool SetUserOptionValue(int userId, UserOPT userOPT, string optionValue, bool samanString)
        {

            try
            {
                UserOption userOption = _dbContext.UserOptions.FirstOrDefault(op => op.UserId == userId && op.Id == (int)userOPT);

                if (userOption != null)
                {
                    _dbContext.UserOptions.Remove(userOption);
                    _dbContext.SaveChanges();
                }

                userOption = new UserOption()
                {
                    Id = (int)userOPT,
                    UserId = userId,
                    OptionValue = optionValue
                };

                //_dbContext.UserOptions.Add(userOption);
                //_dbContext.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[__UserOption__] ON");
                //_dbContext.SaveChanges();
                //_dbContext.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[__UserOption__] OFF");

                try
                {
                    var conn = _dbContext.Database.GetDbConnection();
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = string.Format("SET IDENTITY_INSERT [dbo].[__UserOption__] ON " +
                        "INSERT INTO [dbo].[__UserOption__] ([Id], [UserId] ,[OptionValue]) VALUES ({0}, {1}, '{2}') " +
                        "SET IDENTITY_INSERT[dbo].[__UserOption__] OFF"
                        , userOption.Id, userOption.UserId, userOption.OptionValue);
                    command.ExecuteScalar();
                    conn.Close();

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                    //Debug.WriteLine("File", ex.Message.ToString());
                }

                

                //_dbContext.UserOptions.Add(userOption);
                //_dbContext.SaveChanges();

                //return true;
            }

            catch (Exception ex)
            {
                string message = ex.ToString();
                return false;
            }
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

        #region Internals
        private List<UserSession> ReadStoredUserSessions(byte[] buffer)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<UserSession>));
            using (MemoryStream reader = new MemoryStream(buffer))
            {
                using (XmlTextReader xmlTextReader = new XmlTextReader(reader))
                    return (List<UserSession>)xmlSerializer.Deserialize(xmlTextReader);
            }
        }

        private byte[] StoreUserSessions(List<UserSession> userSessions)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<UserSession>));
            using (MemoryStream writer = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(writer, Encoding.Unicode))
                    xmlSerializer.Serialize(xmlTextWriter, userSessions);
                return writer.GetBuffer();
            }
        }
        #endregion
    }
}