//اطلاعات دریافتی از سرور در صورت لاگین موفق
export class SuccessfulLoginResponseModel {
  constructor(
    public userId: number, //شناسه کاربر در تدبیر
    public sessionId: string, //شناسه جلسه کاربرر -- ممکن است کاربر از چندین کامپیوتر یا مرورگر لاگین شده باشد
    public loginTime: string, //زمان ورود
    public token: string, //توکن امنیتی که برای بازسازی جلسه کاربر مورد نیاز است
    public workspaceId: number, //آخرین شرکت که کاربر به آن وارد شده
    public fpId: number, //آخرین دوره مالی که کاربر به آن وارد شده
    public userRealName: string //نام واقعی کاربر
  ) {}
}
