import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { PaymentRecord } from '../model/payment-record.model';
import { MarketplaceService } from '../marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Subscription } from 'rxjs';
import { OrderItem } from '../model/order-item.model';

@Component({
  selector: 'xp-receipt',
  templateUrl: './receipt.component.html',
  styleUrls: ['./receipt.component.css']
})
export class ReceiptComponent implements OnInit, OnDestroy {
  @Input() totalPrice: number = 0;
  @Input() items: OrderItem[] = [];
  paymentRecords: PaymentRecord[] = [];
  // totalPrice: number = 0;
  user: User;
  routerSubscription: Subscription;

  constructor(
    private router: Router,
    private marketplaceService: MarketplaceService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Dohvati trenutnog korisnika preko AuthService
    this.authService.user$.subscribe((user) => {
      if (user && user.id) {
        this.user = user;
        console.log('Logged-in user ID:', this.user.id);
        this.getPaymentRecords(this.user.id);

        // Praćenje promene rute
        this.routerSubscription = this.router.events.subscribe((event) => {
          if (event instanceof NavigationStart) {
            console.log('Route change detected, clearing payment records...');
            // this.clearPaymentRecords(this.user.id);
          }
        });
      } else {
        console.error('No user found. Redirecting to home.');
        this.router.navigate(['/']);
      }
    });
  }

  getPaymentRecords(touristId: number): void {
    this.marketplaceService.getPaymentRecords(touristId).subscribe({
      next: (records: PaymentRecord[]) => {
        console.log('Payment records fetched:', records);
        this.paymentRecords = records.filter(record => 
          this.items.some(item => item.itemId === record.itemId)
        );
        this.calculateTotalPrice();
      },
      error: (err) => {
        console.error('Error fetching payment records:', err);
        alert('Failed to load payment records.');
      }
    });
  }

  clearPaymentRecords(touristId: number): void {
    this.marketplaceService.clearPaymentRecords(touristId).subscribe({
      next: () => {
        console.log('Payment records cleared successfully.');
      },
      error: (err) => {
        console.error('Error clearing payment records:', err);
      }
    });
  }

  calculateTotalPrice(): void {
    this.totalPrice = this.paymentRecords.reduce((sum, record) => sum + record.price, 0);
    console.log('Total price calculated:', this.totalPrice);
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  getItemName(id: number): string {
    return this.items.filter(item => item.itemId === id)[0].name;
  }
}
