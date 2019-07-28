import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { LoginViewModel } from 'src/app/models/meta/login-view-model';
import { SuccessfulLoginResponseModel } from 'src/app/models/meta/SuccessfulLoginResponseModel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  //model: any = {};
  public headers: HttpHeaders | undefined | null;
  public httpOptions: any;

  //constructor() { }
  //constructor(private http: HttpClient) { }

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    this.httpOptions = { headers: this.headers, responseType: "json" };
  }

  model = new LoginViewModel('admin', '');

  ngOnInit() {
  }
  
  //دکمه ورود
  onSubmit() {
    this.login();
  }

  public login() {

    //فراخوانی سرویس لاگین
    this.loginUser()
      .subscribe(
        (data: SuccessfulLoginResponseModel) => {
          //ورود موفق
          JSON.stringify(data);
          debugger;
        },
      error => {
        if (error.status == 401) {
          //جلسه کاربر را تمدید کنیم
        }
        else {
          //احتمالا جلسه کاربر از روی سرور حذف شده
          localStorage.removeItem('loggonuser');
        }
      });

    //this.loginUser().subscribe(res => {
    //  debugger;
    //}, error => {
    //  debugger;
    //})

    
  }

  loginUser(): Observable<any> {
    var body = JSON.stringify({
      username: this.model.username,
      password: this.model.password,
      serviceAccessType: this.model.serviceAccessType,
      clientAppName: this.model.clientAppName,
      language: this.model.language
    });

    return this.http.post('http://localhost:10818/tauth/login', body, this.httpOptions)
      .pipe(
        map(res => res)
      )
  }

}
