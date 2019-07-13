import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  public headers: HttpHeaders | undefined | null;
  public httpOptions: any;

  //constructor() { }
  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    this.httpOptions = { headers: this.headers, responseType: "json" };
  }

  ngOnInit() {
  }

  //دکمه ورود
  onSubmit() {
    this.register();
  }

  public register() {
    this.insertData().subscribe(res => {
      debugger;
    }, error => {
      debugger;
      })


    //alert(this.model.username);
  }

  insertData(): Observable<any> {
    var body = JSON.stringify({
      name: this.model.name,
      familyName: this.model.family,
      username: this.model.username,
      password: this.model.password,
      email: this.model.email,
      birthdate: this.model.birthdate
    });

    return this.http.post('http://localhost:10818/user/addUser', body, this.httpOptions)
      .pipe(
        map(res => res)
      )
  }
}
