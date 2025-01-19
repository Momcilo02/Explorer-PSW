import { Component, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { LayoutService } from '../layout.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { ClubMessage } from '../model/clubMessage.model';
import { TouristClub } from '../model/touristClub.model';
import { SharedService } from '../shared.service';


@Component({
  selector: 'xp-club-message-form',
  templateUrl: './club-message-form.component.html',
  styleUrls: ['./club-message-form.component.css']
})
export class ClubMessageFormComponent {

  @Input() touristClubId: number;

  sendingFeedback?: string;
  message: string = '';
  remainingChars: number = 280;
  loggedInUser: User;
  editView: boolean = false;
  editingMessage: ClubMessage;
  touristClub: TouristClub;

  constructor(
    private layoutService: LayoutService,
    private authService: AuthService,
    private sharedService: SharedService ){

      this.sharedService.messageEditEvent.subscribe((clubMessage: ClubMessage) => {
        this.editView = true;
        this.message = clubMessage.content;
        this.editingMessage = clubMessage;
      });
    }


  ngOnInit() {
    this.getTouristClub();
    this.getLoggedUser();
  }

  getTouristClub(): void{
    console.log("SHONEEEEEEEEEEEE: ", this.touristClubId);
    this.layoutService.getTouristClubById(this.touristClubId).subscribe((result) => {
      this.touristClub = result;
      console.log("REZULTAT: ---> ", this.touristClub);
    },
    (error) => {
      console.error("Error fetching tourist club: ", error);
    })
  }

  getLoggedUser(): void{
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
  }

  updateRemainingChars() {
    this.remainingChars = 280 - this.message.length;
    this.sendingFeedback = "";
  }

  onSubmit() {
    if (this.remainingChars >= 0) {
      const newMessage: ClubMessage = {
        id: 0,
        senderName: "",
        senderSurname: "",
        senderId: this.loggedInUser.id,
        touristClubId: this.touristClub.id!,
        content: this.message,
        sentDate: new Date(),
        likesCount: 0,
        likedByLoggedUser: false,
      }

      this.layoutService.createTouristClubMessage(newMessage).subscribe({
        next: (result) => {
          console.log("/////////////*********' ", result);
          console.log("/////////// ovo sam poslao //////////", newMessage);
          this.sendingFeedback = "Message sent successfully";
          this.message = "";
          this.sharedService.emitEvent();
        },
        error: (err) => {
          this.sendingFeedback = "Something went wrong!";
          console.log(newMessage);
          console.error('Failed to load tours:', err);
        }
      });

    }
  }

  onEdit(): void{
    if (this.remainingChars >= 0) {
      const newMessage: ClubMessage = {
        id: this.editingMessage.id,
        senderName: this.editingMessage.senderName,
        senderSurname: this.editingMessage.senderSurname,
        senderId: this.editingMessage.senderId,
        touristClubId: this.editingMessage.touristClubId,
        content: this.message,
        sentDate: new Date(),
        likesCount: 0,
        likedByLoggedUser: false,
      }

      this.layoutService.updateTouristClubMessage(newMessage.id, newMessage).subscribe({
        next: (result) => {
          console.log("/////////////*********' ", result);
          this.sendingFeedback = "Message sent successfully";
          this.message = "";
          this.sharedService.emitEvent();
          this.editView = false;
        },
        error: (err) => {
          this.sendingFeedback = "Something went wrong!";
          console.log(newMessage);
          console.error('Failed to load tours:', err);
          this.editView = false;
        }
      });

    }
  }

}
