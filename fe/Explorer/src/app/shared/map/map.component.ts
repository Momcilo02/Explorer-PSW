import { Component, AfterViewInit, Output, EventEmitter, Input, SimpleChanges, OnChanges, OnDestroy } from '@angular/core';
import * as L from 'leaflet';
import { MapService } from './map.service';
import {PublicPoint} from 'src/app/shared/model/public-point.model'
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'xp-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements AfterViewInit, OnChanges, OnDestroy {
  @Input() keyPointsMarker: L.LatLng[]=[];
  @Input() externalPoints: L.LatLng[] = [];
  @Input() publicPoints: PublicPoint[] = [];
  @Input() touristPosition: L.LatLng;
  @Input() isObjectComponent: boolean = false;
  @Input() isMovingTourist: boolean = true;
  @Input() disableMapClick: boolean = false;
  @Output() mapClick = new EventEmitter<{ lat: number, lng: number }>();
  @Output() tourLength = new EventEmitter<number>();
  @Output() tourDuration = new EventEmitter<number>();

  @Input() encounters: { lat: number; lng: number; name: string; encounterType: number }[] = [];
  private encountersLayerGroup = L.layerGroup();

  @Input() update!: boolean;
  private userMarkers = L.layerGroup();
  map_clicked = false;

  private map: any;
  private singleMarker?: L.Marker;
  private keyPointsLayerGroup = L.layerGroup(); 
  private publicPointsLayerGroup = L.layerGroup();
  private MAPBOX_URL_KEY: string = "pk.eyJ1IjoibW9tY2lsbzAyIiwiYSI6ImNtMmVxMDJndzAweDgyanNpYmpiMXc3dGMifQ.e2efHMHGaA8JJ65_CvKrcQ";
  private waypoints: L.LatLng[] = [];
  private routeControl: any;
  private touristMarker?: L.Marker;
  mapProfile: string = 'mapbox/walking';

  @Input() showSearch: boolean = true;

  constructor(private mapService: MapService,private snackBar: MatSnackBar) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['externalPoints'] && this.map) {
      console.log("Menjam waypointe")
      this.isMovingTourist=false;
      this.addMarkersAndSetRoute();
      this.updateMarkersAndRoute();
    }
    if(changes['touristPosition'] && this.map){
      this.updateTouristMarker();
    }
    if(changes['keyPointsMarker'] && this.map){
      this.updateKeyPointsMarkers();
    }
    if (changes['encounters'] && this.map) {
      this.addEncounterMarkers();
      this.updateEncounterMarkers(); 
    }
    if(changes['publicPoints'] && this.map){
      this.updatePublicPoints();
      //this.addMarkersAndSetRoute();
      //this.updateMarkersAndRoute();
    }
    if(changes['update']  && this.map){
      //this.updateKeyPointsMarkers();
      console.log("Reloudujem mapu")
      this.map.remove();
      this.initMap();
    }
  }
  ngOnDestroy(): void {
    console.log('MapComponent destroyed');
    if (this.map) {
      this.map.remove(); // Properly removes the map instance
    }
  }

  private initMap(): void {
    console.log(this.map)
    if (this.map) return; // Provera da li je mapa već inicijalizovana
    
    this.map = L.map('map', {
      center: [54.5260, 15.2551],
      zoom: 4,
    });

    const tiles = L.tileLayer(
      'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
      {
        maxZoom: 18,
        minZoom: 3,
        attribution:
          '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      }
    );
    tiles.addTo(this.map);
    this.keyPointsLayerGroup.addTo(this.map);
    this.encountersLayerGroup.addTo(this.map); 
    this.publicPointsLayerGroup.addTo(this.map);
    this.registerOnClick();

    

    if (this.externalPoints.length > 0) {
      this.externalPoints.forEach(point => {
        L.marker([point.lat, point.lng]).addTo(this.map);
      });
      this.waypoints = [...this.externalPoints];
      this.updateMarkersAndRoute();
    }
    console.log('MapComponent init');
    console.log(this.map)
    
    
  }

  private addMarkersAndSetRoute(): void {
    // Dodaj sve tačke na mapu
    this.externalPoints.forEach(point => {
      L.marker([point.lat, point.lng]).addTo(this.map);
    });
    this.waypoints = [...this.externalPoints];
    this.setRoute();
  }

  private updateMarkersAndRoute(): void {
    // Ukloni prethodne markere, ako je potrebno
    if (this.routeControl) {
      this.routeControl.remove();
    }

    // Postavi nove markere na osnovu `externalPoints`
    this.externalPoints.forEach(point => {
      L.marker([point.lat, point.lng]).addTo(this.map);
    });

    // Postavi rutu samo ako `isObjectComponent` nije aktiviran
    if (!this.isObjectComponent) {
      this.waypoints = [...this.externalPoints];
      
      this.setRoute();
    }
  }

  public addKeyPoint(kp: L.LatLng): void{
    this.keyPointsMarker.push(kp);
  }
  registerOnClick(): void {
    // Osigurava da nema prethodnih `click` eventova pre nego što registruje novi
    if (this.disableMapClick) return;
    this.map.off('click');
    this.map.on('click', (e: L.LeafletMouseEvent) => {
      const { lat, lng } = e.latlng;
      this.mapService.reverseSearch(lat,lng).subscribe((res)=>{});
      
      if(this.map_clicked){//
        this.userMarkers.remove()
        this.userMarkers.clearLayers()
        //this.routeControl.remove()
        //this.routeControl.clearLayers()
      }
      this.userMarkers.addTo(this.map);
      //this.routeControl.addTo(this.map);
      if (this.singleMarker) {
        this.singleMarker.remove();
      }
      if(this.map_clicked){
        this.waypoints.pop();
      }
      this.userMarkers.addLayer(new L.Marker([lat, lng])) //
      this.map_clicked=true//
      

      //this.singleMarker = L.marker([lat, lng]).addTo(this.map);
      this.mapClick.emit({ lat, lng });
      console.log(this.waypoints);
      
      
      console.log(this.waypoints);
      //this.waypoints[this.waypoints.length-1] = L.latLng(lat, lng);
      this.waypoints.push(L.latLng(lat, lng));
      console.log(this.waypoints);
      if(!this.isMovingTourist){
              this.setRoute();
      }
    });
  }

  search(): void {
    this.mapService.search('Strazilovska 19, Novi Sad').subscribe({
      next: (result) => {
        L.marker([result[0].lat, result[0].lon])
          .addTo(this.map)
          .bindPopup('Pozdrav iz Strazilovske 19.')
          .openPopup();
      },
      error: () => {},
    });
  }

  setRoute(): void {
    if (this.isObjectComponent) return;
    if(this.routeControl){
      this.routeControl.remove()
    }
    
    this.routeControl = L.Routing.control({
      waypoints: this.waypoints,
      router: L.routing.mapbox(this.MAPBOX_URL_KEY, { profile: this.mapProfile })
    }).addTo(this.map);

    this.routeControl.on('routesfound', (e: any) => {
      const { totalDistance, totalTime } = e.routes[0].summary;
      this.tourLength.emit(totalDistance/1000);
      this.tourDuration.emit(Math.round(totalTime / 60));
      //alert(`Total distance is ${totalDistance / 1000} km and total time is ${Math.round(totalTime % 3600 / 60)} minutes`);
    });
  }

  // ngAfterViewInit(): void {
  //   const DefaultIcon = L.icon({
  //     iconUrl: 'https://unpkg.com/leaflet@1.6.0/dist/images/marker-icon.png',
  //   });

  //   L.Marker.prototype.options.icon = DefaultIcon;
    
  //   this.initMap(); // Poziva inicijalizaciju mape samo jednom
    
    
  // }

  ngAfterViewInit(): void {
    setTimeout(() => {
      const DefaultIcon = L.icon({
        iconUrl: 'https://unpkg.com/leaflet@1.6.0/dist/images/marker-icon.png',
      });
  
      L.Marker.prototype.options.icon = DefaultIcon;
  
      if (document.getElementById('map')) {
        this.initMap(); // Initialize map only if container exists
      } else {
        console.error('Map container not found');
      }
    }, 0);
  }
  
  updateTouristMarker(): void {
    // Remove existing marker if it exists
    if (this.touristMarker) {
      this.map.removeLayer(this.touristMarker);
    }

    // Add new marker for tourist position
    this.touristMarker = L.marker([this.touristPosition.lat, this.touristPosition.lng]).addTo(this.map);
    //this.map.setView([this.touristPosition.lat, this.touristPosition.lng], this.map.getZoom());
  }
  
  private updateKeyPointsMarkers(): void {
    // Clear existing markers in keyPointsLayerGroup
    this.keyPointsLayerGroup.clearLayers();

    // Add new markers based on keyPointsMarker input
    this.keyPointsMarker.forEach(point => {
      L.marker([point.lat, point.lng]).addTo(this.keyPointsLayerGroup);
    });

    if (!this.isObjectComponent) {
      this.waypoints = [...this.keyPointsMarker];
      this.setKeyPointsRoute();
    }
  }
  setKeyPointsRoute(): void {
    if (this.isObjectComponent) return;

    if (this.routeControl) {
      this.routeControl.remove();
    }

    this.routeControl = L.Routing.control({
      waypoints: this.waypoints,
      router: L.routing.mapbox(this.MAPBOX_URL_KEY, { profile: this.mapProfile }),
    }).addTo(this.map);
  }

  private getCustomIcon(color: string): L.Icon {
    // Ensure the color is URL-encoded, which will handle the hex value correctly
    const encodedColor = encodeURIComponent(color);
  
    return L.icon({
      iconUrl: `data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 50'%3E%3Cpath d='M15 0C8.25 0 3 6.5 3 12c0 5.5 8 21 12 25.5 4-4.5 12-20 12-25.5 0-5.5-5.25-12-12-12z' fill='${encodedColor}'/%3E%3Ccircle cx='15' cy='12' r='4' fill='%23fff' /%3E%3C/svg%3E`,
      iconSize: [50, 75],
      iconAnchor: [15, 50],
      popupAnchor: [0, -40],
      shadowUrl: 'https://unpkg.com/leaflet@1.6.0/dist/images/marker-shadow.png',
      shadowSize: [75, 60],
    });
  }
  
  private updateEncounterMarkers(): void {
    this.encountersLayerGroup.clearLayers();
  
    this.encounters.forEach(encounter => {
      // Primer: Biranje boje na osnovu `name` ili drugog atributa
      const color = this.getColorForEncounter(encounter.encounterType); // Dodaj logiku za izbor boje
      const markerIcon = this.getCustomIcon(color);
  
      L.marker([encounter.lat, encounter.lng], { icon: markerIcon })
        .bindPopup(`<b>${encounter.name}</b>`)
        .addTo(this.encountersLayerGroup);
    });
  }
  
  private addEncounterMarkers(): void {
    this.encounters.forEach(encounter => {
      const color = this.getColorForEncounter(encounter.encounterType); // Dodaj logiku za izbor boje
      const markerIcon = this.getCustomIcon(color);
  
      L.marker([encounter.lat, encounter.lng], { icon: markerIcon })
        .addTo(this.map)
        .bindPopup(`<b>${encounter.name}</b>`);
    });
  }
  private updatePublicPointss(): void {
    this.publicPoints.forEach(point => {
      const marker = L.marker([point.lat, point.lng]).addTo(this.map);
      
      marker.bindPopup(`<b>${point.name}</b><br>${point.description}</b><br>Latitude: ${point.lat}, Longitude: ${point.lng}`);
    });
    //this.setRoute();
  }
  private updatePublicPoints(): void {
    this.publicPointsLayerGroup.clearLayers();


    this.publicPoints.forEach(point => {
      const marker = L.marker([point.lat, point.lng])
        .bindPopup(`<b>${point.name}</b><br>${point.description}</b><br>Latitude: ${point.lat}, Longitude: ${point.lng}`);
      marker.addTo(this.publicPointsLayerGroup);
    });
  }
  private getColorForEncounter(encounterType: number): string {
    // Primer logike za dodelu boje na osnovu `name`
    switch (encounterType) {
      case 0: return '#b83fae';
      case 1: return '#027c68';
      case 2: return '#76C903';
      default: return 'gray';
    }
  }

  searchLocation(query: string): void {
    if (!query.trim()) {
      //alert('Please enter a location to search.');
      this.snackBar.open('Please enter a location to search.', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'info',
        
      });
      return;
    }

    this.mapService.search(query).subscribe({
      next: (results) => {
        if (results && results.length > 0) {
          const { lat, lon, display_name } = results[0];
          this.map.setView([lat, lon], 10);
          const marker = L.marker([lat, lon]).addTo(this.map);
          marker.bindPopup(`<b>${display_name}</b>`).openPopup();
        } else {
          //alert('Location not found. Try a different query.');
          this.snackBar.open('Location not found. Try a different query.', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            panelClass: 'info',
            
          });
        }
      },
      error: () => {
        //alert('Error searching for the location. Please try again.');
        this.snackBar.open('Error searching for the location. Please try again.', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
          
        });
      }
    });
  }

  refreshMap(): void {
    if (this.map) {
      this.map.invalidateSize(); // Leaflet metoda za osvežavanje dimenzija
    }
  }
}
