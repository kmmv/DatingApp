import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})

export class MemberDetailComponent implements OnInit {
  user: any;

 constructor(private userService: UserService,
             private alertify: AlertifyService,
             private route: ActivatedRoute) { }

  ngOnInit() {
    // this.loadUser()
    // now the data is coming from the member-detail.resolver
    this.route.data.subscribe(data => {
      // .user is coming from the route.ts
      this.user = data.user;
    });
  }

  /*
  //this method is not needed as we are using the resolver.
  loadUser() {
    this.userService.getUser(+this.route.snapshot.params.id).subscribe((user: User) => {
      this.user = user;
    }, error => {
      this.alertify.error(error);
    });
  }*/

}
