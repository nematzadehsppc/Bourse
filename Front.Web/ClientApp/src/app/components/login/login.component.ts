import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {};

  constructor() { }
  //constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  public login() {
    this.model.userValidation = 'Angular';
    alert(this.model.username);
  }

}
