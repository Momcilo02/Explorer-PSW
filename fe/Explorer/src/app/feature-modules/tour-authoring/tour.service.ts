import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Tour } from './model/tour.model';
import { environment } from 'src/env/environment';
import { Equipment } from '../administration/model/equipment.model';
import { KeyPoint } from './model/key-point.model';

@Injectable({
  providedIn: 'root'
})
export class TourService {

  constructor(private http: HttpClient) {}

    getTours(): Observable<PagedResults<Tour>>{
      return this.http.get<PagedResults<Tour>>('https://localhost:44333/api/administration/tour');
    }

    addTour(tour: Tour): Observable<Tour>{
      return this.http.post<Tour>(environment.apiHost +'administration/tour',tour);
    }
    setHasQuiz(id: number, hasQuiz: boolean): Observable<Tour> {
      return this.http.put<Tour>(
        'https://localhost:44333/api/tours/' + id + '/hasQuiz',
        hasQuiz, // ili { hasQuiz } ako se očekuje objekat u telu
        { headers: { 'Content-Type': 'application/json' } }
      );
    }


    updatedTour(tour: Tour): Observable<Tour>{
      console.log(tour)
      return this.http.put<Tour>(environment.apiHost + 'administration/tour/' + tour.id, tour);
    }

    updateTourLength(tour: Tour): Observable<Tour>{
      return this.http.put<Tour>(environment.apiHost + 'administration/tour/length/' + tour.id, tour);
    }

    deleteTour(tour: Tour): Observable<Tour>{
      return this.http.delete<Tour>(environment.apiHost + 'administration/tour/' + tour.id);
    }

    getEquipments(): Observable<PagedResults<Equipment>>{
      return this.http.get<PagedResults<Equipment>>(environment.apiHost + 'tours/equipment');
    }
    getTour(id: Number): Observable<Tour>{
      return this.http.get<Tour>(environment.apiHost + 'administration/tour/'+ id);
    }
    publishTour(tour: Tour): Observable<Tour>{
      return this.http.put<Tour>(environment.apiHost + 'administration/tour/publish/' + tour.id, tour);
    }
    archiveTour(tour: Tour): Observable<Tour>{
      return this.http.put<Tour>(environment.apiHost + 'administration/tour/archive/' + tour.id,tour);
    }
    reactivateTour(tour:Tour):Observable<Tour>{
      return this.http.put<Tour>(environment.apiHost + 'administration/tour/reactivate/' + tour.id,tour);
    }
    getByid(id:Number):Observable<Tour>{
      return this.http.get<Tour>(environment.apiHost + 'tours/' + id);
    }
    getMyTours():Observable<PagedResults<Tour>>{
      return this.http.get<PagedResults<Tour>>('https://localhost:44333/api/administration/tour/user');
    }
    getPublicKeyPoints(id:number):Observable<KeyPoint[]>{
      return this.http.get<KeyPoint[]>('https://localhost:44333/api/administration/tour/public/'+id);
    }
}
