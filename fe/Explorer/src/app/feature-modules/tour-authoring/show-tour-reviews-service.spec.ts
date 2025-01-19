import { TestBed } from '@angular/core/testing';
import { showTourReviewsService } from './show-tour-reviews-service';


describe('KeyPointService', () => {
  let service: showTourReviewsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(showTourReviewsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
