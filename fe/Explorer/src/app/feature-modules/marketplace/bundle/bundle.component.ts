import { Component, OnInit } from '@angular/core';
import { Bundle } from '../model/bundle.model';
import { MarketplaceService } from '../marketplace.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Router } from '@angular/router';
import { TourPreview } from '../model/tour-preview';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { OrderItem } from '../model/order-item.model';
import { ShoppingCart } from '../model/shopping-cart.model';
import { Product } from '../model/product.model';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'xp-bundle',
  templateUrl: './bundle.component.html',
  styleUrls: ['./bundle.component.css']
})
export class BundleComponent implements OnInit {
  bundles: Bundle[] = []
  tours: Map<number, TourPreview> = new Map();
  loggedInUser:User;
  cartItemCount: number;
  userCart: ShoppingCart;
  purchasedTours: number[] = [];
  constructor(private service: MarketplaceService, private router: Router, private authService: AuthService,private snackBar: MatSnackBar){}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
      if(this.loggedInUser.role === 'author')
        this.getBundleForAuthor();
      else
        this.getBundleForUser();
      this.getPurchasedTours();
    });
  }

  getBundleForAuthor(){
    this.service.getBundlesForAuthor().subscribe({
      next: (res: PagedResults<Bundle>) =>{
        this.bundles = res.results;
        this.getTours();
      }
    });
  }

  getBundleForUser(){
    this.service.getBundlesForUsers().subscribe({
      next: (res: PagedResults<Bundle>) => {
        this.bundles = res.results;
        this.getTours();
      }
    })
  }

  deleteBundle(bundle: Bundle) {
    const snackBarRef = this.snackBar.open('Are you sure you want to delete this bundle?', 'Confirm', {
      duration: 5000, 
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'info',
    });
  
    snackBarRef.onAction().subscribe(() => {
      this.service.deleteBundle(bundle.id).subscribe({
        next: () =>{
          this.bundles = this.bundles.filter(b => b.id !== bundle.id);
          this.snackBar.open('Bundle deleted successfully!', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            panelClass: 'info',
          });
        }
      })
    });
  }
  onEditClicked(bundle: Bundle) {
    this.router.navigate([`create-bundle/${bundle.id}`]);
  }
  addBundle(){
    this.router.navigate(["create-bundle/0"]);
  }

  displayStatus(status: number): string{
    if(status === 0)
      return "Draft";
    else if(status === 1)
      return "Published";
    else
      return "Archived";
  }
  getTours(){
    if(this.loggedInUser.role === 'author'){
      this.service.getToursForAuthor().subscribe({
        next: (res: PagedResults<TourPreview>) => {
          for(let t of res.results)
            this.tours.set(t.id, t)
        }
      })
    }
    else{
      this.service.getPublishedTours().subscribe({
        next: (res: PagedResults<TourPreview>) => {
          for(let t of res.results)
            this.tours.set(t.id, t)
        }
      })
    }
  }

  publishBundle(bundle: Bundle){
    this.service.publishBundle(bundle).subscribe({
      next: (res: Bundle) => {
        let id = this.bundles.indexOf(bundle);
        this.bundles[id] = res;
        this.service.createBundleItem(bundle).subscribe({ next: () => {}})
        this.snackBar.open('Your bundle has been published!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'info',
        });
      },
      error: (err) =>{
       //alert("If you want to publish bundle, number of published tours in bundle have to be 2 or more.");
        this.snackBar.open('If you want to publish bundle, number of published tours in bundle have to be 2 or more.', 'Close', {
          duration: 7000,
          verticalPosition: 'top',
          panelClass: 'info',
        });
      }
    })
  }

  archiveBundle(bundle: Bundle){
    this.service.archiveBundle(bundle).subscribe({
      next: (res: Bundle) => {
        let id = this.bundles.indexOf(bundle);
        this.bundles[id] = res;
        this.snackBar.open('Your bundle has been archived!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'info',
        });
      }
    })
  }

  onAddToCart(bundle: Bundle): void {
    const orderItem: OrderItem = {
      itemId: bundle.id || 0,
      name: bundle.name,
      price: bundle.price,
      type: 1
    };
    this.addItemToCart(orderItem);
}
  addItemToCart(orderItem: OrderItem): void {
    console.log(orderItem)
    this.service.addItemToShoppingCart(orderItem,this.loggedInUser.id).subscribe((cart) => {
      this.cartItemCount = cart.items.length;
      this.service.addItem(this.loggedInUser.username);
      this.userCart = cart;
    });
  }

  getPurchasedTours(): void {
    this.service.getPurchasedTours(this.loggedInUser.id).subscribe({
      next: (tours: any[]) => { 
        this.purchasedTours = tours.map(tour => tour.itemId); 
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  isBundlePurchased(products: Product[]): boolean {
    let countOfPurchesTour = 0
    for(let p of products){
      if(this.purchasedTours.includes(p.tourId))
        countOfPurchesTour++;
    }
    return countOfPurchesTour >=2;
  }

  checkIsBundleInCart(id: number): boolean {
    if (this.userCart !== undefined && this.userCart.items.length > 0) {
      return this.userCart.items!.some(item => item.itemId === id);
    }
    return false;
  }
}
