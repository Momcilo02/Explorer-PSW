import { Component, Input } from '@angular/core';
import { ClubMessage } from '../model/clubMessage.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { LayoutService } from '../layout.service';
import { SharedService } from '../shared.service';

@Component({
  selector: 'xp-club-message',
  templateUrl: './club-message.component.html',
  styleUrls: ['./club-message.component.css']
})
export class ClubMessageComponent {

  @Input() message: ClubMessage;
  @Input() loggedInUser: User;
  @Input() touristClubOwnerId: number;

  constructor(
    private layoutService: LayoutService,
    private sharedService: SharedService
  ) {}

  ngOnInit(){

  }

  onDeleteClicked(clubMessageId: number): void{
    console.log("****OVO **** BRISEM", clubMessageId);
    this.layoutService.deleteTouristClubMessage(clubMessageId).subscribe({
      next: (result) => {
        console.log("********Messages: ", result);
        this.sharedService.emitEvent();
      },
      error: (err) => {
        console.error('Failed to load tours:', err);
      }
    });
  }

  onEditClicked(message: ClubMessage): void{
    this.sharedService.emitEditMessageEvent(message);
  }

  onLikeClicked(message: any): void {
    if (!message.likedByLoggedUser) {
      message.likedByLoggedUser = true; // Označi da je korisnik lajkovao poruku
      message.likesCount += 1; // Inkrementiraj broj lajkova
      this.layoutService.incrementClubMessageLikes(message, message.id, this.loggedInUser.id).subscribe({
        next: (result) => {
          console.log("********Messages: ", result);
          message = result;
        },
        error: (err) => {
          console.error('Failed to load tours:', err);
        }
      });
    } else {
      message.likedByLoggedUser = false; // Ukloni lajk
      message.likesCount -= 1; // Dekrementiraj broj lajkova
      this.layoutService.decrementClubMessageLikes(message, message.id, this.loggedInUser.id).subscribe({
        next: (result) => {
          console.log("********Messages: ", result);
          message = result;
        },
        error: (err) => {
          console.error('Failed to load tours:', err);
        }
      });
    }

    // Ako treba da sačuvaš stanje na serveru, možeš pozvati API ovde.
    // this.messageService.likeMessage(message.id).subscribe();
  }
}
