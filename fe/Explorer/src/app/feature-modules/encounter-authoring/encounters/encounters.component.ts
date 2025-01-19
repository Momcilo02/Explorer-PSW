import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ImagesService } from '../../tour-authoring/images.service';
import { EncounterAuthoringService } from '../encounter-authoring.service';
import { SocialEncounters } from '../model/social-encounter.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Router } from '@angular/router';
import { HiddenEncounters } from '../model/hidden-encounter.model';
import { MiscEncounters } from '../model/misc-encounter.model';
import { ActivatedRoute } from '@angular/router';
import { LayoutService } from '../../layout/layout.service';
import { ProfileInfo } from '../../layout/model/profileInfo.model';
import { AdministrationService } from '../../administration/administration.service';

@Component({
  selector: 'xp-encounters',
  templateUrl: './encounters.component.html',
  styleUrls: ['./encounters.component.css']
})
export class EncountersComponent implements OnInit {
  
  tourName: string;
  selectedEncounterType?: number | null | undefined;
  newSocialEncounter: SocialEncounters = {
    id: 0,
    name: '',
    description: '',
    totalXp: 0,
    creatorId: 0,
    longitude: 0,
    latitude: 0,
    encounterType: 0,
    status: 0,
    TouristRequestStatus: 0,
    isTourRequired: false,
    tourId: 0,
    activateRange: 0,
    peopleNumb: 0,
};
  newHiddenEncounter: HiddenEncounters ={
    id: 0,
    name: '',
    description: '',
    totalXp: 0,
    creatorId: 0,
    longitude: 0,
    latitude: 0,
    encounterType: 0,
    status: 0,
    TouristRequestStatus: 0,
    isTourRequired: false,
    tourId: 0,
    activateRange: 0,
    image:'',
    imageLongitude:0,
    imageLatitude: 0,
};
  loggedInUser: User;


  //**********Misc Encounter *************//
  newMiscEncounter: MiscEncounters = {
    id: 0,
    name: '',
    description: '',
    totalXp: 0,
    creatorId: 0,
    longitude: 0,
    latitude: 0,
    encounterType: 2,
    status: 0,
    TouristRequestStatus: 0,
    isTourRequired: false,
    tourId: 0,
    activateRange: 0,
    instructions: '',
};

  //**************************************//

  encounterMainForm = new FormGroup({
    tourId: new FormControl(0, [Validators.required]),
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    totalXp: new FormControl(0, [Validators.required]),
    longitude: new FormControl(0, [Validators.required]),
    latitude: new FormControl(0, [Validators.required]),
    encounterType: new FormControl(3, [Validators.required]),
    activateRange:new FormControl(0, [Validators.required]),
    isTourRequired: new FormControl(false, [Validators.required]),
  });

  encounterHiddenForm = new FormGroup({
    image: new FormControl('', [Validators.required]),
    imageLongitude: new FormControl(0, [Validators.required]),
    imageLatitude: new FormControl(0, [Validators.required]),
  });


  encounterSocialForm = new FormGroup({
    peopleNumb:new FormControl(0, [Validators.required]),
  });

  encounterMiscForm = new FormGroup({
    instructions:new FormControl('', [Validators.required]),
  });



  constructor(
    private imgService: ImagesService, 
    private service: EncounterAuthoringService,
    private authService: AuthService,
    private layoutService: LayoutService,
    private router: Router,
    private route: ActivatedRoute,
    private adminService : AdministrationService
  ) {}

  userInfo: ProfileInfo;
  
  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.getCurrentUser(); 
    this.route.queryParams.subscribe(params => {
      if (params['latitude'] && params['longitude']) {
        // Ažuriranje forme koristeći patchValue
        this.encounterMainForm.patchValue({
          latitude: Number(params['latitude']),
          longitude: Number(params['longitude']),
          tourId: Number(params['id'])
        });
        this.fetchTourById();
        console.log(this.tourName);
      }
    });

    
  }

  getCurrentUser(){
    this.layoutService.fetchCurrentUser().subscribe((user) => {
       this.userInfo = user;
       console.log(this.userInfo);
       this.isUserSuitable();
    });
  }
  isUserSuitable(){
    console.log("Da li on sme da pristupi ovome?")
    if(this.userInfo.touristLevel < 10 && this.loggedInUser.role == "tourist"){
      alert("You need to reach level 10 to be able to create encounter!");
      this.router.navigate(['/home']);
    }
  }
  // Reakcija na promenu encounterType
  onEncounterTypeChange(): void {
    this.selectedEncounterType = this.encounterMainForm.value.encounterType;
    console.log('Selected Encounter Type:', this.selectedEncounterType);
  }

  // Prikaz validacije
  isFieldInvalid(fieldName: string): boolean {
    const field = this.encounterMainForm.get(fieldName);
    return !!field?.invalid && (field?.touched || field?.dirty);
  }

  // Podnošenje forme
  onSubmit(): void {
    if (this.encounterMainForm.valid && this.loggedInUser.role != 'author') {

      if (this.selectedEncounterType == 0) {
        if(this.encounterMainForm.value.description)
          this.newSocialEncounter.description = this.encounterMainForm.value.description;
        if(this.encounterMainForm.value.name)
          this.newSocialEncounter.name = this.encounterMainForm.value.name;
        if(this.encounterMainForm.value.longitude)
          this.newSocialEncounter.longitude = this.encounterMainForm.value.longitude;
        if(this.encounterMainForm.value.latitude)
          this.newSocialEncounter.latitude = this.encounterMainForm.value.latitude;
        if(this.encounterMainForm.value.activateRange)
          this.newSocialEncounter.activateRange = this.encounterMainForm.value.activateRange;
        if(this.encounterSocialForm.value.peopleNumb)
          this.newSocialEncounter.peopleNumb = this.encounterSocialForm.value.peopleNumb;
        if(this.encounterMainForm.value.totalXp)
          this.newSocialEncounter.totalXp = this.encounterMainForm.value.totalXp;


        this.newSocialEncounter.creatorId = this.loggedInUser.id;
        this.newSocialEncounter.encounterType = 0;

        if(this.loggedInUser.role == 'tourist'){
          this.newSocialEncounter.TouristRequestStatus = 1;
        } else 
          this.newSocialEncounter.TouristRequestStatus = 3;

        this.service.addSocialEncounter(this.newSocialEncounter,this.loggedInUser).subscribe({
        next: (res) => {
          console.log('Social encounter added:', res);
          this.encounterMainForm.reset(); 
          this.encounterSocialForm.reset();
          this.encounterHiddenForm.reset();
          this.encounterMiscForm.reset();
          this.router.navigate(['encounter-view']);
        },
          error: (err) => console.error('Error adding social encounter:', err)
        });
      }
      else if (this.selectedEncounterType == 1) {
        if(this.encounterMainForm.value.description)
          this.newHiddenEncounter.description = this.encounterMainForm.value.description;
        if(this.encounterMainForm.value.name)
          this.newHiddenEncounter.name = this.encounterMainForm.value.name;
        if(this.encounterMainForm.value.longitude)
          this.newHiddenEncounter.longitude = this.encounterMainForm.value.longitude;
        if(this.encounterMainForm.value.latitude)
          this.newHiddenEncounter.latitude = this.encounterMainForm.value.latitude;
        if(this.encounterMainForm.value.activateRange)
          this.newHiddenEncounter.activateRange = this.encounterMainForm.value.activateRange;
        if(this.encounterMainForm.value.totalXp)
          this.newHiddenEncounter.totalXp = this.encounterMainForm.value.totalXp;

        if(this.encounterHiddenForm.value.image)
          this.newHiddenEncounter.image = this.encounterHiddenForm.value.image;
        if(this.encounterHiddenForm.value.imageLongitude)
          this.newHiddenEncounter.imageLongitude = this.encounterHiddenForm.value.imageLongitude;
        if(this.encounterHiddenForm.value.imageLatitude)
          this.newHiddenEncounter.imageLatitude = this.encounterHiddenForm.value.imageLatitude;


        this.newHiddenEncounter.creatorId = this.loggedInUser.id;
        this.newHiddenEncounter.encounterType = 1;

        if(this.loggedInUser.role == 'tourist'){
          this.newHiddenEncounter.TouristRequestStatus = 1;
        }else
        this.newHiddenEncounter.TouristRequestStatus = 3;
        console.log(this.newHiddenEncounter);

        this.service.addHiddenEncounter(this.newHiddenEncounter,this.loggedInUser).subscribe({
        next: (res) => {
          console.log('Hidden location encounter added:', res);
          this.encounterMainForm.reset(); 
          this.encounterSocialForm.reset();
          this.encounterHiddenForm.reset();
          this.encounterMiscForm.reset();
          this.router.navigate(['encounter-view']);
        },
          error: (err) => console.error('Error adding hidden location encounter:', err)
        });
      }
     if (this.selectedEncounterType == 2) {

          if(this.encounterMainForm.value.description)
            this.newMiscEncounter.description = this.encounterMainForm.value.description;

          if(this.encounterMainForm.value.name)
            this.newMiscEncounter.name = this.encounterMainForm.value.name;

          if(this.encounterMainForm.value.longitude)
            this.newMiscEncounter.longitude = this.encounterMainForm.value.longitude;

          if(this.encounterMainForm.value.latitude)
            this.newMiscEncounter.latitude = this.encounterMainForm.value.latitude;

          if(this.encounterMainForm.value.activateRange)
            this.newMiscEncounter.activateRange = this.encounterMainForm.value.activateRange;

          if(this.encounterMiscForm.value.instructions)
            this.newMiscEncounter.instructions = this.encounterMiscForm.value.instructions;
         
          if(this.encounterMainForm.value.totalXp)
            this.newMiscEncounter.totalXp = this.encounterMainForm.value.totalXp;
  
  
          this.newMiscEncounter.creatorId = this.loggedInUser.id;
          this.newMiscEncounter.encounterType = 2;

          if(this.loggedInUser.role == 'tourist'){
            this.newMiscEncounter.TouristRequestStatus = 1;
          } else 
            this.newMiscEncounter.TouristRequestStatus = 3;
          this.service.addMiscEncounter(this.newMiscEncounter,this.loggedInUser).subscribe({
          next: (res) => {
            console.log('Misc encounter added:', res);
            this.encounterMainForm.reset(); 
            this.encounterSocialForm.reset();
            this.encounterHiddenForm.reset();
            this.encounterMiscForm.reset();
            this.router.navigate(['/encounter-view']);
          },
            error: (err) => console.error('Error adding social encounter:', err)
          });
        }

    }


//Logika za dodavanje od autora

    else if(this.loggedInUser.role == 'author'){
      if (this.selectedEncounterType == 0) {
        if(this.encounterMainForm.value.description)
          this.newSocialEncounter.description = this.encounterMainForm.value.description;
        if(this.encounterMainForm.value.name)
          this.newSocialEncounter.name = this.encounterMainForm.value.name;
        if(this.encounterMainForm.value.longitude)
          this.newSocialEncounter.longitude = this.encounterMainForm.value.longitude;
        if(this.encounterMainForm.value.latitude)
          this.newSocialEncounter.latitude = this.encounterMainForm.value.latitude;
        if(this.encounterMainForm.value.activateRange)
          this.newSocialEncounter.activateRange = this.encounterMainForm.value.activateRange;
        if(this.encounterSocialForm.value.peopleNumb)
          this.newSocialEncounter.peopleNumb = this.encounterSocialForm.value.peopleNumb;
        if(this.encounterMainForm.value.totalXp)
          this.newSocialEncounter.totalXp = this.encounterMainForm.value.totalXp;
        
        if(this.encounterMainForm.value.isTourRequired = true)
          this.newSocialEncounter.isTourRequired = true;
        else
          this.newSocialEncounter.isTourRequired = false

        this.newSocialEncounter.tourId = Number(this.encounterMainForm.value.tourId);
        this.newSocialEncounter.creatorId = this.loggedInUser.id;
        this.newSocialEncounter.encounterType = 0;

        this.service.addSocialEncounterAuthor(this.newSocialEncounter).subscribe({
        next: (res) => {
          console.log('Social encounter added:', res);
          this.encounterMainForm.reset(); 
          this.encounterSocialForm.reset();
          this.encounterHiddenForm.reset();
          this.encounterMiscForm.reset();
        },
          error: (err) => console.error('Error adding social encounter:', err)
        });
      }
      else if (this.selectedEncounterType == 1) {
        if(this.encounterMainForm.value.description)
          this.newHiddenEncounter.description = this.encounterMainForm.value.description;
        if(this.encounterMainForm.value.name)
          this.newHiddenEncounter.name = this.encounterMainForm.value.name;
        if(this.encounterMainForm.value.longitude)
          this.newHiddenEncounter.longitude = this.encounterMainForm.value.longitude;
        if(this.encounterMainForm.value.latitude)
          this.newHiddenEncounter.latitude = this.encounterMainForm.value.latitude;
        if(this.encounterMainForm.value.activateRange)
          this.newHiddenEncounter.activateRange = this.encounterMainForm.value.activateRange;
        if(this.encounterMainForm.value.totalXp)
          this.newHiddenEncounter.totalXp = this.encounterMainForm.value.totalXp;

        if(this.encounterHiddenForm.value.image)
          this.newHiddenEncounter.image = this.encounterHiddenForm.value.image;
        if(this.encounterHiddenForm.value.imageLongitude)
          this.newHiddenEncounter.imageLongitude = this.encounterHiddenForm.value.imageLongitude;
        if(this.encounterHiddenForm.value.imageLatitude)
          this.newHiddenEncounter.imageLatitude = this.encounterHiddenForm.value.imageLatitude;

        if(this.encounterMainForm.value.isTourRequired = true)
          this.newHiddenEncounter.isTourRequired = true;
        else
          this.newHiddenEncounter.isTourRequired = false

        this.newHiddenEncounter.tourId = Number(this.encounterMainForm.value.tourId);
        this.newHiddenEncounter.creatorId = this.loggedInUser.id;
        this.newHiddenEncounter.encounterType = 1;

        console.log(this.newHiddenEncounter);

        this.service.addHiddenEncounterAuthor(this.newHiddenEncounter).subscribe({
        next: (res) => {
          console.log('Hidden location encounter added:', res);
          this.encounterMainForm.reset(); 
          this.encounterSocialForm.reset();
          this.encounterHiddenForm.reset();
          this.encounterMiscForm.reset();
        },
          error: (err) => console.error('Error adding hidden location encounter:', err)
        });
      }
     if (this.selectedEncounterType == 2) {

          if(this.encounterMainForm.value.description)
            this.newMiscEncounter.description = this.encounterMainForm.value.description;

          if(this.encounterMainForm.value.name)
            this.newMiscEncounter.name = this.encounterMainForm.value.name;

          if(this.encounterMainForm.value.longitude)
            this.newMiscEncounter.longitude = this.encounterMainForm.value.longitude;

          if(this.encounterMainForm.value.latitude)
            this.newMiscEncounter.latitude = this.encounterMainForm.value.latitude;

          if(this.encounterMainForm.value.activateRange)
            this.newMiscEncounter.activateRange = this.encounterMainForm.value.activateRange;

          if(this.encounterMiscForm.value.instructions)
            this.newMiscEncounter.instructions = this.encounterMiscForm.value.instructions;
         
          if(this.encounterMainForm.value.totalXp)
            this.newMiscEncounter.totalXp = this.encounterMainForm.value.totalXp;
  
          if(this.encounterMainForm.value.isTourRequired = true)
            this.newMiscEncounter.isTourRequired = true;
          else
            this.newMiscEncounter.isTourRequired = false
           

          this.newMiscEncounter.tourId = Number(this.encounterMainForm.value.tourId);
          this.newMiscEncounter.creatorId = this.loggedInUser.id;
          this.newMiscEncounter.encounterType = 2;
          this.service.addMiscEncounterAuthor(this.newMiscEncounter).subscribe({
          next: (res) => {
            console.log('Misc encounter added:', res);
            this.encounterMainForm.reset(); 
            this.encounterSocialForm.reset();
            this.encounterHiddenForm.reset();
            this.encounterMiscForm.reset();
          },
            error: (err) => console.error('Error adding social encounter:', err)
          });
        }
    }
     else {
      console.log('Form is invalid');
    }
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
      this.imgService.addImage(formData).subscribe({
        next: (res) =>{
          this.encounterHiddenForm.value.image = res.filePath;
        }
      });
    }
  }

  lockEncounterCoordinates : boolean = false;

  LockCoordinates(){
    this.lockEncounterCoordinates = !this.lockEncounterCoordinates;

  }
  onMapClick(event: { lat: number; lng: number }): void {

    if(!this.lockEncounterCoordinates && this.loggedInUser.role !== 'author'){
    this.encounterMainForm.patchValue({
      latitude: event.lat,
      longitude: event.lng 
    });
    } else if(this.lockEncounterCoordinates){
      this.encounterHiddenForm.patchValue({
        imageLatitude: event.lat,
        imageLongitude: event.lng
      })
    }
    else{
      alert(
        'Choose encounter type hidden and lock in coordinates to set picture location'
      );
    }
  }


  fetchTourById(): void {
    this.adminService.getTourById(Number(this.encounterMainForm.value.tourId)).subscribe({
      next: (tour) => {
        if (tour && tour.name) {
          this.tourName = tour.name;
        }
      },
      error: (err) => {
        console.log(`Error fetching tour for tourId ${this.encounterMainForm.value.tourId}:`, err);
      },
    });
  }

}
