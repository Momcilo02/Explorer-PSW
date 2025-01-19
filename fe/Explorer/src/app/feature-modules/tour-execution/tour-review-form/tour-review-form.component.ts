import { Component, EventEmitter, Output, Input } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { TourReview } from '../model/tour-review.model';
import { TourExecutionService } from '../tour-execution.service';
import { subscribeOn } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'xp-tour-review-form',
  templateUrl: './tour-review-form.component.html',
  styleUrls: ['./tour-review-form.component.css']
})
export class TourReviewFormComponent {

  @Output() reviewUpdated = new EventEmitter<null>();
  @Input() tourReview: TourReview;
  @Input() shouldEdit: boolean = false;
  @Input() tourId: number; // New input
  @Input() completedPercentage: number; // New input
  @Input() visitDate: Date; // New input


  constructor(private fb: FormBuilder, private service: TourExecutionService, private route: ActivatedRoute) { }

  ngOnChanges(): void {
    this.reviewForm.reset();


    if (this.shouldEdit && this.tourReview) {
      // Patch form values, converting numeric and date values to string
      this.reviewForm.patchValue({
        tourId: this.tourId.toString(),
        rating: this.tourReview.rating.toString(),
        comment: this.tourReview.comment,
        touristId: this.tourReview.touristId.toString(),
        visitDate: this.visitDate ? this.visitDate.toString().split('T')[0] : '',
        completedPercentage: this.completedPercentage.toString(),
        images: this.tourReview.images ? this.tourReview.images.join(', ') : ''
      });
    }
  }
  

  reviewForm = new FormGroup({
    tourId: new FormControl('', [Validators.required]),
    rating: new FormControl('', [Validators.required]),
    comment: new FormControl('', [Validators.required]),
    touristId: new FormControl('', [Validators.required]),
    visitDate: new FormControl('', [Validators.required]),
    completedPercentage:  new FormControl('', [Validators.required]),
    images: new FormControl('')  // Optional, add validation if necessary
  })

  addReview(): void {
    const currentDateTime = new Date();
    
    const tourReview: TourReview = {
      tourId: Number(this.tourId) || 0,  // Convert to number
      rating: Number(this.reviewForm.value.rating) || 0,  // Convert to number
      comment: this.reviewForm.value.comment || "",
      touristId: Number(this.reviewForm.value.touristId) || 0,  // Convert to number
      visitDate: this.visitDate ? new Date(this.visitDate) : null,  // Handle null/undefined
      reviewDate: currentDateTime,
      completedPercentage: Number(this.completedPercentage) || 0,
      images: this.reviewForm.value.images ? this.reviewForm.value.images.split(',').map(img => img.trim()) : []
    }

    console.log(this.tourReview)

    this.service.addReview(tourReview).subscribe({
      next: (_) => {
        this.reviewUpdated.emit()
      }
    });
  }

    updateReview(): void {
      const currentDateTime = new Date();

      const tourReview: TourReview = {
        tourId: Number(this.tourId) || 0,  // Convert to number
        rating: Number(this.reviewForm.value.rating) || 0,  // Convert to number
        comment: this.reviewForm.value.comment || "",
        touristId: Number(this.reviewForm.value.touristId) || 1,  // Convert to number
        visitDate: this.visitDate ? new Date(this.visitDate) : null,  // Handle null/undefined
        reviewDate: currentDateTime,
        completedPercentage: Number(this.completedPercentage) || 0,
        images: this.reviewForm.value.images ? this.reviewForm.value.images.split(',').map(img => img.trim()) : []};
        tourReview.id = this.tourReview.id;
      this.service.updateEquipment(tourReview).subscribe({
        next: () => { this.reviewUpdated.emit();}
      });
    }
}
