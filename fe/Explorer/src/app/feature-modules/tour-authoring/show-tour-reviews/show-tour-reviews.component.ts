import { Component, OnInit } from '@angular/core';
import { TourReview } from '../../tour-execution/model/tour-review.model';
import { showTourReviewsService } from '../show-tour-reviews-service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { ActivatedRoute } from '@angular/router';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Login } from 'src/app/infrastructure/auth/model/login.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'xp-show-tour-reviews',
  templateUrl: './show-tour-reviews.component.html',
  styleUrls: ['./show-tour-reviews.component.css']
})
export class ShowTourReviewsComponent implements OnInit {

  reviews: TourReview[] = [];
  tourId: number | undefined;
  usernameCache = new Map<number, string>();
  averageGrade: number | undefined;

  constructor(private showTourReviewsService: showTourReviewsService, private authService: AuthService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.tourId = id ? +id : undefined;
      this.getReviews();
      this.getAverageGrade();
    });
  }

  getReviews(): void {
    this.showTourReviewsService.getReviewsByTourId(this.tourId).subscribe({
      next: (result: PagedResults<TourReview>) => {
        console.log('Vidi ovo:', result);
        this.reviews = result.results;

        this.reviews.forEach(review => {
          if (review.touristId && !this.usernameCache.has(review.touristId)) {
            console.log('Ovo je bas lepo: ', review);
            this.authService.getUsername(review.touristId).subscribe({
              next: (login: Login) => {
                this.usernameCache.set(review.touristId, login.username);  // Keširanje korisničkog imena
                console.log('Koji je username: ', login.username);
              },
              error: err => console.log(err)
            });
          }
        });
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  getUsername(userId: number | undefined): string {
    return userId && this.usernameCache.has(userId) ? this.usernameCache.get(userId)! : 'Unknown';
  }

  getAverageGrade(): void {
    this.showTourReviewsService.getAverageGrade(this.tourId).subscribe({
      next: (grade: number) => {
        this.averageGrade = grade; // Dodeljivanje stvarne vrednosti
      },
      error: (err: any) => {
        console.log('Greška pri dobijanju prosečne ocene', err);
      }
    });
  }
  
}
