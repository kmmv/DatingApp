import { Component, OnInit, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
@Output() cancelRegister = new EventEmitter();
registerForm: FormGroup;
// we are adding datepickerConfig to change the theme, some config are mandatory but we dont want to config every mandatory field
// so we are getting the variable as partial so that all the config will be mandatory
bsConfig: Partial<BsDatepickerConfig>;


// km :model(variable) name of type any with object passed to it
  model: any = {};

  // km : to consume the service
  constructor(private authService: AuthService,
              private alertify: AlertifyService,
              private fb: FormBuilder) { }

  ngOnInit() {
    /* this.registerForm = new FormGroup({
        username: new FormControl('Hello', Validators.required),
        password : new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
        confirmPassword : new FormControl('', Validators.required)
     }, this.passwordMatchValidator);*/

    // for the calendar control themeing
     this.bsConfig = {
      containerClass: 'theme-red'
    };
     this.createRegisterForm();
  }


  createRegisterForm() {
    // this.fb.group is equivalent to new FormGroup created on the ngInit
    this.registerForm = this.fb.group(
      {
        gender: ['male'],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(8)
          ]
        ],
        confirmPassword: ['', Validators.required]
      },
      { validator: this.passwordMatchValidator }
    );
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
