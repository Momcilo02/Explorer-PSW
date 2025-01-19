import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LayoutService } from '../layout.service';
import { TouristClub } from '../model/touristClub.model';
import {User} from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { ImagesService } from '../../tour-authoring/images.service';

@Component({
  selector: 'xp-tourist-club-form',
  templateUrl: './tourist-club-form.component.html',
  styleUrls: ['./tourist-club-form.component.css']
})
export class TouristClubFormComponent {

  @Output() touristClubsUpdated = new EventEmitter<null>();
  @Input() touristClub: TouristClub;
  @Input() shouldEdit: boolean = false;
  loggedInUser:User;

  constructor(private service: LayoutService, private authService: AuthService, private imageService:ImagesService){ }

  ngOnChanges(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.touristClubForm.reset();
    if(this.shouldEdit) {
      this.touristClubForm.patchValue(this.touristClub);
    }
  }

  touristClubForm = new FormGroup({
    name : new FormControl('',[Validators.required]),
    description: new FormControl('',Validators.required),
    picture: new FormControl(''),
    ownerId: new FormControl(-1)
  });

  addTouristClub() : void {
    console.log(this.touristClubForm.value)

    const touristClub: TouristClub =  {
         name: this.touristClubForm.value.name || "",
         description: this.touristClubForm.value.description || "",
         picture : this.touristClubForm.value.picture || "",
         ownerId: Number(this.loggedInUser.id)
    }
    this.service.addTouristClub(touristClub).subscribe({
      next: (_) => {
         console.log("Succesfull request!");
         this.touristClubsUpdated.emit();
      }
    });
    this.touristClubForm.reset();
  }

  updateTouristClub(): void {
    const touristClub: TouristClub =  {
      name: this.touristClubForm.value.name || "",
      description: this.touristClubForm.value.description || "",
      picture : this.touristClubForm.value.picture || "",
      ownerId: Number(this.loggedInUser.id)
    };
    touristClub.id = this.touristClub.id;
    touristClub.ownerId = this.loggedInUser.id;
    this.service.updateTouristClub(touristClub).subscribe({
      next: () => { this.touristClubsUpdated.emit();}
    });
    this.touristClubForm.reset();
  }


  onFileSelected(event:Event){
    const target = event.target as HTMLInputElement;
    if (target.files) {
      this.uploadImage(target.files[0]);
    }
  }

  uploadImage(file: File) {
    if (file) {
      const formData = new FormData();
      formData.append('file', file, file.name);
      this.imageService.addImage(formData).subscribe({
        next: (res) =>{
          this.touristClubForm.value.picture = res.filePath;
        }
      });
    }
  }


}
