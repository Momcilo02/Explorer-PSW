import { Component, OnInit } from '@angular/core';
import { EncounterExecutionService } from '../encounter-execution.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { EncounterExecution } from '../model/encounter-execution.model';
import { ActiveEncounter } from '../model/active-encounter.model';
import { Router } from '@angular/router';
import { ImagesService } from '../../tour-authoring/images.service';
import { LayoutService } from '../../layout/layout.service';
import { ProfileInfo } from '../../layout/model/profileInfo.model';

@Component({
  selector: 'xp-started-encounter',
  templateUrl: './started-encounter.component.html',
  styleUrls: ['./started-encounter.component.css']
})
export class StartedEncounterComponent implements OnInit {
  encounterExecution: EncounterExecution;
  encounterExecutionWithDetails: ActiveEncounter;
  loggedInUser: User;
  userInfo: ProfileInfo;
  intervalsWaited: number = 0;

  imageMap: Map<string, string> = new Map();
  hiddenEncounterImage : string;

  
  //currentEncounter: Encounter;
  selectedCoordinates: { lat: number; lng: number } | null = null;
  encounterMarkers: { lat: number; lng: number; name: string; encounterType: number}[] = [];
  intervalId: any;
  isModalOpen: boolean = false; // Track modal visibility
  currentImageSrc: string = ''; // Store the current image source
  isRankUpModalOpen: boolean = false;
  constructor(
    private service: EncounterExecutionService,
    private authService: AuthService,
    private router: Router,
    private imagesService: ImagesService,
    private layoutService: LayoutService
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.getCurrentTourist();
    
    this.getActivatedEncounter();
    this.updateUserLocation();
  }

  getCurrentTourist(){
    this.layoutService.fetchCurrentUser().subscribe((user) => {
       this.userInfo = user;
       console.log(this.userInfo);
    });
  }

  getActivatedEncounter(): void {
    this.service.getActivatedEncounter(this.loggedInUser.id).subscribe({
      next: (data) => {
        this.encounterExecution = data;
        this.loadEncounterDetails();
      },
      error: (error) => {
        console.error('Error fetching encounters:', error);
      }
    });
  }

  loadEncounterDetails(): void {
    if (this.encounterExecution) {
      this.service.getEncounterById(this.encounterExecution.encounterId).toPromise()
        .then((encounter) => {
          this.encounterExecutionWithDetails = {
            ...this.encounterExecution,
            encounter: encounter
          };
          if(this.encounterExecutionWithDetails.encounter?.encounterType == 1){
            this.hiddenEncounterImage = this.encounterExecutionWithDetails.encounter.image || '';
            this.getImage(this.hiddenEncounterImage);
          }
          this.transformEncountersToMarkers();
        })
        .catch((error) => {
          console.error('Error loading encounter details:', error);
        });
    }
  }

  getEncounterTypeLabel(encounterType?: number): string {
    switch (encounterType) {
      case 0:
        return 'Social';
      case 1:
        return 'Hidden';
      case 2:
        return 'Misc';
      default:
        return 'Unknown';
    }
  }

  transformEncountersToMarkers(): void {
    if (this.encounterExecutionWithDetails && this.encounterExecutionWithDetails.encounter) {
      const encounter = this.encounterExecutionWithDetails.encounter;
      this.encounterMarkers = [{
        lat: encounter.latitude,
        lng: encounter.longitude,
        name: encounter.name,
        encounterType: encounter.encounterType
      }];
    } else {
      this.encounterMarkers = [{
        lat: 0,
        lng: 0,
        name: '',
        encounterType: 0
      }];
    }
  }

  onMapClick(event: { lat: number; lng: number }) {
    this.selectedCoordinates = event;
  }

  updateUserLocation(): void {
    this.intervalId = setInterval(() => {
      if (this.selectedCoordinates) {
        this.encounterExecution.touristLatitude = this.selectedCoordinates.lat;
        this.encounterExecution.touristLongitude = this.selectedCoordinates.lng;

        const outOfRange = !this.isWithinRange(
          this.selectedCoordinates.lat,
          this.selectedCoordinates.lng
        );
        if (outOfRange) {
          if (confirm('You are out of range. Are you sure you want to leave the challenge?')) {
            this.service.leaveEncounter(this.encounterExecution).subscribe({
              next: (data) => {
                  this.router.navigate(['/encounter-view']);
                  clearInterval(this.intervalId);
              },
              error: (error) => {
                console.error('Error leaving encounter:', error);
              }
            });
          } else { 
            return;
          }
        }  
        else {
          this.service.updateUserLocation(this.encounterExecution, this.encounterExecutionWithDetails.encounter?.peopleNumb!).subscribe({
            next: (data) => {
              console.log('Location updated:', data);
              if (data.status == 1) { 
                alert('Challenge completed!');
                this.router.navigate(['/encounter-view']);
                clearInterval(this.intervalId);
              } //dodati prikaz broja aktivnih
            },
            error: (error) => {
              console.error('Error updating location:', error);
            }
            });

            if(this.encounterExecutionWithDetails.encounter?.encounterType == 1){
              const encounter = this.encounterExecutionWithDetails.encounter; 
              let imageLongitude = encounter.imageLongitude || 0;
              let imageLatitude = encounter.imageLatitude  || 0; 

              let imageDistance = this.calculateDistance(this.selectedCoordinates.lat, this.selectedCoordinates.lng, imageLatitude, imageLongitude)
             
              if(imageDistance < 50)
                this.intervalsWaited++;
              else
                this.intervalsWaited = 0;
              
              console.log("Koliko je intervala proslo:" + this.intervalsWaited);
              if(this.intervalsWaited == 4)
                this.setToCompleted()
            }


          }
        }
    }, 10000);
  }
  
  isWithinRange(lat: number, lng: number): boolean {
    if (!this.encounterExecutionWithDetails || !this.encounterExecutionWithDetails.encounter) return true;

    const encounter = this.encounterExecutionWithDetails.encounter;
    const distance = this.calculateDistance(lat, lng, encounter.latitude, encounter.longitude);

    if(!(this.encounterExecutionWithDetails.encounter.encounterType == 1))
    return distance <= encounter.activateRange;

    else {
      let imageLongitude = encounter.imageLongitude || 0;
      let imageLatitude = encounter.imageLatitude  || 0;
      let howFar = this.calculateDistance(encounter.latitude, encounter.longitude, imageLatitude, imageLongitude);
      howFar = howFar + encounter.activateRange;
      return distance <= howFar;
    }

    
  }

  calculateDistance(lat1: number, lng1: number, lat2: number, lng2: number): number {
    const earthRadius = 6371; // Radius of Earth in kilometers
    const dLat = this.degreesToRadians(lat2 - lat1);
    const dLng = this.degreesToRadians(lng2 - lng1);
    const a = 
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(this.degreesToRadians(lat1)) * Math.cos(this.degreesToRadians(lat2)) * 
      Math.sin(dLng / 2) * Math.sin(dLng / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return earthRadius * c * 1000; // Return distance in meters
  }

  degreesToRadians(degrees: number): number {
    return degrees * (Math.PI / 180);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  getImage(path: string): void {
    this.imagesService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }

  /******* IVA *******/
  setToCompleted(){
    this.intervalsWaited = 0;
    this.service.setEncounterOnCompleted(this.encounterExecution).subscribe({
      next: (data) => {
        alert('Challenge completed! You just earned ' + this.encounterExecutionWithDetails.encounter?.totalXp + " xp points!");
        this.calculateLevel();
      },
      error: (error) => {
        console.error('Error updating location:', error);
      }
    })
  }


  openImageModal(imageSrc: string): void {
    this.currentImageSrc = imageSrc;
    this.isModalOpen = true;
  }

  // Close the image modal
  closeImageModal(): void {
    this.isModalOpen = false;
    this.currentImageSrc = '';
  }
  /*******************/


 calculateLevel(){
  this.userInfo.touristXp += this.encounterExecutionWithDetails.encounter?.totalXp || 0 ;
  let currentLevel = this.userInfo.touristLevel;
  if(this.userInfo.touristXp >= 100 )
    currentLevel = 1;

  if(this.userInfo.touristXp >= 200 )
    currentLevel= 2;

  if(this.userInfo.touristXp >= 300 )
    currentLevel = 3;

  if(this.userInfo.touristXp >= 400 )
    currentLevel = 4;

  if(this.userInfo.touristXp >= 500 )
    currentLevel = 5;

  if(this.userInfo.touristXp >= 600 )
    currentLevel = 6;

  if(this.userInfo.touristXp >= 700 )
    currentLevel = 7;

  if(this.userInfo.touristXp >= 800 )
    currentLevel = 8;

  if(this.userInfo.touristXp >= 900 )
    currentLevel = 9;

  if(this.userInfo.touristXp >= 1000 )
    currentLevel = 10;

  if(currentLevel != this.userInfo.touristLevel){
    this.userInfo.touristLevel = currentLevel;
    alert('Congratulations you leveled up! New level is ' + currentLevel);
  }

  this.service.updateCurrentUser(this.userInfo).subscribe({
    next:( data ) =>{
        this.userInfo = data;
    }
  });
  this.rankUp();
 }

 rankUp(): void {
  let currentRank = this.userInfo.touristRank;
  if (this.userInfo.touristLevel === 2) 
    currentRank = 1;
  else if (this.userInfo.touristLevel === 4) 
    currentRank = 2;
  else if (this.userInfo.touristLevel === 6) 
    currentRank = 3;
  else if (this.userInfo.touristLevel === 8) 
    currentRank = 4;
  else 
    currentRank = this.userInfo.touristRank;

  
  if(currentRank != this.userInfo.touristRank){
    this.userInfo.touristRank = currentRank;
    this.isRankUpModalOpen = true;
    this.service.updateCurrentUser(this.userInfo).subscribe({
      next:( data ) =>{
          this.userInfo = data;
      }
    });
  }
  else
    this.router.navigate(['/encounter-view']);
 }

 closeRankUpModal(): void {
  this.isRankUpModalOpen = false;
  this.router.navigate(['/encounter-view']);
  }

}


