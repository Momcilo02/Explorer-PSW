import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { MarketplaceService } from '../../marketplace/marketplace.service';
import { ShoppingCart } from '../../marketplace/model/shopping-cart.model';
import { OrderItem } from '../../marketplace/model/order-item.model';
@Component({
  selector: 'xp-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class NavbarComponent implements OnInit {

  user: User | undefined;
  shoppingCartItems: number = 0;
  cart : ShoppingCart;
  orderItems: OrderItem[] = [];
  showDropdown: string | null = null;

  toggleDropdown(menu: string) {
    this.showDropdown = this.showDropdown === menu ? null : menu;
}


  constructor(private authService: AuthService,private marketplaceService: MarketplaceService) {}

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
    });
    this.marketplaceService.cartItemCount$.subscribe((count) => {
      this.shoppingCartItems = count;
    });
  }

  onLogout(): void {
    this.authService.logout();
  }

  closeDropdown() {
    console.log('Closing dropdown');
    this.showDropdown = null;
  }
}
