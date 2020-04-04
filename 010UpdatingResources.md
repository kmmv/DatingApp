010 - Updating Resources
************************
Create a component to edit profile
Candeactivate route guard - protect changes
@ViewChild decorator
Persisting changes on the API

lab 97 Creating a Member Edit component
***
*. creating member-edit inside members folder
*. goto app.module.ts and add MemberEditComponent
*. goto routes.ts and and add path: 'member/edit ' route
*. goto nav.component.ts and add  <a class="dropdown-item" [routerLink]="['member/edit']" ><i class="fa fa-user"></i>Edit Profile</a>
*. Need to bring currently login user details to the member-edit MemberEditComponent
        - for this go to resolvers and create a member-edit.resolver.ts
*. on the member-edit.resolver.ts add authService for accessing the token
*. complete the resolver by adding this.authService.decodedToken.name.id
*. goto app.module.ts and add MemberEditResolver
*. add the resolver on the Routes.ts
*. goto member-edit.component and add
       - this.route.data.subscribe( data => { this.user = data['user'];
*. goto member-edit component.html and add {{user.knownAs}} as check to see the username is appearing

lab 98 & 99- Designing the template for the member profile edit page - part 1
***
*. copy the member-detail.component.ts content to member-edit.component.html
*.  edit the fields to suit the edit Profile
*.  For saving changes information box
    - <form #editForm="ngForm" id="editForm"  >
    - <div  *ngIf="editForm.dirty" class="alert alert-info">
*. For Save changes button - <button [disabled]="!editForm.dirty" form="editForm" class="btn btn-success btn-block"
*. Add updateUser method to show a alertify message
*. <form #editForm="ngForm" id="editForm" (ngSubmit)="updateUser()"  >
        -without id-"editForm" alertify will not work because the button is not part of the form
        - without form="editForm" on the button the alert will not work because of explanation above
*. The alert message is still showing on the top after clicking save changes - to solve this
      -add  @ViewChild('editForm') editForm: NgForm; on the member-edit.component.ts
      - add   this.editForm.reset(this.user); to the updateUser on the member-edit.component.ts

-Now when we click on some other link, the changes are on the edit are not persisted.


lab 100 Adding a Candeactivate route guard
***
*. create  prevent-unsaved-changes.ts on the _guards
*. goto app.module.ts and add preventUnsavedChanges to the providers array
*. goto routes.ts add the preventUnsavedChanges to the member/edit component
    -The above steps will add a message confirming whether the user wants to lose changes by navigating
- but if the browser tab is closed, angular hasn't go any control of this
- so we can do some workaround like the following
*. goto member-edit.component.ts
      -- add @HostListener('window:beforeunload', ['$event']) and unLoadNotification($event: any) {

lab 101- Persisting the Member updateUser
***
*. Goto the API project
*. Add new UserforUpdateDtop inside Dtos for the change/update fields for member-edit
*. Create map on the AutoMapperProfile,cs    CreateMap<UserForUpdateDto, User>();
*. Create and complete put method - UpdateUser on the UserController
*. Test the put method on Postman first - we always check with Postman to see the API side is okay
 - If the postman is showing 415 unsupported media type  - add the content-type to applciation/json
 - https://stackoverflow.com/questions/42670413/unsupported-media-type-in-postman


- use the browser to update the member profile
lab - 102 Finishing off the Member edit component
***
*. create 'updateUser' on the user.service
*. bring userService and authService on to the member-edit.component
*. invoke this.userService.updateUser from updateUser
