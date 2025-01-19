import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { TourPreview } from '../model/tour-preview';
import { MarketplaceService } from '../marketplace.service';
import { ActivatedRoute } from '@angular/router';
import { MapComponent } from 'src/app/shared/map/map.component';
import { Router } from '@angular/router';
import {OrderItem } from '../model/order-item.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { ShoppingCart } from '../model/shopping-cart.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ImagesService } from '../../tour-authoring/images.service';

@Component({
  selector: 'xp-tour-overview-details',
  templateUrl: './tour-overview-details.component.html',
  styleUrls: ['./tour-overview-details.component.css']
})
export class TourOverviewDetailsComponent implements OnInit {
  @ViewChild(MapComponent) mapComponent: MapComponent;

  tour: TourPreview;
  @Input() tourID: number;
  @Output() addedToCart: EventEmitter<null> = new EventEmitter<null>();
  user: User;
  tourAvarageRating: number = 0;
  shouldEdit: boolean = false;
  userCart: ShoppingCart;
  isTourInCart: boolean = false;
  buttonColor: string = 'orange';
  cartItemCount: number;
  location: string;
  tours: TourPreview[] = [];
  isTourOnSale: boolean = false;
  tokenExists: boolean;

  constructor(private service: MarketplaceService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private imageService: ImagesService) { }

  ngOnInit(): void {

    // this.service.cartItemCount$.subscribe(count => {
    //   this.cartItemCount = count;
    // });

    this.authService.user$.subscribe(user => {
      this.user = user;
      this.findShoppingCart();
      this.isTourPurchased();
    });
  }
  getPurchasedTour(id: number): void {
    this.service.getPurchasedTour(id).subscribe((result: TourPreview) => {
      this.tour = result;
      console.log("TURAAAAA", this.tour)
      this.imageService.getImage(this.tour.image).subscribe(blob => {
        const image = URL.createObjectURL(blob);
        this.tour.image = image;
      });
    });
  }
  getPublishedTourPreview(id: number): void{
    this.service.getPublishedTourPreview(id).subscribe((result: TourPreview) => {
      this.tour = result;
      console.log("TURAAAAA", this.tour)
      this.imageService.getImage(this.tour.image).subscribe(blob => {
        const image = URL.createObjectURL(blob);
        this.tour.image = image;
      });
    });
  }

  onBack(): void {
    this.router.navigate([`tour-overview`]);
  }

  onAddToCart(t: TourPreview): void {
      const orderItem: OrderItem = {
        itemId: t.id || 0,
        name: t.name,
        price: t.cost,
        type: 0
      };
      this.addItemToCart(orderItem);
      this.router.navigate([`tour-overview`]);
     
      this.addedToCart.emit();
  }

  addItemToCart(orderItem: OrderItem): void {
    console.log(orderItem)
    this.service.addItemToShoppingCart(orderItem,this.user.id).subscribe((cart) => {
      this.cartItemCount = cart.items.length;
      this.service.addItem(this.user.username);
      this.userCart = cart;
      this.isTourInCart = this.checkIsTourInCart();
      if (this.isTourInCart == true) {
        this.buttonColor = 'gray';
      }
    });
  }


  checkIsTourInCart(): boolean {
    if (this.userCart.items.length > 0) {
      return this.userCart.items.some(item => item.itemId == this.tourID);
    }
    return false;
  }


  findShoppingCart(): void {
    this.service.getShoppingCart(this.user.id).subscribe((result) => {
      if (!result) {
        this.isTourInCart = false;
        this.buttonColor = 'orange';
      } else {
        this.userCart = result;
        this.isTourInCart = this.checkIsTourInCart();
        if (this.isTourInCart == true) {
          this.buttonColor = 'gray';
        }
      }
    });
  }

  isTourPurchased(): boolean{
    console.log("OVO MI JE ID TURE: ", this.tourID);
    // this.service.tokenExists(this.tourID, this.user.id).subscribe((result) => {
    //   this.tokenExists = result;
    // })

    // return this.tokenExists;

    this.service.tokenExists(this.tourID, this.user.id).pipe(
      catchError((error) => {
        this.tokenExists = false;
        console.log("TOKEEEN", this.tokenExists);
        this.getPublishedTourPreview(this.tourID);
        return of(false);
      })
    ).subscribe((result) => {
      this.tokenExists = result;
      console.log("TOKEEEN", this.tokenExists);
      this.getPurchasedTour(this.tourID);
    });

    return this.tokenExists;
  }
}
