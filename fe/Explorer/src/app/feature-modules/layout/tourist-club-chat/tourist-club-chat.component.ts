import { Component, Input, ViewChild } from '@angular/core';
import { LayoutService } from '../layout.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { ClubMessage } from '../model/clubMessage.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../shared.service';
import { Subscription } from 'rxjs';
import { TouristClub } from '../model/touristClub.model';


@Component({
  selector: 'xp-tourist-club-chat',
  templateUrl: './tourist-club-chat.component.html',
  styleUrls: ['./tourist-club-chat.component.css']
})
export class TouristClubChatComponent {

  @Input() touristClub: TouristClub;

  messages: ClubMessage[] = []
  loggedInUser: User;
  private subscription: Subscription;
  touristClubId: number;
  touristClubName: string;
  ownerId: number;

  constructor(
    private route: ActivatedRoute,
    private layoutService: LayoutService,
    private authService: AuthService,
    private sharedService: SharedService ) {

      this.subscription = this.sharedService.eventEmitted.subscribe(() => {
        this.handleEvent();
      });
    }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.touristClubId = +params['touristClubId']; // Konvertuje string u broj
      this.touristClubName = params['touristClubName'];
      this.ownerId = params['ownerId'];
      console.log('Chat for tourist club:', this.touristClubId);
    });
    this.getLoggedUser();
  }


  handleEvent() {
    console.log('Događaj primljen u ChildB');
    this.loadMessages();
  }

  getLoggedUser(): void{
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
      this.loadMessages();
    });
  }

  loadMessages(): void{

    this.layoutService.getAllTouristClubMessagesForLoggedUser(this.touristClubId!, this.loggedInUser.id).subscribe({
      next: (result) => {
        console.log("********Messages: ", result);
        this.messages = result.results.map(message => ({
          ...message
        }));
      },
      error: (err) => {
        console.error('Failed to load tours:', err);
      }
    });

  }

}
