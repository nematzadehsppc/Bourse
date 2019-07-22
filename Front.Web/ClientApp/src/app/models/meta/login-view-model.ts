export class LoginViewModel {
  constructor(
    public username: string,
    public password: string,
    public serviceAccessType: number = 1,
    public clientAppName: string = 'Tadbir Web',
    public language: string = 'fa-IR',
  ){}
}
