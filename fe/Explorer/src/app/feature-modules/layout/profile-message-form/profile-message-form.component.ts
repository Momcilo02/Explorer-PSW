import { Component, Input } from '@angular/core';
import { LayoutService } from '../layout.service';
import { Tour } from '../../tour-authoring/model/tour.model';
import { Blogs } from '../../blog/model/blog.model';
import { ProfileMessage } from '../model/profileMessage.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'xp-profile-message-form',
  templateUrl: './profile-message-form.component.html',
  styleUrls: ['./profile-message-form.component.css']
})
export class ProfileMessageFormComponent {

  @Input() name!: string;
  @Input() surname!: string;
  @Input() receiverId!: number;

  message: string = '';
  remainingChars: number = 280;
  selectedResource: { id: number; type: string; name: string };
  resources: { id: number; type: string; name: string }[] = [];
  sendingFeedback?: string;
  loggedInUser: User;

  constructor(
    private layoutService: LayoutService,
    private authService: AuthService ){}

  ngOnInit() {
    this.getAllTours();
    this.getAllBlogs();
    this.getLoggedUser();
  }


  getLoggedUser(): void{
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
  }

  getAllTours(): void{
    this.layoutService.getAllTours().subscribe({
      next: (result) => {
        console.log("********TUREEEE: ", result);

      const tours = result.results.map((tour: Tour) => ({
        id: tour.id as number,
        type: 'Tura',
        name: tour.name,
      }));

      console.log("////////////  tours: ", tours);

      this.resources = [...this.resources, ...tours];
      },
      error: (err) => {
        console.error('Failed to load tours:', err);
      }
    });
  }

  getAllBlogs(): void{
    this.layoutService.getAllBlogs().subscribe({
      next: (result) => {
        console.log("********BLOGGGSS: ", result);

        const blogs = result.results.map((blog: Blogs) => ({
          id: blog.id as number,
          type: 'Blog',
          name: blog.title,
        }));

        console.log("////////////  blogs: ", blogs);

        this.resources = [...this.resources, ...blogs];
      },
      error: (err) => {
        console.error('Failed to load tours:', err);
      }
    });
  }

  updateRemainingChars() {
    this.remainingChars = 280 - this.message.length;
    this.sendingFeedback = "";
  }

  onSubmit() {
    if (this.remainingChars >= 0) {
      const newMessage: ProfileMessage = {
        id: 0,
        senderName: "",
        senderSurname: "",
        senderId: this.loggedInUser.id,
        receiverId: this.receiverId,
        content: this.message,
        sentDate: new Date(),
        resourcesId: this.selectedResource.id,
        resourcesType: this.selectedResource.type,
        resourcesName: this.selectedResource.name
      }

      this.layoutService.sendProfileMessage(newMessage).subscribe({
        next: (result) => {
          console.log("/////////////*********' ", result);
          this.sendingFeedback = "Message sent successfully";
          this.message = "";

        },
        error: (err) => {
          console.log("###### RESOURCES #######", this.resources);
          this.sendingFeedback = "Something went wrong!";
          console.log(this.selectedResource);
          console.log(newMessage);
          console.error('Failed to load tours:', err);
        }
      });
    }
  }

}
