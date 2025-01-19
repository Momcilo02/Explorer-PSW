import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Notification } from './model/notification.model';

@Injectable({
  providedIn: 'root'
})
export class UserNotificationsService {

  constructor(private http: HttpClient) { }

  getNotification(userId: number): Observable<PagedResults<Notification>>{
    return this.http.get<PagedResults<Notification>>(`https://localhost:44333/api/user/notification/getByLoggedUser/${userId}`);
   }

   /*api/user/notification/1 */
   notificationIsRead(notification: Notification): Observable<Notification> {
      return this.http.put<Notification>(`https://localhost:44333/api/user/notification/${notification.id}`, notification);
    }
}
