import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Object } from './model/object.model';
import { environment } from 'src/env/environment';

@Injectable({
  providedIn: 'root'
})
export class ObjectService {

  constructor(private http: HttpClient) { }

  getObjects(): Observable<PagedResults<Object>>{
    return this.http.get<PagedResults<Object>>('https://localhost:44333/api/tours/object');
  }

  addObject(object: Object) : Observable<Object>{
    console.log("Ovaj objekat saljem: ", object);
    return this.http.post<Object>(environment.apiHost + 'tours/object', object);
  }

  updateObject(object: Object) : Observable<Object> {
    return this.http.put<Object>(environment.apiHost + 'tours/object/' + object.id, object);
  }

  deleteObject(object:Object) : Observable<Object> {
    return this.http.delete<Object>(environment.apiHost + 'tours/object/' + object.id);
  }
}
