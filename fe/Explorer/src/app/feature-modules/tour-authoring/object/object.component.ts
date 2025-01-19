import { Component, OnInit,Output,ViewChild } from '@angular/core';
import { Object } from '../model/object.model';
import { ObjectService } from '../object.service';
import { Router } from '@angular/router';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { ObjectFormComponent } from '../object-form/object-form.component';
import * as L from 'leaflet';
import { MapComponent } from 'src/app/shared/map/map.component';
import { ImagesService } from '../images.service';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'xp-object',
  templateUrl: './object.component.html',
  styleUrls: ['./object.component.css']
})
export class ObjectComponent implements OnInit {  
  @ViewChild('objectForm') objectForm!: ObjectFormComponent;
  @Output() isObjectComponent: boolean = false;

  objects: Object[] = [];
  selectedObject: Object;
  shouldRenderObjectsForm: boolean = false;
  shouldEdit: boolean = false;
  shouldAdd: boolean = false;
  externalPoints: L.LatLng[]=[];
  currentMarker: L.Marker | null = null;
  imageMap: Map<string, string> = new Map();
  longitude: number = 0;
  latitude: number = 0;
  

  constructor(private service: ObjectService, private router: Router, private imagesService: ImagesService,private snackBar: MatSnackBar) {}

  ngOnInit(): void{
    this.getObjects();
    this.isObjectComponent = true;
  }

  getObjects(): void {
    this.service.getObjects().subscribe({
      next: (result: PagedResults<Object>) => {
        this.objects = result.results;
        this.externalPoints = this.objects.map( obj=> L.latLng(obj.latitude,obj.longitude));
        for (let ob of this.objects) {
          this.getImage(ob.image);
        }
        // window.scrollTo(0, 0);
        this.closeModal();
      },
      error:(err: any) =>{
        console.log(err)
      }
    })
  }

  onEditClicked(object: Object) : void {
    this.shouldRenderObjectsForm = true;
    this.selectedObject = object;
    this.shouldEdit = true;
    this.shouldAdd = false;
    // window.scrollTo(0, document.body.scrollHeight);
  }

  onAddClicked() : void {
    if(this.longitude === 0 || this.latitude === 0){
      this.snackBar.open('You need to click on map first!', 'close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'info'
      });
      return;
    }
    this.shouldRenderObjectsForm = true;
    this.shouldEdit = false;
    this.shouldAdd = true;
    // window.scrollTo(0, document.body.scrollHeight);
  }

  deleteObject(object: Object) : void {
    const snackBarRef = this.snackBar.open('Are you sure you want to delete this object?', 'Confirm', {
      duration: 5000, 
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'info',
    });
  
    snackBarRef.onAction().subscribe(() => {
    this.service.deleteObject(object).subscribe({
      next: (_) => {
        window.location.reload();
        this.snackBar.open('Deleted object!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'info',
          
        });
      }
    })
  });
  }

  getCategory(id: number) : string {
    if(id === 0)
      return "WC";
    else if(id === 1)
      return "Restaurant";
    else if(id === 2)
      return "Parking";
    else
      return "Other";
  }

  getImage(path: string): void {
    console.log('Ovo je path: ', path);
    this.imagesService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }

  onMapClick(event: { lat: number, lng: number }) {
    this.latitude = event.lat;
    this.longitude = event.lng;
  }
 
  closeModal() {
    this.shouldAdd = false;
    this.shouldEdit = false;
  }
}
