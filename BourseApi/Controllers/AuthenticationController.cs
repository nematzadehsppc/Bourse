using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BourseApi.Contract;
using Back.DAL.Models;
using Microsoft.AspNetCore.Cors;
using System.Net;
using System;
using System.Diagnostics;
using BourseService;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace BourseApi.Controllers
{
    /// <summary>
    /// ای پی آی تأیید هویت کاربر
    /// </summary>
    [Produces("application/json")]
    [Route("api/tauth")]
    [EnableCors("OmidanCorsPolicy")]
    public class AuthenticationController : Controller
    {

        #region Services and Dependency Injection process

        private IAuthenticationContract AuthenticationContract { get; set; }
        private ITokenServiceContract _tokenService; //تولید توکنها
        private IHttpContextAccessor _accessor; //جهت به دست آوردن ip کلاینت
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="sysDataAccessLayer">instance of ISysDataAccessLayer</param>
        /// <param name="tokenService">instance of ISPPCTokenService</param>
        /// <param name="accessor">instance of IHttpContextAccessor</param>
        public AuthenticationController(IAuthenticationContract authenticationContract, ITokenServiceContract tokenService, IHttpContextAccessor accessor)
        {
            AuthenticationContract = authenticationContract;
            _tokenService = tokenService;
            _accessor = accessor;
        }
        #endregion

        #region Login
        /// <summary>        
        /// Verifies provided login information and returns the result
        /// ------------------------------------------------------------------------------------------
        /// input: Login information (ClientIPAddress is filled by server, do not bother filling it)
        /// ------------------------------------------------------------------------------------------
        /// returns:
        /// 
        /// OnSuccess: <seealso cref="SuccessfulLoginResponseModel"/>
        /// an structure consisting of:
        /// 1. UserId (int)
        /// 2. SessionId (Guid)
        /// 3. LoginTime (DateTime)
        /// 4. Token (string)
        /// 
        /// ------------------------------------------------------------------------------------------
        /// OnFailure
        /// an structure consisting of <seealso cref="FailedLoginResponseModel"/>
        /// 1. code (int) <seealso cref="TadbirAuthenticationResultCode"/>
        /// 2. tadbirAuthenticationResult (string)
        /// 3. additionalInformation (string) - unhandled onformation exception
        /// 
        /// </summary>
        /// <param name="loginModel">Login information</param>
        /// <returns>
        /// OnSuccess: <seealso cref="SuccessfulLoginResponseModel"/>
        /// an structure consisting of:
        /// 1. UserId (int)
        /// 2. SessionId (Guid)
        /// 3. LoginTime (DateTime)
        /// 4. Token (string)
        /// 
        /// ------------------------------------------------------------------------------------------
        /// OnFailure
        /// an structure consisting of: <seealso cref="FailedLoginResponseModel"/>
        /// 1. code (int) <seealso cref="TadbirAuthenticationResultCode"/>
        /// 2. tadbirAuthenticationResult (string)
        /// 3. additionalInformation (string) - unhandled onformation exception
        /// </returns>   
        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SuccessfulLoginResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(FailedLoginResponseModel))]
        public IActionResult Login(
            [FromBody]LoginViewModel loginModel
            )
        {
            try
            {
                string clientIPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //در گام اول با بررسی اطلاعات کاربر در دیتابیس
                //__Sys__
                //از درستی اطلاعات وارد شده شامل نام کاربری و کلمه عبور و همینطور دسترسی داشتن کاربر به بخش مورد نیاز
                //مطمئن می‌شویم
                Tuple<AuthenticationResult, User> authResult = AuthenticationContract.AuthenticateUser(loginModel, clientIPAddress);

                if (authResult.Item1.Code != AuthenticationResultCode.AuthenticationSuccess)
                {
                    return BadRequest(
                        new FailedLoginResponseModel()
                        {
                            code = authResult.Item1.Code,
                            authenticationResult = AuthenticationContract.GetAuthenticationResultMessage(authResult.Item1.Code, loginModel.Language),
                            additionalInformation = authResult.Item1.AdditionalErrorMessage
                        }
                        );
                }



                User user = authResult.Item2;

                Debug.Assert(user != null);

                Guid sessionId = AuthenticationContract.GetNextSessionId(user.Id);
                if (sessionId == Guid.Empty)
                {
                    return BadRequest("error finding a new session id");
                }

                var tokenInfo = _tokenService.GenerateToken(user, sessionId);

                UserSession userSession = new UserSession()
                {
                    SessionId = sessionId,
                    UserId = user.Id,
                    AppName = loginModel.ClientAppName,
                    IpAddress = clientIPAddress,
                    LoginTime = DateTime.Now,
                    LastRenewal = DateTime.Now,
                    ValidUntil = DateTime.Now + TimeSpan.FromSeconds(SecurityParameters.DefaultTokenExpirationInSeconds),
                    UserMCheckSum = user.CheckSum,
                    Token = tokenInfo,
                    ServiceAccessType = loginModel.ServiceAccessType,
                    Language = loginModel.Language,
                };

                userSession = AuthenticationContract.AddUserSession(userSession, out string exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                {
                    return BadRequest(exceptionStr);
                }


                return new ObjectResult(
                            new SuccessfulLoginResponseModel()
                            {
                                UserId = userSession.UserId,
                                SessionId = userSession.SessionId,
                                LoginTime = userSession.LoginTime,
                                Token = userSession.Token
                            }
                            );
            }
            catch (Exception exp)
            {
                return BadRequest(
                    new FailedLoginResponseModel()
                    {
                        code = AuthenticationResultCode.UnknownServerError,
                        authenticationResult = AuthenticationContract.GetAuthenticationResultMessage(AuthenticationResultCode.UnknownServerError, loginModel.Language),
                        additionalInformation = exp.ToString()
                    }
                    );
            }

        }
        #endregion


        #region Renew User Session
        /// <summary>
        /// Renew expired usersession (if any key authentication information for user such as password is changed this method fails and does not renew session)
        /// </summary>
        /// <param name="tadbirUserSessionModel">userid and sessionid</param>
        /// <returns>
        /// if succeeds: <seealso cref="SuccessfulLoginResponseModel"/>
        /// if failse: 400 (BadRequest)
        /// </returns>
        [Produces("application/json")]
        [Route("renew")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SuccessfulLoginResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult RenewExpiredUserSession([FromBody]UserSessionModel tadbirUserSessionModel)
        {
            try
            {
                if (tadbirUserSessionModel == null)
                    return BadRequest("tadbirUserSessionModel == null");
                //بر اساس اطلاعات ورودی سشن کاربر را پیدا می‌کنیم
                UserSession userSession = AuthenticationContract.GetUserSession(tadbirUserSessionModel.UserId, tadbirUserSessionModel.SessionId, out string exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                    return BadRequest(exceptionStr);
                if (userSession == null)
                    return BadRequest("userSession == null");
                //سشن کاربر را حذف می‌کنیم
                AuthenticationContract.RemoveUserSession(userSession, out exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                    return BadRequest(exceptionStr);
                //اگر اطلاعات اساسی کاربر مثلا پسوردش عوض شده باشد دیگر سشنشن را تمدید نمی‌کنیم
                User user = AuthenticationContract.GetUser(tadbirUserSessionModel.UserId);
                if (user.CheckSum != userSession.UserMCheckSum)
                    return BadRequest("اطلاعات اساسی کاربر تغییر کرده است");
                //توکن جدید را تولید و ذخیره می‌کنیم
                string newtoken = _tokenService.RegenerateToken(userSession.Token, out exceptionStr);
                if (newtoken == null)
                    return BadRequest("تولید مجدد رمز کاربر با خطا مواجه شد. " + exceptionStr);
                userSession.IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(); //ممکن است نشانی ip کامپیوتر تغییر کرده باشد
                userSession.Token = newtoken;
                userSession.LastRenewal = DateTime.Now;
                userSession.ValidUntil = DateTime.Now + TimeSpan.FromSeconds(SecurityParameters.DefaultTokenExpirationInSeconds);
                userSession = AuthenticationContract.AddUserSession(userSession, out exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                    return BadRequest(exceptionStr);
                if (userSession == null)
                {
                    return BadRequest("userSession = AuthenticationContract.AddUserSession(userSession, out exceptionStr) failed");
                }

                AuthenticationContract.LogUserSessionRenewal(user, userSession.IpAddress, out exceptionStr);

                return new ObjectResult(
                            new SuccessfulLoginResponseModel()
                            {
                                UserId = userSession.UserId,
                                SessionId = userSession.SessionId,
                                LoginTime = userSession.LoginTime,
                                Token = userSession.Token
                            }
                    );
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        #endregion

        #region Delete User Session
        /// <summary>
        /// Delete user session (logout)
        /// </summary>
        /// <param name="tadbirUserSessionModel">userid and sessionid</param>
        /// <returns>
        /// if succeeds: <seealso cref="SuccessfulLoginResponseModel"/>
        /// if failse: 400 (BadRequest)
        /// </returns>
        [Produces("application/json")]
        [Route("delsession")]
        [HttpDelete]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public IActionResult RemoveUserSession([FromQuery]UserSessionModel tadbirUserSessionModel)
        {
            try
            {
                //بر اساس اطلاعات ورودی سشن کاربر را پیدا می‌کنیم
                UserSession userSession = AuthenticationContract.GetUserSession(tadbirUserSessionModel.UserId, tadbirUserSessionModel.SessionId, out string exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                    return BadRequest(exceptionStr);
                if (userSession == null)
                    return BadRequest("userSession == null");
                //سشن کاربر را حذف می‌کنیم
                bool res = AuthenticationContract.RemoveUserSession(userSession, out exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                    return BadRequest(exceptionStr);
                if (res)
                {
                    if (AuthenticationContract.LogEvent
                    (
                    //خروج کاربر را لاگ می کنیم
                    new EventLog()
                    {
                        UserId = userSession.UserId,
                        EventId = EventLogType.ET_Login,
                        EDesc = $"خروج از سیستم",
                        EventDate = DateTime.Now,
                        EventTime = DateTime.Now

                    }
                    ))
                    {
                        return new ObjectResult(true);
                    }
                }

                //خطایی رخ داده
                return BadRequest("RemoveUserSession(userSession) returned false");
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }

        }
        #endregion

        #region User Sessions
        /// <summary>
        /// لیست جلسات ورود کاربر جاری را برمی‌گرداند
        /// </summary>
        /// <param name="currenSessionId">شناسه جلسه جاری</param>
        /// <returns>لیست جلسات ورود کاربر جاری</returns>
        [HttpGet]
        [Authorize]
        [Route("sessions")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string[][]))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetUserSessions(Guid currenSessionId)
        {
            try
            {
                int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "TadbirUserId").Value);
                string name = AuthenticationContract.GetUser(userId).Name;



                List<string[]> userSessionItems = new List<string[]>();
                UserSession[] sessions = AuthenticationContract.GetUserSessions(userId, out string exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                {
                    return BadRequest(exceptionStr);
                }
                foreach (UserSession session in sessions)
                {
                    userSessionItems.Add
                        (
                        new string[]
                        {
                            name,
                            session.IpAddress,
                            _GetSessionTime(session.LoginTime),
                            _GetSessionTime(session.LastRenewal),
                            session.AppName,
                            session.SessionId.ToString(),
                            (session.SessionId == currenSessionId).ToString()
                        }
                        );

                }

                return new ObjectResult(userSessionItems.ToArray());
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }

        }

        private string _GetSessionTime(DateTime dateTime)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                return
                                String.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}",
                                pc.GetYear(dateTime),
                                pc.GetMonth(dateTime),
                                pc.GetDayOfMonth(dateTime),
                                dateTime.Hour,
                                dateTime.Minute
                                );
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Calculate Expire Time For Session
        /// <summary>
        /// زمان انقضای جسله کاربر بر حسب ثانیه برمی‌گرداند
        /// </summary>
        /// <param name="sessionId">شناسه جلسه</param>
        /// <returns>زمان انقضای جسله کاربر بر حسب ثانیه</returns>
        [HttpGet]
        [Authorize]
        [Route("expirationinseconds")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(double))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetSessionExpirationInSeconds(Guid sessionId)
        {
            try
            {
                int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "TadbirUserId").Value);
                UserSession session = AuthenticationContract.GetUserSession(userId, sessionId, out string exceptionStr);
                if (!string.IsNullOrEmpty(exceptionStr))
                {
                    return BadRequest(exceptionStr);
                }
                if (session == null)
                {
                    return new ObjectResult((double)0);
                }
                return new ObjectResult
                    (
                        (session.ValidUntil - DateTime.Now).TotalSeconds
                    );
            }
            catch (Exception exp)
            {
                return BadRequest(exp.ToString());
            }
        }
        #endregion
    }
}