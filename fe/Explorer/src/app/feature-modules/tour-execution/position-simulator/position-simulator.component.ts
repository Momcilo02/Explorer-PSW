import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { TourExecutionService } from '../tour-execution.service';
import { TourService } from '../../tour-authoring/tour.service';
import { KeyPoint } from '../../tour-authoring/model/key-point.model';
import { Object } from '../../tour-authoring/model/object.model';
import { ActivatedRoute } from '@angular/router';
import * as L from 'leaflet';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TouristLocation } from '../model/tourist-location';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { PublicPoint } from 'src/app/shared/model/public-point.model';

@Component({
  selector: 'xp-position-simulator',
  templateUrl: './position-simulator.component.html',
  styleUrls: ['./position-simulator.component.css']
})
export class PositionSimulatorComponent implements OnChanges, OnInit {

  constructor(private tourExecutionService: TourExecutionService, private route: ActivatedRoute) {}

  tour: Tour;
  map: any;
  keyPoints : KeyPoint[] = [];
  tourObjects : Object[] = [];
  private marker?: L.Marker;
  publicKeyPoints: PublicPoint[] = [];

  ngOnInit(): void {
    this.getKeyPoints();
  }

  ngOnChanges(changes: SimpleChanges): void {
    //if(changes['keyPointsMarker'] && this.map){
     // this.updateKeyPointsMarkers();
    //}
  }
  getKeyPoints():void{
    this.tourExecutionService.getPublicKeyPoints().subscribe({
      next:(result: PagedResults<KeyPoint>)=>{
        this.keyPoints = result.results;
        this.publicKeyPoints = this.keyPoints.map(kp => ({
          lat: kp.latitude,
          lng: kp.longitude,
          name: kp.name, // Assuming `name` exists in KeyPoint
          description: kp.description // Assuming `description` exists in KeyPoint
        }));
        this.getPublicTourObjects();
        },
        error:(err: any) =>{
          console.log(err)
        }
    })
  }
  getPublicTourObjects(): void {
    this.tourExecutionService.getPublicTourObjects().subscribe({
      next: (result: PagedResults<Object>) => {
        const tourObjectPoints = result.results.map(t => ({
          lat: t.latitude,
          lng: t.longitude,
          name: t.name, // Assuming `name` exists in Object
          description: t.description // Assuming `description` exists in Object
        }));
        this.publicKeyPoints = [...this.publicKeyPoints, ...tourObjectPoints]; // Dodajemo na postojeće
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

}
