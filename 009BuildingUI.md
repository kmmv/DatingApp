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

lab 89- Using Auth0 jwtModule to send up jwt token automatically.
***