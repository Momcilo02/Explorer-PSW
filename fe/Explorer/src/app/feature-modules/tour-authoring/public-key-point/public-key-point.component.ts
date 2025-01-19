import { Component,OnInit } from '@angular/core';
import { KeyPoint } from '../model/key-point.model';
import { Tour } from '../model/tour.model';
import { PublicPoint } from 'src/app/shared/model/public-point.model';
import { TourService } from '../tour.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ImagesService } from '../images.service';
import * as L from 'leaflet';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'xp-public-key-point',
  templateUrl: './public-key-point.component.html',
  styleUrls: ['./public-key-point.component.css']
})
export class PublicKeyPointComponent {
  publicKeyPoints: KeyPoint[] = [];
  keyPoints: KeyPoint[] = [];
  tour: Tour;
  selectedKeyPoint: KeyPoint;
  imageMap: Map<string,string> = new Map();
  markerKeyPoints: PublicPoint[] = [];
  tourLengthValue: number=0;
  tourDuration: number=0;
  externalPoints: L.LatLng[]=[];
  constructor(private tourService: TourService,private route: ActivatedRoute,private router: Router,private imagesService: ImagesService,private snackBar: MatSnackBar){}
  ngOnInit():void{
    this.route.paramMap.subscribe(params=>{
      const idParam = params.get('id');
      const id = idParam ? Number(idParam) : 0;  // Ensure id is a valid number
      if (isNaN(id)) {
        console.error('Invalid tour id');
        return;  // Exit early if id is invalid
      }
      this.getPublicKeyPoints(Number(id));
      this.tourService.getTour(Number(id)).subscribe({
        next:(res)=>{
          this.tour=res;
          this.keyPoints = this.tour.keyPoints;
          this.externalPoints = this.keyPoints.map(kp => L.latLng(kp.latitude, kp.longitude));
        }
      })
    })
  }
  getPublicKeyPoints(id:number):void{
    this.tourService.getPublicKeyPoints(Number(id)).subscribe({
      next:(result: KeyPoint[])=>{
        this.publicKeyPoints = result;
        for (let kp of this.publicKeyPoints) {
          this.getImage(kp.image);
        }
        this.markerKeyPoints = this.publicKeyPoints.map(kp => ({
          lat: kp.latitude,
          lng: kp.longitude,
          name: kp.name, // Assuming `name` exists in KeyPoint
          description: kp.description // Assuming `description` exists in KeyPoint
        }));
        },
        error:(err: any) =>{
          console.log(err)
        }
    })
  }

  add(keyPoint: any){
    const snackBarRef = this.snackBar.open('Would you like to add this public key point to your tour?', 'Confirm', {
      duration: 5000, 
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'info',
    });
  
    snackBarRef.onAction().subscribe(() => {
      this.addKeyPoint(keyPoint);
    });
  }

  addKeyPoint(keyPoint: any) {
    keyPoint.id = 0;
    keyPoint.status=0;
    this.tour.keyPoints.push(keyPoint);
    this.keyPoints = this.tour.keyPoints;
    const newLatLng = L.latLng(keyPoint.latitude, keyPoint.longitude);
    this.externalPoints = [...this.externalPoints, newLatLng];
    console.log('Updated externalPoints:', this.externalPoints);
    this.markerKeyPoints = this.markerKeyPoints.filter(
      marker => marker.lat !== keyPoint.latitude || marker.lng !== keyPoint.longitude
    );
    this.getImage(keyPoint.image);
    if(this.keyPoints.length>0)
    {
      this.updateLength(this.tourLengthValue)
    }
    this.updateDuration(this.tourDuration);
    this.updateTourLength();
    //this.updateTour();
  }
  calculateRouteLength() {
    let totalLength = 0;
    for (let i = 1; i < this.keyPoints.length; i++) {
      const point1 = L.latLng(this.keyPoints[i - 1].latitude, this.keyPoints[i - 1].longitude);
      const point2 = L.latLng(this.keyPoints[i].latitude, this.keyPoints[i].longitude);
      totalLength += point1.distanceTo(point2)/1000;  // Izračunava udaljenost između dve tačke
    }
    this.updateLength(totalLength);  // Ažurirajte dužinu ture
  }
  
  updateTour(){
    this.tourService.updatedTour(this.tour).subscribe({
      next: (res) =>{
        this.tour = res;
        this.keyPoints = res.keyPoints;
        this.getPublicKeyPoints(this.tour?.id!);
      }
    })
  }
  getImage(path: string): void {
    this.imagesService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }
  updateTourLength()
  {
    this.tourService.updateTourLength(this.tour).subscribe({
      next: (res) =>{
        this.tour = res;
        this.keyPoints = res.keyPoints;
        this.getPublicKeyPoints(this.tour?.id!);
        this.snackBar.open('Successfully added keypoint!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
        
      }
    })
  }
  onTourLengthReceived(length:number){
    this.tourLengthValue = length;
    
}
  updateLength(lengthTour:number){
    this.tour.length=lengthTour;
  }
  updateDuration(duration:number){
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
  onTourDurationRecived(duration: number) {
    this.tourDuration=duration;
  }
  // back() {
  //   this.updateLength(this.tourLengthValue);
  //   this.updateDuration(this.tourDuration);
  //   this.tourService.updateTourLength(this.tour).subscribe({
  //     next: (res) =>{
  //       this.router.navigate(['tour']);
  //     }
  //   })
  // }


}
