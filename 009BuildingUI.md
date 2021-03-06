009 Building a UI

lab - 84 Creating another Angular Service
***
*.  create apiUrl: 'http://localhost:5000/api/' on the environment.ts
*.  create new service named user.service, inject httpService and complete implementation
*. Update member-list.component.ts with loadUsers()
*. update member-list.component.html
*. The users are listed on the matches page

lab - 86 Creating Member Cards to display on our Members list page
***
*.  create member-card component inside members/member-list
*. create an Input user to bring the user from parent component which is member-list
*. complete member-card component using bootstrap card
*. pass the user from the memberlist component inside <app member-card> component
      -from memeber-list component - Note the [user] is inside the <> braces
      -   <app-member-card [user]="user"></app-member-card>

lab 87 and 88 - styling member-card component

Note: issue when the members are not displayed while login because the auth token is not send

https://github.com/auth0/angular2-jwt: This library provides an HttpInterceptor which automatically attaches a JSON Web Token to HttpClient requests.


lab 89- Using Auth0 jwtModule to send up jwt token automatically.
***
*. on the app.module.ts copy the code from github for export function - export function tokenGetter() {
*. on the app.module.ts, add JwtModule to the imports
*. remove httpOptions from user.service.ts because we are going to use JwtModule from now on.
*. try login and see check all the tokens - on users - token will be displayed, on login token will not


lab 90 - Createing the member detailed view component class
***
*. create member-detail component inside member-detail
*. add memeberdetailcomponent to the appmodule.ts
*. import userService.  AlertifyService and ActivateRoute to the MemberDetailComponent
*. and complete loadUser method and call it on ngInit
*. add 'members/:id' memberDetailComponent to the routes.ts
*. add [routerLink]="['/members/', user.id] to the member-card.component.html

lab 91 Designing the Member detailed view template - left hand side
***
*. add html to the member-detail.component
*. add css

lab 92: Adding a tabbed panel for the right hand side of the Member detailed page
**
*. add this package -  ng add ngx -bootstrap
*. add TabsModule.forRoot() to the imports module
*. add <tabset> and other <tab> components to the member-detail.component.html
*. complete the tabbed panel for RHS on the member-detail.component.html
*. add css snippet from the resources to the styles.css file
 -In this case we need to add it as a global style as we cannot access the specific classes within the --component itself as they are generated by the tabset
  -and it will not work in this instance by adding them directly to the component css

lab 93: Using Route Resolvers to retrieve data
***
-We have to eliminate the safe navigation operators {{user?.lookingFor}} when data is not available
- before the route is not activated. This is resolved using route resolvers A resolver contains code -  - which is executed when a link has been clicked and before a component is loaded.
- return of - of is the type of observable
*. create a folder named _resolvers inside the app folder.
*. create a file named member-detail.resolver.ts
*. bringin Injectable and complete member-detail.resolver.ts
*. Add MemberDetailResolver inside the providers array of app.module.ts
*. add resolver to the routes.ts  -  { path: 'members/:id', component: MemberDetailComponent,
                                  - resolve: {users: MemberDetailResolver}},
*. goto member-detail.component.ts and use the resolver instead inside ngOninit
                                          - this.route.data.subscribe(data => {
     // .user is coming from the route.ts
   });
*.remove all the safe navigation operators from the member-detail.component.html
*. we have to add resolver to the member-list-page
*. copy the member-detail.resolver and create member-list.resolver
*. goto the routes.ts and add the resolve to the MemberListComponent
*. goto app.module.ts and add MemberListResolver
*. goto member-list.component, bringin ActivateRoute on the constructor
*

lab 94: Adding a photo gallery to our application
***
*. https://www.npmjs.com/package/ngx-gallery to find the documentaion
*. install using the command : npm install @kolkov/ngx-gallery
*. goto app.module.ts and import { NgxGalleryModule } from '@kolkov/ngx-gallery';
*. on the app.module.ts add NgxGalleryModule  to the imports
    - Note the order  NgxGalleryModule created an error 'Renderer2', so I re-ordered
*. complete the member-detail.component using galleryOptions and galleryImages
*. on the member-detail.component add <ngx-gallery [options]="galleryOptions" [images]="galleryImages" class="ngx-gallery"></ngx-gallery>
* add styles in styles.css
            .ngx-gallery {
                display: inline-block;
                margin-bottom: 20px;
              }

  Note: Add   "no-string-literal" : false to tslint.json to disable data['user'] error
