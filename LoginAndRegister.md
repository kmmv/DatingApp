Client side Login and register
**************************

Bootstrap: https://getbootstrap.com/

Lab:
****
Step1:find the navbar on the bootstrap
Step2:Generate nav component inside the app
Step3:Paste the navabar html code from bootstrap to the nav.component.html
Step4:Copy the selector <app.nav> and paste on the app.component.html
Step5:Format the html (the code which is copied and pasted)
Step6: On the nav.component.ts add model:any={} to store username and password
Step7:Add method to login
Step8:import {FormsModule } from '@angular/forms' to the appmodule.ts;
Step8:add template form #loginForm="ngForm" on nav.component.html
Step9:required validation and ngModel for controls
Step10: button [!disabled] for disable button until the controls are validated


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


