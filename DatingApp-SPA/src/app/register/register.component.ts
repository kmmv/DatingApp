import { Component, OnInit, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
@Input() valuesFromHome: any;
@Output() cancelRegister = new EventEmitter();

// km :model(variable) name of type any with object passed to it
  model: any = {};

  constructor() { }

  ngOnInit() {
  }

  register() {
      console.log(this.model);
  }

  cancel() {
    // km emit can be anything, an object, boolean etc
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }

}
