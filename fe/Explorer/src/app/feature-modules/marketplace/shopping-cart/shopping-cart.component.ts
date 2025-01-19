import { Component, OnInit } from '@angular/core';
import { MarketplaceService } from '../marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Router } from '@angular/router';
import { OrderItem } from '../model/order-item.model';
import { ShoppingCart } from '../model/shopping-cart.model';
import { TouristWallet } from '../model/tourist-wallet.model';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})

@Component({
  selector: 'xp-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {
  cart: ShoppingCart;
  user: User;
  orderItems: OrderItem[] = [];
  cartItemCount: number;
  adventureCoins: number = 0; // Dodata promenljiva za AC
  showReceipt: boolean;
  items: OrderItem[] = []

  constructor(
    private service: MarketplaceService,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.loadShoppingCart();
      this.getAdventureCoins(); // Učitavanje AC za korisnika
    });
  }

  loadShoppingCart(): void {
    this.service.getShoppingCart(this.user.id).subscribe({
      next: (result: ShoppingCart) => {
        this.cart = result;
        this.orderItems = this.cart.items;
      },
      error: (error) => {
        console.log('Error occurred while fetching shopping cart: ' + error);
      }
    });
  }

  getAdventureCoins(): void {
    this.service.getAdventureCoins(this.user.id).subscribe({
      next: (wallet: TouristWallet) => {
        this.adventureCoins = wallet.adventureCoins;
      },
      error: (err) => {
        console.error('Error fetching adventure coins:', err);
        this.adventureCoins = 0; // Ako ne postoji novčanik, postavi AC na 0
      }
    });
  }

  calculateTotalPrice(): number {
    return this.calculateSubtotalPrice();
  }

  calculateSubtotalPrice(): number {
    return this.orderItems.reduce((total, item) => total + item.price, 0);
  }

  checkout() {
  if (this.user && this.user.id !== undefined) {
    const totalPrice = this.calculateTotalPrice();

    // Check if user has enough Adventure Coins
    if (this.adventureCoins < totalPrice) {
      // alert("You do not have enough Adventure Coins to complete the checkout.");
      this.snackBar.open('You do not have enough Adventure Coins to complete the checkout.', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
      return; // Stop further execution
    }

    this.service.shoppingCartCheckOut(this.user.id).subscribe(
      (cart) => {
        console.log('Checkout completed successfully');
        this.showReceipt = true;
        this.items = this.orderItems;
        // Resetovanje korpe
        this.orderItems = [];
        this.cart = { id: undefined, userId: this.user.id, price: 0, items: [] };
        this.service.clearCart(this.user.username);
        this.adventureCoins -= totalPrice;
      },
      (error) => {
        this.snackBar.open('An error occurred while processing your checkout. Please try again later.', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
        });
      }
    );
  } else {
    this.snackBar.open('User information is missing. Please log in and try again.', 'Close', {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: 'error',
    });
  }
}

  removeShopppingCartItem(item: OrderItem): void {
    this.service.removeItemFromShoppingCart(item, this.user.id).subscribe((cart) => {
      this.cart = cart;
      this.cart.price = cart.price;
      this.orderItems = this.cart.items;
      this.service.removeItem(this.user.username);
    });
  }

  closeModal() {
    this.showReceipt = false;
  }

  openModal(){
    this.showReceipt = true;
  }

  continueShopping(){
    this.router.navigate(["tour-overview"]);
  }
}
