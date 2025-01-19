//https://localhost:44333/api/administration/applicationGrade
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class ApplicationGradeService {
  private apiUrl = 'https://localhost:44333/api/administration/applicationGrade'; // Ispravan API endpoint

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {}

  getUserId(): number {
    const token = localStorage.getItem('access-token');
    if (!token) {
      return 0; // Vraća 0 ako token ne postoji
    }
    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken?.sub || 0;
  }

  addGrade(grade: any): Observable<any> {
    const token = localStorage.getItem('access-token');
    if (token) {
      const decoded = this.jwtHelper.decodeToken(token);
      const userId = decoded?.id || 0;
      grade.userId = userId;  // Dodaj userId u podatke o oceni
    }
    return this.http.post(this.apiUrl, grade);
  }

  getGrades(): Observable<any> {
    return this.http.get<any>(this.apiUrl); // Proveri da je tip povratne vrednosti tačan
  }
  
}


