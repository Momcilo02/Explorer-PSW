import { Component } from '@angular/core';
import { ProfileMessage } from '../model/profileMessage.model';
import { ActivatedRoute } from '@angular/router';
import { LayoutService } from '../layout.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';

@Component({
  selector: 'xp-profile-chat',
  templateUrl: './profile-chat.component.html',
  styleUrls: ['./profile-chat.component.css']
})
export class ProfileChatComponent {
  messages: ProfileMessage[] = []
  secondUserId!: number;
  secondUsername: string;
  loggedInUser: User;

  constructor(
    private route: ActivatedRoute,
    private layoutService: LayoutService,
    private authService: AuthService ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.secondUserId = +params['id']; // Konvertuje string u broj
      console.log('Chat with user ID:', this.secondUserId);
    });
    this.getLoggedUser();
    this.getSecondUser();
  }

  getLoggedUser(): void{
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
      this.loadMessages();
    });
  }
  getSecondUser(): void{
    this.authService.getUsername(this.secondUserId).subscribe((result) => {
      this.secondUsername = result.username;
    });
  }

  loadMessages(): void{
    this.layoutService.getAllProfileMessages(this.loggedInUser.id, this.secondUserId).subscribe({
      next: (result) => {
        console.log("********Messages: ", result);
        console.log("------ OVO SAM SLAO: ", this.loggedInUser.id, this.secondUserId);
        this.messages = result.results.map(message => ({
          ...message,
          resourcesType: this.mapMessageType(message.type)
        }));
      },
      error: (err) => {
        console.error('Failed to load tours:', err);
      }
    });
  }

  mapMessageType(type: number | undefined): string {
    switch (type) {
      case 0:
          return 'Tour';
      case 1:
          return 'Blog';
      case undefined:
          return 'Unknown';
      default:
          return 'Unknown';
  }
}

}
