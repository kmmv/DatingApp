Routing in Angular
-Ordering is important, first match serve

lab - 63 Setting up routing in Angular
***
*.on the client create member-list, lists and messages component using GenerateComponent
*.the root of app folder, create a file called routes.ts
*.create Routes array inside routes.tx
*.Each routes is a component inside {}
*. On the app.module.ts import { appRoutes} from './routes';
*.  On the app.module.ts add  RouterModule.forRoot(appRoutes) of the imports module

-We nee to add the routing functionality on our nav bar links
lab - 64 Setting up our links in the nav GenerateComponent
***

*. goto nav.component.html type a-router to change [routerLink]= and routerLinkActive=
*. on the app.component.html change <app-home> to <router-outlet> to make the links working

- We need to add the functionality when we login we will be directed to the memebers page
- and when we logout we need to go to the home page
lab - 65 Using routing in our components
***
*. Inject private router: Routes to the constructor
*. On the login, overload the function to the complete after error and add router.navigate
*. add navigation to the logout method

-now we need to add the functionality to restrict the user to go the page by typing page url
lab -66 Protecting our routes with a route guard
*. create _guards folder inside src/app
*. use command cd into the _guards folder and issue command - ng g guard auth --skipTests
*. choose canActivate option and press enter
*. The above action will create a auth.guard.ts inside our _guards folder
*. All we need is to check the user logged in so remove everything from canActivate
*. Add route guards to our routes.ts file by canActivate: [AuthGuard]

-now we want to use 1 route without duplicating the canActivate AuthGuard
lab-Protecting multiple routes with a single route guard using dummy routes
***
*.Create a dummy route
*. Mitigate the error - Outlet is not activated by change to  { path: '**', redirectTo: '', pathMatch: 'full'}
