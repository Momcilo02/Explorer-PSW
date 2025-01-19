import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TouristClub } from './model/touristClub.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/env/environment';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { of } from 'rxjs';
import { ProfileInfo } from './model/profileInfo.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenStorage } from 'src/app/infrastructure/auth/jwt/token.service';
import { Result } from 'postcss';
import { Tour } from '../tour-authoring/model/tour.model';
import { Blogs } from '../blog/model/blog.model';
import { ProfileMessage } from './model/profileMessage.model';
import { ClubMessage } from './model/clubMessage.model';
import { ClubMessageComponent } from './club-message/club-message.component';



@Injectable({
  providedIn: 'root'
})
export class LayoutService {



  getTouristClub(): Observable<PagedResults<TouristClub>>{
    return this.http.get<PagedResults<TouristClub>>('https://localhost:44333/api/tourist/touristClub');
  }

  addTouristClub(touristClub : TouristClub) : Observable<TouristClub>{
    return this.http.post<TouristClub>('https://localhost:44333/api/tourist/touristClub', touristClub);
  }

  updateTouristClub(touristClub : TouristClub) : Observable<TouristClub>{
    return this.http.put<TouristClub>('https://localhost:44333/api/tourist/touristClub/' + touristClub.id, touristClub);
  }

  constructor(private http: HttpClient, private authService: AuthService, private tokenStorage: TokenStorage) { }

  fetchCurrentUser(): Observable<any> {
    const jwtHelperService = new JwtHelperService();
    const accessToken = this.tokenStorage.getAccessToken() || "";
    const personId = jwtHelperService.decodeToken(accessToken).id;
    if (personId) {
      return this.http.get(environment.apiHost + `profile-administration/edit/${personId}`);
    }
    return of(null);
  }



  saveNewInfo(profileInfo: ProfileInfo, formData: FormData): Observable<any> {
    const options = { headers: new HttpHeaders() };

    formData.append('id', profileInfo.id.toString());
    formData.append('userId', profileInfo.userId.toString());
    formData.append('name', profileInfo.name);
    formData.append('surname', profileInfo.surname);
    formData.append('email', profileInfo.email);
    formData.append('biography', profileInfo.biography);
    formData.append('motto', profileInfo.motto);



    formData.append('profilePictureUrl', profileInfo.profilePictureUrl);

    return this.http.put(environment.apiHost + 'profile-administration/edit', formData, options);

  }

  updateCurrentUser(currentUser: ProfileInfo): Observable<ProfileInfo>{
    return this.http.put<ProfileInfo>(environment.apiHost+'profile-administration/edit/currentUser',currentUser,{ headers: { 'Content-Type': 'application/json' } });
  }


  updateSelectedUser(selectedUser: ProfileInfo): Observable<ProfileInfo>{
    return this.http.put<ProfileInfo>(environment.apiHost+'profile-administration/edit/currentUser',selectedUser,{ headers: { 'Content-Type': 'application/json' } });
  }


  getAllUsers(): Observable<PagedResults<ProfileInfo>>{
    return this.http.get<PagedResults<ProfileInfo>>(environment.apiHost + 'profile-administration/edit');
  }

  getAllTours(): Observable<PagedResults<Tour>>{
    return this.http.get<PagedResults<Tour>>('https://localhost:44333/api/tours');
  }

  getAllBlogs(): Observable<PagedResults<Blogs>>{
    return this.http.get<PagedResults<Blogs>>('https://localhost:44333/api/blogs');
  }

  getAllProfileMessages(senderId: number, receiverId: number): Observable<PagedResults<ProfileMessage>>{
    return this.http.get<PagedResults<ProfileMessage>>(`https://localhost:44333/api/profile-message/${senderId}/${receiverId}`);
  }

  sendProfileMessage(message: ProfileMessage): Observable<ProfileMessage>{
    return this.http.post<ProfileMessage>(`https://localhost:44333/api/profile-message`, message);
  }

  getAllTouristClubMessages(touristClubId: number): Observable<PagedResults<ClubMessage>>{
    return this.http.get<PagedResults<ClubMessage>>(`https://localhost:44333/api/club-message/${touristClubId}`);
  }

  getAllTouristClubMessagesForLoggedUser(touristClubId: number, userId: number): Observable<PagedResults<ClubMessage>>{
    return this.http.get<PagedResults<ClubMessage>>(`https://localhost:44333/api/club-message/${touristClubId}/${userId}`);
  }

  createTouristClubMessage(message: ClubMessage):  Observable<ClubMessage>{
    return this.http.post<ClubMessage>(`https://localhost:44333/api/club-message`, message);
  }

  updateTouristClubMessage(clubMessageId: number, message: ClubMessage):  Observable<ClubMessage>{
    return this.http.put<ClubMessage>(`https://localhost:44333/api/club-message/${clubMessageId}`, message);
  }

  deleteTouristClubMessage(clubMessageId: number):  Observable<ClubMessage>{
    return this.http.delete<ClubMessage>(`https://localhost:44333/api/club-message/${clubMessageId}`);
  }

  addMemberInClub(touristClub : TouristClub) : Observable<TouristClub>{
    return this.http.put<TouristClub>('https://localhost:44333/api/tourist/touristClub/addMember/' + touristClub.id, touristClub);
  }

  addClubInMember(member: ProfileInfo): Observable<ProfileInfo>{
    return this.http.put<ProfileInfo>(environment.apiHost+'profile-administration/edit/clubMember',member,{ headers: { 'Content-Type': 'application/json' } });
  }

  changeTouristStatus(tourist: ProfileInfo): Observable<ProfileInfo>{
    return this.http.put<ProfileInfo>(environment.apiHost+'profile-administration/edit/changeStatus',tourist,{ headers: { 'Content-Type': 'application/json' } });
  }

  incrementClubMessageLikes(message: ClubMessage, clubMessageId: number, userId: number): Observable<ClubMessage>{
    return this.http.put<ClubMessage>(`https://localhost:44333/api/club-message/like/${clubMessageId}/${userId}`,message,{ headers: { 'Content-Type': 'application/json' } });
  }

  decrementClubMessageLikes(message: ClubMessage, clubMessageId: number, userId: number): Observable<ClubMessage>{
    return this.http.put<ClubMessage>(`https://localhost:44333/api/club-message/dislike/${clubMessageId}/${userId}`,message,{ headers: { 'Content-Type': 'application/json' } });
  }

  getTouristClubById(touristClubId: number): Observable<TouristClub>{
    return this.http.get<TouristClub>(`https://localhost:44333/api/tourist/touristClub/${touristClubId}`,{ headers: { 'Content-Type': 'application/json' } });
  }

  rateClub(touristClub: TouristClub){
    return this.http.put<TouristClub>('https://localhost:44333/api/tourist/touristClub/rates/' + touristClub.id, touristClub);
  }





}
