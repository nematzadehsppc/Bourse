import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { LoginViewModel } from 'src/app/models/meta/login-view-model';

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
    this.loginUser().subscribe(res => {
      debugger;
    }, error => {
      debugger;
    })

    
  }

  loginUser(): Observable<any> {
    var body = JSON.stringify({
      mahinName: this.model.username
    });

    return this.http.get('http://localhost:10818/tauth/userlogin/5', this.httpOptions)
      .pipe(
        map(res => res)
      )
  }

}
