import { Component, OnInit } from '@angular/core';
import { TourReview } from '../model/tour-review.model';
import { TourExecutionService } from '../tour-execution.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'xp-tour-review',
  templateUrl: './tour-review.component.html',
  styleUrls: ['./tour-review.component.css']
})
export class TourReviewComponent implements OnInit {
  tourReview: TourReview[] = [];
  selectedReview: TourReview;
  shouldRenderReviewForm: boolean = false;
  shouldEdit: boolean = false;

  IdTour: number = 0;
  Percentage: number = 0;
  lastActivity: Date;

  constructor(private service: TourExecutionService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.getTourReview();

    this.route.queryParams.subscribe(params => {
      if (params['tourId'] && params['completedPercentage'] && params['lastActivity']) {
        // Capture individual query parameters
        this.IdTour = Number(params['tourId']);
        this.Percentage = Number(params['completedPercentage']);

        this.lastActivity = new Date(params['lastActivity']); // Ensure lastActivity is a Date object
        console.log('Captured Query Params:', this.IdTour, this.Percentage, this.lastActivity);
      }
    });
  }

  getTourReview(): void {
    this.service.getTourReview().subscribe({
      next: (result: PagedResults<TourReview>) => {
        this.tourReview = result.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  deleteReview(id: number): void {
    this.service.deleteReview(id).subscribe({
      next: () => {
        this.service.getTourReview();
      }
    });
  }

  onEditClicked(tourReview: TourReview): void {
    this.selectedReview = tourReview;
    this.shouldRenderReviewForm = true;
    this.shouldEdit = true;
  }

  onAddClicked(): void {
    this.shouldEdit = false;
    this.shouldRenderReviewForm = true;

    // Use the captured query parameters to create a new review object
    this.selectedReview = {
      tourId: this.IdTour,
      completedPercentage: this.Percentage,
      visitDate: this.lastActivity, // Assuming lastActivity is a Date
      // Fill in other fields as needed
    } as TourReview;
  }
}
