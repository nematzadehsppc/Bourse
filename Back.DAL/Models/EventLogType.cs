using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.DAL.Models
{
    public enum EventLogType
    {
        ET_Login= 201,
        ET_Logout= 202,
        ET_ChangePassword= 203,
        ET_ResetPassword_SendEmail= 204,
        ET_ResetPassword_SavePass= 205,
        ET_Failure_Login= 206,
        ET_WS_Back_Up_Successful= 207,
        ET_WS_Back_Up_Failure= 208,
        ET_INTERFACE_EXCEPTION = 209, //ذخیره اکسپشن های رابط کاربری
    }
}
