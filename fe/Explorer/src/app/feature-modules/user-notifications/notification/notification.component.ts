import { Component, OnInit } from '@angular/core';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Notification } from '../model/notification.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { UserNotificationsService } from '../user-notifications.service';
import { AdministrationService } from '../../administration/administration.service';
import { TourService } from '../../tour-authoring/tour.service';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { Router } from '@angular/router';

@Component({
  selector: 'xp-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css'],
})
export class NotificationComponent implements OnInit {
  notification: Notification[] = [];
  userId: number;
  loggedInUser: User;
  tourName: string[] = [];
  totalNotifications: number = 0;
  adventureCoins: number = 0; // Čuva broj AC za ulogovanog korisnika

  constructor(
    private service: UserNotificationsService,
    private authService: AuthService,
    private adminService: AdministrationService,
    private tourService: TourService,
    private marketplaceService: MarketplaceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
      this.userId = user.id;
      this.getAdventureCoins(user.id); // Učitavanje AC   
      console.log("LOOOOOOGGGGGGEEEEDDDD", this.loggedInUser.id);

      this.getNotification(this.loggedInUser.id);
    });
  }

  getAdventureCoins(userId: number): void {
    this.marketplaceService.getAdventureCoins(userId).subscribe({
      next: (result) => {
        if (result && result.adventureCoins !== undefined) {
          this.adventureCoins = result.adventureCoins;
        } else {
          console.warn(`No wallet found for user ID: ${userId}.`);
          this.adventureCoins = 0; // Postavi na 0 ako novčanik ne postoji
        }
      },
      error: (err) => {
        if (err.status === 404) {
          console.warn(`Wallet not found for user ID: ${userId}. Skipping...`);
        } else {
          console.error('Error fetching adventure coins:', err);
        }
        this.adventureCoins = 0; // Osiguraj da je vrednost 0 u slučaju greške
      },
    });
  }

  getNotification(userId: number): void {
    this.service.getNotification(userId).subscribe({
      next: (result: PagedResults<Notification>) => {
        this.notification = result.results;
        this.totalNotifications = this.notification.length;

        this.notification.forEach((notification) => {
          const reportId = notification.reportId;
          if (reportId) {
            this.fetchReportById(reportId);
          }
        });
      },
      error: (err: any) => {
        console.log('Error fetching notifications:', err);
      },
    });
  }

  fetchReportById(reportId: number): void {
    this.adminService.getReportedTourProblemById(reportId).subscribe({
      next: (report) => {
        if (report && report.tourId) {
          this.fetchTourById(report.tourId);
        }
      },
      error: (err) => {
        console.log(`Error fetching report for reportId ${reportId}:`, err);
      },
    });
  }

  fetchTourById(tourId: string): void {
    this.tourService.getByid(Number(tourId)).subscribe({
      next: (tour) => {
        if (tour && tour.name) {
          this.tourName.push(tour.name);
        }
      },
      error: (err) => {
        console.log(`Error fetching tour for tourId ${tourId}:`, err);
      },
    });
  }

  navigateToReport(notification: Notification): void {
    this.router.navigate(['reported-tour-problems']);
    this.service.notificationIsRead(notification).subscribe({
      next: () => {
        console.log('Notification marked as read');
        this.getNotification(this.loggedInUser.id);
      },
      error: (error) => {
        console.error('Error marking notification as read:', error);
      },
    });
}
navigateToChat(notification: Notification): void {
  console.log("evo vodim te sad");
  this.router.navigate([`/profile-chat/${notification.senderId}`]);
  this.service.notificationIsRead(notification)
  .subscribe({
    next: () => {
      console.log('Notification marked as read');
      this.getNotification(this.loggedInUser.id);
    },
    error: (error) => {
      console.error('Error marking notification as read:', error);
    }
  });
}

}
