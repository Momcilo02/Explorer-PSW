import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { TokenStorage } from './jwt/token.service';
import { environment } from 'src/env/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Login } from './model/login.model';
import { AuthenticationResponse } from './model/authentication-response.model';
import { User } from './model/user.model';
import { Registration } from './model/registration.model';
import { Image } from './model/image.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user$ = new BehaviorSubject<User>({ username: "", id: 0, role: "", email: "" });

  constructor(private http: HttpClient,
              private tokenStorage: TokenStorage,
              private router: Router) { }

  login(login: Login): Observable<AuthenticationResponse> {
    return this.http
      .post<AuthenticationResponse>(environment.apiHost + 'users/login', login)
      .pipe(
        tap((authenticationResponse) => {
          this.tokenStorage.saveAccessToken(authenticationResponse.accessToken);
          this.setUser();
        })
      );
  }

  register(registration: Registration): Observable<AuthenticationResponse> {
    return this.http
      .post<AuthenticationResponse>(environment.apiHost + 'users', registration)
      .pipe(
        tap((authenticationResponse) => {
          this.tokenStorage.saveAccessToken(authenticationResponse.accessToken);
          this.setUser();
        })
      );
  }

  logout(): void {
    this.router.navigate(['/home']).then(_ => {
      this.tokenStorage.clear();
      this.user$.next({ username: "", id: 0, role: "", email: "" });
    });
  }

  checkIfUserExists(): void {
    const accessToken = this.tokenStorage.getAccessToken();
    if (accessToken == null) {
      return;
    }
    this.setUser();
  }

  getUsername(userId: number | undefined): Observable<Login> {
    console.log('Fetching username for user ID:', userId);
    return this.http.get<Login>(environment.apiHost + 'users/username/' + userId);
  }

  private setUser(): void {
    const jwtHelperService = new JwtHelperService();
    const accessToken = this.tokenStorage.getAccessToken() || "";

    // Decode token
    const decodedToken = jwtHelperService.decodeToken(accessToken);

    // Use bracket notation to access the role key
    const roleKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
    const user: User = {
      id: +decodedToken.id, // Convert id to a number
      username: decodedToken.username || "unknown", // Provide a default for username
      role: decodedToken[roleKey] || "unknown", // Access role using bracket notation
      email: decodedToken.email || "no-email@example.com" // Provide a default for email
    };
    console.log('Decoded Token:', decodedToken);
console.log('Extracted Role:', decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);

    this.user$.next(user);
  }

  addImage(formData: FormData){
    return this.http.post<Image>(environment.apiHost + 'profile/image', formData);
  }

  getImage(filePath: string): Observable<Blob> {
    const params = new HttpParams().set('filePath', filePath);
    return this.http.get<Blob>(environment.apiHost + 'profile/image', { params, responseType: 'blob' as 'json' });
  }


}
