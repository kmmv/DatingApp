006 3rd Party components
************************
For notifications : Go to site https://alertifyjs.com - this contains some simple css and
javascript for event notifications

Todo: Install alertify and create a angular service wrapper for alertify and inject
the angular service wrapper to our application

lab - 57 Wrapping 3rd party libraries
***

*.go to client app terminal -  npm install alertifyjs
*.goto styles.css and import alertify.min.css and bootstreap.min.css
*.goto services and create new services named alertify (2 files are created)
*.while importing there will be a warning
      - resolve this by creating typings.d.ts inside src folder
      - add using declare module 'alertifyjs to the typings.d.ts
      - goto tsconfig.json and add typeRoots to point src/typings.d.ts
      - warning will be goine
*. implement AlertifyService -  implement methods confirm, success, warning, message

- use the alertify services
*. goto nav.component.ts and register.component.ts and change all the console.log to appropriate alertify messages

At the moment we are storing the token in localstorage and nothing is being done to check
we are using genuine token. This is not a security risk because the server will validate against it
Even though this is not security risk - we can use better way of managing token.



lab - 58 Using the Angular JWT
***
*. Find the auth0 angular2 https://github.com/auth0/angular2-jwt (check documentation and installation)
*. on the client npm install @auth0/angular-jwt will install @auth0/angular-jwt@4.0.0 or higher
*. we have to create a loggedin method on the auth.service and service class is where we want to plug service methods
*. change the loggedin method of navcomponent to use the loggedIn service from auth.service

-Now the welcome user needs to be changed to actual username. Best way is to find the username
-is from the token rather than

Lab - 58 Using the Angular JWT library to decode tokens
***
*.on the client -add a variable decodedToken : any on the auth.service
*.in the login method. this.jwtHelper.decodedToken(user.token)
*.on the console window of the browser the unique_name is revealed
*.on the navcomponent.html change to Welcome {{authService.decodedToken.unique_name}}
*.In order to elminate the error on browser refresh
  -change private authService to public on the contructor of nav.component.ts
  -Implement constructor and OnInit on Appcomponent

-Now we have to style the Welcome Username part on the right top
Lab -59 Adding Ngx Bootstrap to power out Bootstrap components
*. find ngx bootstrap on google by valor software to complement our bootstrap
*. install npm install ngx-bootstrap --save
*. goto navcomponent.html
*. add dropdown on <div *ngIf="loggedIn()" class="dropdown" dropdown>
*. add dropdowntoggle on <a class="dropdown-toggle text-light" dropdownToggle >
*. add dropdown-men on  <div  class="dropdown-menu" *dropdownMenu >
*. css changes add class to nav.component.css

Lab - 60 Brining some color to our app with a theme from Bootswatch
*.find bootswatch on google
*.npm install bootswatch
*.add to styles : @import '../node_modules/bootswatch/dist/united/bootstrap.min.css';
*.change navbar on nav.component.html - <nav class="navbar navbar-expand-md navbar-dark bg-primary">
