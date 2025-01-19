import { Component, OnDestroy, OnInit } from '@angular/core';
import { EMPTY, interval, Observable, Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { TourExecutionService } from '../tour-execution.service';
import { KeyPoint } from '../../tour-authoring/model/key-point.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TourService } from '../../tour-authoring/tour.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { TouristLocation } from '../model/tourist-location';
import * as L from 'leaflet';
import { forkJoin } from 'rxjs';
import { TourExecution } from '../model/tour-execution';
import { Encounter } from '../../encounter-authoring/model/encounter.model';
import { EncounterExecution } from '../../encounter-execution/model/encounter-execution.model';
import { EncounterExecutionService } from '../../encounter-execution/encounter-execution.service';
import { ImagesService } from '../../tour-authoring/images.service';
@Component({
  selector: 'xp-completed-key-point',
  templateUrl: './completed-key-point.component.html',
  styleUrls: ['./completed-key-point.component.css']
})
export class CompletedKeyPointComponent implements OnInit,OnDestroy {
  keyPoints : KeyPoint[] = [];
  completedKeyPoint : KeyPoint[] = [];
  keyPointsMarker: L.LatLng[]=[]
  previousCompletedKeyPoints: number[] = [];
  loggedInUser: User;

  tour: Tour;
  tourExecution: TourExecution;

  touristLocation: TouristLocation;
  touristPosition: L.LatLng;

  encounters: Encounter[] = [];
  encounterExecution: EncounterExecution;
  encounterMarkers: { lat: number; lng: number; name: string; encounterType: number}[] = [];
  selectedCoordinates: { lat: number; lng: number } | null = null;

  /*IVA*/
  expandedEncounterId: number | null = null;
  isCompleted: Boolean | null = null;
  selectedFrame: number = 0; 
  imageMap: Map<string, string> = new Map();

  private locationCheckSubscription: Subscription;
  constructor(private tourExecutionService:TourExecutionService, private authService: AuthService,private tourService: TourService
    , private route: ActivatedRoute,  private router: Router, private encounterExecutionService: EncounterExecutionService, private imageService:ImagesService
  ){}

  ngOnInit():void{
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });

    this.route.queryParams.subscribe(params => {
      if (params['tour']) {
        this.tour = JSON.parse(params['tour']);
      }
      if (params['tourExecution']) {
        this.tourExecution = JSON.parse(params['tourExecution']);
      }
    });
  
    console.log(this.tour)
///////////////////////////////////////////////////////
      //DODAJ USLOV OVDEEEEEEEEEEEEE
    this.loadTourAndCompletedKeyPoints();
    this.getLocation();
    this.locationCheckSubscription = interval(10000)
      .pipe(
        switchMap(() => this.tourExecutionService.getLocationByTouristId(this.loggedInUser.id)), // Get current location from the service
        filter((location:TouristLocation)=>location!==undefined && location!==null),
        switchMap((location:TouristLocation) => {
          this.touristLocation = location;
          console.log(location);
          this.touristPosition = L.latLng(location.latitude, location.longitude);

          /////////// IVA ////////////////
          let encounterForChecking: Encounter | null = this.findEncounterThatsClosest(location.latitude, location.longitude);
          console.log(encounterForChecking);
          if (encounterForChecking !== null) {
            this.isEncounterCompleted(encounterForChecking.id)
            console.log(this.isCompleted);
          } else {
            console.log("encounterForChecking is null");
          }
            if (encounterForChecking != null) {
              if ( this.isCompleted && encounterForChecking.isTourRequired) {
                return this.tourExecutionService.checkLocation(
                  location,
                  this.tourExecution.id
                ); // Check if near a key point and update the status
                
              } else if (!encounterForChecking.isTourRequired) {
                return this.tourExecutionService.checkLocation(
                  location,
                  this.tourExecution.id
                ); // Check if near a key point and update the status

              } else if(encounterForChecking != null && !this.isCompleted && encounterForChecking.isTourRequired) {
                alert("Encounter needs to be completed to proceed");
                return EMPTY; // Emituje prazan tok da bi ispunio očekivanja
              }
              else{
                return this.tourExecutionService.checkLocation(
                  location,
                  this.tourExecution.id
                ); // Check if near a key point and update the status
              }
            } else {
              return this.tourExecutionService.checkLocation(
                location,
                this.tourExecution.id
              ); // Check if near a key point and update the status
            }
          })
      )

      .subscribe({
        next: (response) => {

          this.loadTourAndCompletedKeyPoints();
          this.tourExecution=response; 
          if (typeof this.tourExecution.completedPercentage === 'number' && this.tourExecution.completedPercentage === 100) {
            this.tourExecutionService.completeTour(this.tourExecution.id).subscribe({
              next:(result)=>{
              alert('Congratulations! You just finished the tour');
              console.log('Percentage:',this.tourExecution.completedPercentage);
              this.router.navigate(['tourist-tours']);
              }
            })
        } else if (this.tourExecution.completedPercentage === undefined) {
            console.error("completedPercentage is undefined. Ensure it is initialized properly.");
        }
        },
        error: (err) => {
          console.log('Greska');
        }
      });
      

      this.getAllEncounters();
  }

  ngOnDestroy(): void {
    if (this.locationCheckSubscription) {
      this.locationCheckSubscription.unsubscribe();
    }
    
  }


  getLocation():void{
    this.tourExecutionService.getLocationByTouristId(this.loggedInUser.id).subscribe({
      next:(result:TouristLocation)=>{
        if(result!==null){
          this.touristLocation=result;
          this.touristPosition = L.latLng(result.latitude, result.longitude); 
        }
      }
    })
  }


  loadTourAndCompletedKeyPoints(): void {
    forkJoin({
      tour: this.tourService.getByid(this.tourExecution.tourId),
      completedKeypointShow: this.tourExecutionService.getCompletedKeyPoints(this.tourExecution.id),
    }).subscribe({
      next: ({ tour, completedKeypointShow }) => {
        this.tour = tour;
        this.previousCompletedKeyPoints = this.completedKeyPoint.map(kp => kp.id);
        this.completedKeyPoint = completedKeypointShow;
        console.log(completedKeypointShow)
        if(this.previousCompletedKeyPoints.length<this.completedKeyPoint.length && this.previousCompletedKeyPoints.length!==0){
          alert('You have completed a new key point!');
        }
        this.getKeyPoints(tour);  
       // Call getKeyPoints only when both are loaded
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }
  

  getKeyPoints(tour:Tour):void{
    if(this.tour && this.tour.keyPoints){

      for (let kp of this.tour.keyPoints) {
        this.getImage(kp.image);
      }
      
      this.keyPoints = this.tour.keyPoints.filter(
        kp => !this.completedKeyPoint.some(completed => completed.id === kp.id )
      );
    }
    console.log(this.completedKeyPoint)
    this.keyPointsMarker = this.keyPoints.map(kp => L.latLng(kp.latitude, kp.longitude));
  }

  onMapClick(event: { lat: number, lng: number }) {
    this.touristPosition = L.latLng(event.lat, event.lng);
    ///////Iva////////
    this.selectedCoordinates = event;
    /////////////////
    //this.marker = L.marker([event.lat, event.lng]).addTo((L as any).map('objectMap'));
    if(!this.touristLocation){
      const touristLocation : TouristLocation = {
        id: 0,
        touristId: this.loggedInUser.id,
        latitude: event.lat,
        longitude: event.lng
      }
      this.tourExecutionService.addTouristLocation(touristLocation).subscribe({
        next: (result) => {
          // Update touristLocation with the result from the server
          this.touristLocation = result; // Assuming your backend returns the created location
          console.log('Tourist location added:', result);
        },
        error: (error) => {
          console.error('Error adding tourist location:', error);
        }
      });
    }else{
      this.touristLocation.latitude = event.lat;
      this.touristLocation.longitude = event.lng;
      this.tourExecutionService.updateTouristLocation(this.touristLocation).subscribe();
    }
  }

LeaveTour(){
    if (confirm('Are you sure you want to leave this tour? Note: Once leaved, you can not return to it.')) {
      this.tourExecutionService.leaveStartedTour(this.tourExecution).subscribe({
        next:(result)=>{
          this.router.navigate(['tourist-tours']); 
        }
      })   //Ovo treba da proradi
    } else {
      console.log('You did not leave the tour.');
    }
  }

  /**************** IVA ******************/
  transformEncountersToMarkers(): void {
    this.encounterMarkers = this.encounters.map(encounter => ({
      lat: encounter.latitude,
      lng: encounter.longitude,
      name: encounter.name,
      encounterType: encounter.encounterType
    }));
  }

  getAllEncounters(): void {
    this.tourExecutionService.getEncountersByTour(this.tour.id).subscribe({
      next: (data) => {
        this.encounters = data;
        this.transformEncountersToMarkers();
      },
      error: (error) => {
        console.error('Error fetching encounters:', error);
      }
    });
  }


  findEncounterThatsClosest(lat: number, lng: number): Encounter | null {
    let nearestEncounter: Encounter | null = null;
    let minDistance = Infinity;

    this.encounters.forEach((encounter) => {
      const encounterPosition = L.latLng(encounter.latitude, encounter.longitude);
      const distance = L.latLng(lat, lng).distanceTo(encounterPosition);

      if (distance < minDistance) {
        minDistance = distance;
        if(minDistance <=150)
        nearestEncounter = encounter;
      }
    });

    return nearestEncounter;
  }

  isEncounterCompleted(idEncounter: number): void {
    
    this.encounterExecutionService.HasTouristCompletedEncounter(idEncounter, this.loggedInUser.id).subscribe({
      next: (data) => {
        this.isCompleted = data;
      },
      error: (error) => {
        console.error('Error checking encounter status:', error);
      }
    });
  }

  
 
  
  
  


  


  activateEncounter(encId: number) {
    if (this.selectedCoordinates) {
      const encounterExecution: EncounterExecution = {
        touristLatitude: this.selectedCoordinates.lat,
        touristLongitude: this.selectedCoordinates.lng,
        touristId: this.loggedInUser.id,
        status: 0,
        encounterId: encId
      };
      this.tourExecutionService.activateEncounter(encounterExecution).subscribe(
        (response) => {
          this.router.navigate(['/started-encounter']);
        },
        (error) => {
          console.error('Error activating encounter:', error);
        }
      );
      this.selectedCoordinates = null;
    }
  }

  getEncounterTypeLabel(type: number): string {
    switch (type) {
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

  toggleExpand(encounterId: number): void {
    this.expandedEncounterId = this.expandedEncounterId === encounterId ? null : encounterId;
  }




  selectFrame(index: number): void {
    this.selectedFrame = index;
    this.expandedEncounterId = null;
  }
  
  getImage(path: string): void {
    this.imageService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }
  /***************************************/
}
