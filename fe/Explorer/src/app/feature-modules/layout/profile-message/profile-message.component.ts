import { Component, Input } from '@angular/core';
import { ProfileMessage } from '../model/profileMessage.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';


@Component({
  selector: 'xp-profile-message',
  templateUrl: './profile-message.component.html',
  styleUrls: ['./profile-message.component.css']
})
export class ProfileMessageComponent {

  @Input() message: ProfileMessage;
  @Input() loggedInUser: User;

  constructor() {}

  ngOnInit(){
    console.log("zzzzzzzzzzz", this.message);
  }
}
