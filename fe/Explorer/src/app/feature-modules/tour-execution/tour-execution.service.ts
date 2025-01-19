import { TouristEquipment } from './model/tourist-equipment.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TourReview } from './model/tour-review.model';
import { ReportingTourProblem } from './model/reporting-tour-problem.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/env/environment';
import { Injectable } from '@angular/core';
import { Tour } from '../tour-authoring/model/tour.model';
import { TouristLocation } from './model/tourist-location';
import { KeyPoint } from '../tour-authoring/model/key-point.model';
import { TourExecution } from './model/tour-execution';
import { Object } from '../tour-authoring/model/object.model';
import { Encounter } from '../encounter-authoring/model/encounter.model';
import { EncounterExecution } from '../encounter-execution/model/encounter-execution.model';
@Injectable({
  providedIn: 'root'
})
export class TourExecutionService {

  constructor(private http: HttpClient) { }

  getTouristEquipment(): Observable<PagedResults<TouristEquipment>> {
    return this.http.get<PagedResults<TouristEquipment>>(environment.apiHost + 'administration/tourist_equipment')
  }

  addTouristEquipment(touristEquipment: TouristEquipment): Observable<TouristEquipment>{
    return this.http.post<TouristEquipment>(environment.apiHost+'administration/tourist_equipment',touristEquipment)
  }
  deleteTouristEquipment(id: number): Observable<TouristEquipment> {
    return this.http.delete<TouristEquipment>(environment.apiHost + 'administration/tourist_equipment/' + id);
  }

   getTourReview(): Observable<PagedResults<TourReview>>{
    return this.http.get<PagedResults<TourReview>>('https://localhost:44333/api/tourist/tour-review');
   }

   getTour(id: Number): Observable<Tour>{
    return this.http.get<Tour>('https://localhost:44333/api/tourist/touristLocation/'+ id);
  }

  addTouristLocation(touristLocation: TouristLocation): Observable<TouristLocation>{
    return this.http.post<TouristLocation>(environment.apiHost + 'tourist/touristLocation', touristLocation);
  }


  addReview(tourReview: TourReview): Observable<TourReview> {
    return this.http.post<TourReview>('https://localhost:44333/api/tourist/tour-review', tourReview);
  }

  deleteReview(id: number): Observable<TourReview> {
    return this.http.delete<TourReview>('https://localhost:44333/api/tourist/tour-review/' + id);
  }

  updateEquipment(tourReview: TourReview): Observable<TourReview> {
    return this.http.put<TourReview>('https://localhost:44333/api/tourist/tour-review/'+ tourReview.id, tourReview);
  }
  addReport(reportingTourProblem: ReportingTourProblem): Observable<ReportingTourProblem> {
    return this.http.post<ReportingTourProblem>('https://localhost:44333/api/tourist/tour-problem', reportingTourProblem);
  }
  getTourById(id: number): Observable<Tour> {
    return this.http.get<Tour>('https://localhost:44333/api/administration/tour/' + id);
  }
  getCompletedKeyPoints(id:number):Observable<KeyPoint[]>{
    return this.http.get<KeyPoint[]>('https://localhost:44333/api/tourExecutions/completed/'+id);
  }
  getLocationByTouristId(id: number): Observable<TouristLocation>{
    return this.http.get<TouristLocation>('https://localhost:44333/api/tourist/touristLocation/'+id);
  }
  updateTouristLocation(touristLocation: TouristLocation): Observable<TouristLocation>{
    return this.http.put<TouristLocation>(environment.apiHost + 'tourist/touristLocation/' +touristLocation.id,touristLocation);
  }
  checkLocation(location: TouristLocation,id:number): Observable<any>{
    return this.http.put<TouristLocation>('https://localhost:44333/api/tourExecutions/checkLocation/'+id,location);
  }
  getAllTouristTours(id: number): Observable<TourExecution[]>{
    return this.http.get<TourExecution[]>('https://localhost:44333/api/tourExecutions/allMyTours/' + id);
  }
  startNewTourExecution(selectedTour: Tour, touristId: number): Observable<TourExecution>{
    return this.http.post<TourExecution>('https://localhost:44333/api/tourExecutions/startNewTour/' + touristId, selectedTour);
  }

  getOnGoingTour(touristId: number): Observable<TourExecution>{
    return this.http.get<TourExecution>('https://localhost:44333/api/tourExecutions/activeTour/' + touristId);
  }

  leaveStartedTour(leavedTour: TourExecution): Observable<TourExecution>{
    return this.http.put<TourExecution>('https://localhost:44333/api/tourExecutions/leaveTour',leavedTour);
  }
  completeTour(completedTourId: number): Observable<TourExecution>{
    return this.http.put<TourExecution>('https://localhost:44333/api/tourExecutions/finishTour/'+completedTourId,this.completeTour);
  }
  getPublicKeyPoints():Observable<PagedResults<KeyPoint>>{
    return this.http.get<PagedResults<KeyPoint>>('https://localhost:44333/api/tours/keypoint/public');
  }
  getPublicTourObjects():Observable<PagedResults<Object>>{
    return this.http.get<PagedResults<Object>>('https://localhost:44333/api/tours/object/public');
  }
  getEncountersByTour(tourId?: number): Observable<Encounter[]> {
    
      return this.http.get<Encounter[]>(`https://localhost:44333/api/user/encounters/byTour/${tourId}`); 
  }

  activateEncounter(encounterExecution: EncounterExecution): Observable<EncounterExecution> {
    return this.http.post<EncounterExecution>(`https://localhost:44333/api/tourist/encounterExecution`, encounterExecution);
  }
}
