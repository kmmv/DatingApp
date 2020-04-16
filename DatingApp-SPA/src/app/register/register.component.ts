import { Component, OnInit, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
@Output() cancelRegister = new EventEmitter();
registerForm: FormGroup;


// km :model(variable) name of type any with object passed to it
  model: any = {};

  // km : to consume the service
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
     this.registerForm = new FormGroup({
        username: new FormControl('Hello', Validators.required),
        password : new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
        confirmPassword : new FormControl('', Validators.required)
     }, this.passwordMatchValidator);
  }

  // Custome validator
  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {

    // this.authService.register(this.model).subscribe(() => {
    //   this.alertify.success('registration successful');
    // }, error => {
    //   this.alertify.error(error);
    // });

    console.log(this.registerForm.value);
  }

  cancel() {
    // km emit can be anything, an object, boolean etc
    this.cancelRegister.emit(false);
  }

}
