import { ChangeDetectorRef, Component, OnInit,ViewChild } from '@angular/core';
import { KeyPoint } from '../model/key-point.model';
import { TourService } from '../tour.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Tour } from '../model/tour.model';
import { nextDay } from 'date-fns';
import { ImagesService } from '../images.service';
import { KeyPointFormComponent } from '../key-point-form/key-point-form.component';
import * as L from 'leaflet';
import { loadConfigFromFile } from 'vite';
import { PRECONNECT_CHECK_BLOCKLIST } from '@angular/common';
import { TourDuration } from '../model/tour-duration.model';
import { MatSnackBar } from '@angular/material/snack-bar';
// DODATO OVO IZNAD BIBLIOTEKA LEAFLET


@Component({
  selector: 'xp-key-point',
  templateUrl: './key-point.component.html',
  styleUrls: ['./key-point.component.css']
})
export class KeyPointComponent implements OnInit {
  keyPoints : KeyPoint[] = [];
  tour: Tour;
  selectedKeyPoint: KeyPoint;
  shouldEdit: boolean = false;
  shouldAdd: boolean = false;
  imageMap: Map<string, string> = new Map();
  @ViewChild('keyPointForm') keyPointForm!: KeyPointFormComponent;
  externalPoints: L.LatLng[]=[]
  tourLengthValue: number=0;
  triggerMap: boolean = false;
  clicked: boolean = false;
  longitude: number = 0;
  latitude: number = 0;

  constructor(private tourService: TourService, private route: ActivatedRoute, private router: Router, private imagesService: ImagesService,private snackBar: MatSnackBar, private cdr: ChangeDetectorRef){}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id') || 0;

      this.tourService.getTour(Number(id)).subscribe({
        next: (res) =>{
          console.log(res)
          this.tour = res;
          this.keyPoints = this.tour?.keyPoints!;
          // this.tour.tourDurations
          if(this.keyPoints.length<2)
            {
              this.tour.length=0;
              this.updateTourLength();
            }

          for (let kp of this.keyPoints) {
            this.getImage(kp.image);
          }
          this.externalPoints = this.keyPoints.map(kp => L.latLng(kp.latitude, kp.longitude));
          // window.scrollTo(0, 0);
        }
      })
    })
  }

  add(){
    if(!this.clicked){
      this.snackBar.open('You need to click on map first!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'info',
      });
      return;
    }
    this.shouldAdd = true;
    this.shouldEdit = false;
    this.clicked = false;
  }

  addKeypoint(keyPoint: any) {
    this.shouldEdit=false;
    this.tour.keyPoints.push(keyPoint);
    this.keyPoints = this.tour.keyPoints;
    this.getImage(keyPoint.image);
    if(this.keyPoints.length>0)
    {
      this.updateLength(this.tourLengthValue)
    }
    this.updateTourLength();
    //window.scrollTo(0, 0);

    // window.location.reload();
    this.snackBar.open('Successfully added keypoint!', 'Close', {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: 'success',
    });
    this.closeModal();
    this.longitude = 0;
    this.latitude = 0;
  }
  updateTour(){
    this.tourService.updatedTour(this.tour).subscribe({
      next: (res) =>{
        this.tour = res;
        this.keyPoints = res.keyPoints;
      }
    })
  }
  updateKeyPoint(keyPoint: any){
    this.tour.keyPoints = this.tour.keyPoints.map(kp =>
      kp.id === keyPoint.id ? keyPoint : kp
    );
    this.updateTour();
    this.keyPoints = this.tour.keyPoints;
    window.scrollTo(0, 0);
    this.snackBar.open('Successfully updated keypoint!', 'Close', {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: 'success',
    });
    this.closeModal();
    this.longitude = 0;
    this.latitude = 0;
  }

  updateTourLength()
  {
    this.tourService.updateTourLength(this.tour).subscribe({
      next: (res) =>{
        this.tour = res;
        this.keyPoints = res.keyPoints;
      }
    })
  }

  back() {
    this.router.navigate(['tour']);
  }

  getImage(path: string): void {
    this.imagesService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }

  edit(kp: KeyPoint) {
    this.shouldEdit =  true;
    this.shouldAdd = false;
    this.selectedKeyPoint = kp;
    // window.scrollTo(0, document.body.scrollHeight);
    //this.shouldRenderTourForm=true;
  }

  onMapClick(event: { lat: number, lng: number }) {
    // this.triggerMap = true;
    // Postavi koordinate u formu
    this.latitude = event.lat;
    this.longitude = event.lng;
    // this.keyPointForm.setCoordinates(event.lat, event.lng);
    // window.scrollTo(0, document.body.scrollHeight);
    this.clicked = true;
    // this.cdr.detectChanges();
    console.log(this.clicked);
  }

  onTourLengthReceived(length:number){
      this.tourLengthValue = length;
      //this.updateLength(length);
  }

  updateLength(lengthTour:number){
    this.tour.length=lengthTour;
  }
  onTourDurationRecived(duration: number) {
    var time = 1;
    if(duration >  60){
      time = 2;
      duration = duration / 60;
    }
    let tourDuration = {
      duration: duration,
      transportType: 0,
      timeUnit: time
    }
    this.tour.tourDurations = [];
    this.tour.tourDurations.push(tourDuration);
  }

  closeModal() {
    this.shouldEdit = false;
    this.shouldAdd = false;
  }

  /***************** IVA *******************/
  createNewEncounter(latitude: number, longitude: number) {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id') || 0;

      this.router.navigate(['/encounters'], {
        queryParams: { latitude, longitude, id }
      });

    });
  }
  /****************************************/

}
