import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Image } from './model/image.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  constructor(private http : HttpClient) { }

  addImage(formData: FormData){
    return this.http.post<Image>('https://localhost:44333/api/tour/image', formData);
  }

  getImage(filePath: string): Observable<Blob> {
    const params = new HttpParams().set('filePath', filePath);
    return this.http.get<Blob>('https://localhost:44333/api/tour/image', { params, responseType: 'blob' as 'json' });
  }
}
