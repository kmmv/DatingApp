Displaying dates in the SPA using pipes
For example to show dates like (updated 2 mins ago)
Action Filters can have higher action filter without repeating action code

lab 136 - Using a TimeAgo pipe for dates in Angular
- Member Since formatting using angular pipe
*. on the SPA goto member-detail.component.ts and add action pipe  {user.created | date: 'mediumDate'}
*. goto member-edit.component and repeat the step above
- for the last active- need time ago pipe here but there's no default pipe we have to import a library
- google angular npm install ngx-timeago --save
*.goto app.module.ts and import { TimeagoModule } from 'ngx-timeago'; TimeagoModule.forRoot()
*.on the member-edit.component.html add <div>{‌{user.lastActive | timeago}}</div>

- now we need to update the last active whenever the user logins
lab - 137 Using Action Filters
