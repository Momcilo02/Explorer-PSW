import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Comment } from './model/comment.model';

import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { environment } from 'src/env/environment';
@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = `/api/administration/comments`;
  constructor(private http: HttpClient) { }
  getComments() : Observable<PagedResults<Comment>>{
    return this.http.get<PagedResults<Comment>>('https://localhost:44333/api/administration/comments');
  }
  addComment(comment: Comment): Observable<Comment> {
    return this.http.post<Comment>(environment.apiHost + 'administration/comments', comment);
  }
  deleteComment(commentId: number): Observable<void> {
    const url = `https://localhost:44333/api/administration/comments/${commentId}`; // DELETE /api/administration/comments/{commentId}
    return this.http.delete<void>(url);
  }
  updateComment(comment: Comment): Observable<Comment> {
    const url = `https://localhost:44333/api/administration/comments/${comment.id}`;
    return this.http.put<Comment>(url, comment);
  }
}

