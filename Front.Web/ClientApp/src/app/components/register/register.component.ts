import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};

  constructor() { }
  //constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  //دکمه ورود
  onSubmit() {
    this.login();
  }

  public login() {
    alert(this.model.username);
  }

}
