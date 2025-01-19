import { Component, OnInit } from '@angular/core';
import { AdministrationService } from '../administration.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { TouristWallet } from 'src/app/feature-modules/marketplace/model/tourist-wallet.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Observable, map, of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
export enum Role {
  Administrator = 0,
  Author = 1,
  Tourist = 2
}

@Component({
  selector: 'xp-accounts-management',
  templateUrl: './accounts-management.component.html',
  styleUrls: ['./accounts-management.component.css']
})
export class AccountsManagementComponent implements OnInit {
  bindingAccounts: {
    id: number;
    userId: number;
    username: string;
    role: Role;
    email: string;
    isActive: boolean;
    adventureCoins: number;
    paymentCoins: number;
  }[] = [];
  allUsers: User[] = [];
  loggedInUserId: number;

  constructor(
    private service: AdministrationService,
    private authService: AuthService,
    private marketplaceService: MarketplaceService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUserId = user.id;
      this.getAllUsers();
    });
  }

  getAllUsers(): void {
    this.service.getAllUserss().subscribe({
      next: (users: PagedResults<User>) => {
        this.allUsers = users.results; // Preuzimanje korisnika iz results
        console.log('Fetched users:', this.allUsers);
        this.fillBindingList(); // Popunjavanje bindingAccounts
      },
      error: (err) => {
        console.error('Error fetching users:', err);
      }
    });
  }

  fillBindingList(): void {
    if (!this.allUsers || this.allUsers.length === 0) {
      console.error('No users found.');
      return;
    }

    this.allUsers.forEach((user) => {
      const account = {
        id: user.id,
        userId: user.id, // User's ID
        username: user.username, // Extract username from the User model
        role: this.mapRole(user.role), // Map role to Role enum
        email: user.email || `${user.username}@example.com`, // Use email from the user model or generate a placeholder
        isActive: true, // Default assumption: all users are active
        adventureCoins: 0, // AdventureCoins to be fetched
        paymentCoins: 1, // Default payment value
      };

      // Fetch AdventureCoins if the user is a Tourist
      if (this.isTourist(account.role)) {
        this.getAdventureCoins(account.userId, account.role).subscribe({
          next: (adventureCoins: number) => {
            account.adventureCoins = adventureCoins;
          },
          error: (err) => {
            console.error(`Error fetching AdventureCoins for user ${account.userId}:`, err);
          },
        });
      }

      this.bindingAccounts.push(account);
    });

    console.log('Binding accounts:', this.bindingAccounts);
  }


  getAdventureCoins(id: number, role: Role): Observable<number> {
    if (this.isTourist(role)) {
      return this.marketplaceService.getAdventureCoins(id).pipe(
        map((result: TouristWallet) => result.adventureCoins)
      );
    } else {
      return of(0);
    }
  }

  pay(account: {
    id: number;
    userId: number;
    username: string;
    role: Role;
    email: string;
    isActive: boolean;
    adventureCoins: number;
    paymentCoins: number;
  }): void {
    if(account.paymentCoins <= 0){
      this.snackBar.open(`The AC you pay must be positive!`, 'Close', {
        duration: 4000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
      return;
    }
    this.marketplaceService.paymentAdventureCoins(account.userId, account.paymentCoins).subscribe({
      next: (result: TouristWallet) => {
        account.adventureCoins = result.adventureCoins; // Ažuriranje AdventureCoins nakon uplate
        console.log(`Payment successful for user ${account.userId}:`, result);
        this.snackBar.open(`Payment successful for ${account.email} in the amount of ${account.paymentCoins}`, 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      },
      error: (err) => {
        console.error(`Error making payment for user ${account.userId}:`, err);
      }
    });
  }

  mapRole(role: string | undefined): Role {
    if (!role) {
      console.warn('Undefined role detected, setting default to Tourist.');
      return Role.Tourist; // Default role
    }

    // Normalize and map role string to Role enum
    switch (role.toLowerCase()) {
      case 'administrator':
        return Role.Administrator;
      case 'author':
        return Role.Author;
      case 'tourist':
        return Role.Tourist;
      default:
        console.warn(`Unknown role "${role}", setting default to Tourist.`);
        return Role.Tourist; // Default role for unknown cases
    }
  }



  isTourist(role: Role): boolean {
    return role === Role.Tourist;
  }
}
