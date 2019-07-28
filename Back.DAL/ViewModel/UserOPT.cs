using System;
using System.Collections.Generic;
using System.Text;

namespace Back.DAL.ViewModel
{
    public enum UserOPT
    {
        USER_OPT_PASSWORD_RECOVERY_ENABLE = 1, //امکان ریست شدن پسوورد
        USER_OPT_SMTP_USERNAME = 2, //نام کاربری مربوط به SMTP
        USER_OPT_SMTP_PASSWORD = 3, //رمز عبور مربوط به SMTP
        USER_OPT_PASSWORDRECOVERY_URL_GUID = 4, //پارامتر یونیک مربوط به آدرس صفحه تغییر پسوورد
        USER_OPT_PASSWORD_CHANGE_ENABLE = 5, //امکان تغییر پسوورد
        USER_OPT_BACKGROUND_COLOR = 6, //رنگ پس زمینه
        USER_OPT_LOGIN_CAPTCHA_ENABLE = 7, //نمایش کد امنیتی در هنگام ورود به سیستم
        USER_OPT_PROJECT_PATH = 8, //مسیر پروژه
        USER_OPT_SENDING_LOGIN_EMAIL_ENABLE = 9, //ارسال ایمیل به کاربر برای ورود به سیستم
        USER_OPT_LAST_CHART_TYPE = 10,//نوع آخرین چارت انتخابی کاربر  
        USER_OPT_SESSION_TIMEOUT = 11,// طول عمر سشن
        USER_OPT_LOGIN_TEMPLATE = 12, //نام فایل قالب صفحه ورود به سیستم
        USER_OPT_SELECTED_FONT_NAME = 13, //فونت انتخابی کاربر
        USER_OPT_DB_BACKUP_PATH = 14, //مسیر ذخیره پشتیبان شرکت ها
        USER_OPT_ATTACHMENT_FILES_SUFFIXES = 15, //پسوندهای قابل قبول برای فایل های پیوست 
        USER_OPT_LOGINSESSIONSIMAGEGUID = 16, //شناسه رکورد متناظر در جدول __Image__ در دیتابیس __ImgData__ که اطلاعات لاگین کاربر در آن نگهداری می‌شود
    }
}
