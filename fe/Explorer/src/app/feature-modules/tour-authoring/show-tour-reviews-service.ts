import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { environment } from 'src/env/environment';
import { TourReview } from '../tour-execution/model/tour-review.model';

@Injectable({
    providedIn: 'root'
  })
export class showTourReviewsService {

    constructor(private http: HttpClient) {}

    getReviewsByTourId(id: number|undefined) : Observable<PagedResults<TourReview>> {
        return this.http.get<PagedResults<TourReview>>(environment.apiHost + 'tourist/tour-review/' + id);
    }

    getAverageGrade(tourId: number|undefined) : Observable<number>{
        return this.http.get<number>(environment.apiHost + 'tourist/tour-review/grade/' + tourId);
    }
}