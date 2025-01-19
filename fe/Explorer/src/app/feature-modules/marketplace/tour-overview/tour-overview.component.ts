import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MarketplaceService } from '../marketplace.service';
import { Router } from '@angular/router';
import { TourPreview } from '../model/tour-preview';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import * as L from 'leaflet';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MapComponent } from 'src/app/shared/map/map.component';
import { ImagesService } from '../../tour-authoring/images.service';

@Component({
  selector: 'xp-tour-overview',
  templateUrl: './tour-overview.component.html',
  styleUrls: ['./tour-overview.component.css'],
})
export class TourOverviewComponent implements OnInit {
  publishedTours: TourPreview[] = [];
  purchasedTours: number[] = [];
  user: User;
  shouldSearch: boolean = false;
  marker?: L.Marker;
  distanceForm = new FormGroup({
    distance:  new FormControl(0, [Validators.min(10), Validators.required])
  })
  isSearchOpen: boolean = false;
  isClicked: boolean = true;
  selectedTour: TourPreview | null;

  constructor(
    private service: MarketplaceService,
    private router: Router,
    private authService: AuthService,
    private imageService: ImagesService
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.user = user;
      this.getPurchasedTours(); // Učitaj kupljene ture nakon prijave korisnika
    });
    this.getPublishedTours();
  }

  getPublishedTours(): void {
    this.service.getPublishedTours().subscribe({
      next: (result: PagedResults<TourPreview>) => {
        this.publishedTours = result.results;
        for(let t of this.publishedTours){
          this.imageService.getImage(t.image).subscribe(blob => {
            const image = URL.createObjectURL(blob);
            t.image = image;
          });
        }
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  getPurchasedTours(): void {
    this.service.getPurchasedTours(this.user.id).subscribe({
      next: (tours: any[]) => { // Pretpostavka: tours je niz objekata sa itemId
        this.purchasedTours = tours.map(tour => tour.itemId); // Kreiraj niz samo sa itemId vrednostima
        console.log("Kupljeni itemId-ovi:", this.purchasedTours); // Provera
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  isTourPurchased(tourId: number): boolean {
    return this.purchasedTours.includes(tourId);
  }

  searchTours() : void{
    if(!this.marker){
      this.isClicked = false
      return
    }
    this.isClicked = true;
    if(!this.distanceForm.valid)
    {
      this.distanceForm.markAllAsTouched();
      return
    }
    console.log("lat i long:   "+this.marker?.getLatLng().lat, this.marker?.getLatLng().lng)
    this.service.searchTours(this.distanceForm.value.distance!, this.marker?.getLatLng().lat, this.marker?.getLatLng().lng).subscribe({
      next: (res) => {
        if(res)
          this.publishedTours = res;
        this.isSearchOpen = false;
        for(let t of this.publishedTours){
          this.imageService.getImage(t.image).subscribe(blob => {
            const image = URL.createObjectURL(blob);
            t.image = image;
          });
        }
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  clearSearch() : void {
    this.getPublishedTours();
    this.distanceForm.patchValue({
      distance: 0
    });
    //this.marker?.remove;
  }

  openDetails(tour: TourPreview): void {
    // this.router.navigate([`tour-overview-details/${tour.id}`]);
    this.selectedTour = tour;
  }

  onMapClick(event: { lat: number, lng: number }) {
    if (this.marker) {
      this.marker.remove();
    }
    this.marker = L.marker([event.lat, event.lng]).addTo((L as any).map('searchMap'));
  }

  toggleMap() {
    this.isSearchOpen = !this.isSearchOpen;
    this.distanceForm.patchValue({
      distance: 0
    });
  }

  closeModal() {
    this.selectedTour = null;
  }

}
