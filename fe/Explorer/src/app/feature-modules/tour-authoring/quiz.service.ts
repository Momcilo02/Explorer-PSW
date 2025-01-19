import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/env/environment';
import { Quiz } from './model/quiz.model';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private baseUrl = `${environment.apiHost}quizzes`;

  constructor(private http: HttpClient) {}

  createQuiz(quiz: Quiz): Observable<Quiz> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Quiz>(`${this.baseUrl}`, quiz, { headers });
  }

  updateQuiz(id: number, quiz: Quiz): Observable<Quiz> {
    return this.http.put<Quiz>(`${this.baseUrl}/${id}`, quiz);
  }

  deleteQuiz(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
  getQuiz(tourId: number): Observable<Quiz> {
    return this.http.get<Quiz>(`${this.baseUrl}/byTour/${tourId}`);
  }
}
