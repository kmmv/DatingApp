Client side Login and register
**************************

Bootstrap: https://getbootstrap.com/

Lab: 40:Navigation and Login form
****
*:find the navbar on the bootstrap
*:Generate nav component inside the app
*:Paste the navabar html code from bootstrap to the nav.component.html
*:Copy the selector <app.nav> and paste on the app.component.html
*:Format the html (the code which is copied and pasted)

-Pass the data on the form to the object 
- model: any = {} {} is the object passed to model. model(variable name) ie model of type any
Lab: 41: Introduction to Angular template forms
****
*: On the nav.component.ts add model:any={} to store username and password
*:Add method to login
*:import {FormsModule } from '@angular/forms' to the appmodule.ts;
*:add template form #loginForm="ngForm" on nav.component.html
*:required validation and ngModel for controls
*: button [!disabled] for disable button until the controls are validated


Angular services
****************

 this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response;
    }
The code above is difficult to manage and needs to be duplicated every time a component needs a value.
To manage this problem efficiently angular provide services

Lab - 42.Introduction to Angular services
***
-Create a _service folder under the app folder
-Right click and Generate auth service
    --services are not injectable by default
-Link the service in the providers array of the app.module.ts
-Specify the baseUrl on the auth.service.ts 
-Add Http client to the contructor to consume the httpclient
-Implement login method using this.http.post
    --http.post will return an observable as object
-import {map} from 'rxjs/operators' on the auth.service.ts
-The http.post will return the token, we need to store the token (http.post returns the body as object)
-using the pipe and map opertator store the token on the localstorage

Use the service on the nav bar component

Lab - Use the service on the nav bar component
***
1.Inject auth service into the contructor of the nav.component.ts
2.on the login method use the authService.login and subscribe and error method
3.Goto the browser and test login, on the network tab status check the return status (200 OK), find the payoad and token.
4.On the Storage tab of the browser a localstorage is created


Lab 44-Using *ngif to conditionally
***
*.Copy dropdown from bootstrap
*.goto navcomponent.ts and paste it
*.Format the dropdown to show Welcome User For example: "fa fa-user"
*.Implement LoggedIn and Logout on the nav.component.ts
*.implement *ngIf on the dropdown and loginform on nav.component.html
*.implment a Logout menu for now

Lab:45 User can Register to our site
***
*.Generate components home and register
*.Start with home component by pasting the snippet files on home and register component
*.Add  app-home component to the app.component.html
*.Add  app-register to the home component
*.Implement the register toggle when the register button is clicked

-we need to implement the communication between the components - this can be 2 ways which are 'Parent to Child' and viceversa  
- Parent to Child 
Lab: 46 Parent to Child Component communcation using Input properties
***
*. home - parent component  <app-register [valuesFromHome]="values"> </app-register>
*. register - child component @Input() valuesFromHome: any;
*. and on the child component html -   <option *ngFor="let value of valuesFromHome" [value]="value">{{value.name}}</option>


-Child to Parent
-When the register button is clicked  the register form is show but when the cancel button is clicked 
-it should toggleback to home component - because the cancel button is on the register component

Lab: 47 Component Communication Child to Parent using Output properties
***
*. create @Output() cancelRegister = new EventEmitter() on the child (register) component;
*. Implement this.cancelRegister.emit(false); inside a new method named cancel on the child componnet
*. Add a cancelregister event handler on the parent(home) component - <app-register  (cancelRegister)="cancelRegisterMode($event)" 
*. Implement cancelRegisterMode on the parent component

Lab: 48 Adding the register method
***
*.create register method on the auth service
*.import the auth service on the register component
*.On the register component to call this.authService(this.model).subscribe Note:the service method is observable hence subscribe


