import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Blogs } from './model/blog.model';
import { environment } from 'src/env/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenStorage } from 'src/app/infrastructure/auth/jwt/token.service';
import { of } from 'rxjs';
import { Rating } from './model/rating.model';


import { Comment, CommentDto } from './model/comment.model';
import { Image } from '../tour-authoring/model/image.model';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  constructor(private http: HttpClient,  private tokenStorage: TokenStorage) { }
  getFilteredBlogs(endpoint: string): Observable<Blogs[]> {
    return this.http.get<Blogs[]>('https://localhost:44333'+endpoint);
  }
  getBlogs() : Observable<PagedResults<Blogs>>{
    return this.http.get<PagedResults<Blogs>>('https://localhost:44333/api/blogs');
  }

  addBlog(blog: Blogs): Observable<Blogs> {
    return this.http.post<Blogs>(environment.apiHost + 'blogs', blog)
  }
  updateRating(blogId: number,  rat : Rating) : Observable<Blogs>{
    var url = `https://localhost:44333/api/blogs/rating/${blogId}`
    return this.http.put<Blogs>(url, rat)
  }
  getBlogById(blogId: number): Observable<Blogs> {
    const url = 'https://localhost:44333/api/blogs/'+blogId;
    return this.http.get<Blogs>(url);
  }
  addComment(blogId: number, comment: Comment): Observable<Comment> {
    const url = `https://localhost:44333/api/blogs/${blogId}/comments`;
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  
    const payload: CommentDto = {
      Text: comment.text, // Map `text` from Comment to `Text` in CommentDto
      Username: comment.username, // Map `username` to `Username`
      CreatedAt: comment.lastModified,     // Pass the CreatedAt value
      LastModified: comment.lastModified,  
      userId : comment.userId
    };
  
    return this.http.post<Comment>(url, payload, { headers });
  }
  

  // PUT: Update an existing comment in a blog
  updateComment(blogId: number, commentId: number, newText: string): Observable<Comment> {
    const url = `https://localhost:44333/api/blogs/${blogId}/comments/${commentId}`;
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  
    // Send `newText` directly as a JSON string
    return this.http.put<Comment>(url, JSON.stringify(newText), { headers });
  }

  // DELETE: Delete a comment from a blog
  deleteComment(blogId: number, commentId: number): Observable<void> {
    const url = 'https://localhost:44333/api/blogs/' + blogId + '/comments/' + commentId;
    return this.http.delete<void>(url);
  }
  publishBlog(blogId: number, blog: Blogs): Observable<Blogs>{
    var url = `https://localhost:44333/api/blogs/publish/${blogId}`
    return this.http.put<Blogs>(url, blog)
  }

  closeBlog(blogId: number, blog: Blogs): Observable<Blogs>{
    var url = `https://localhost:44333/api/blogs/close/${blogId}`
    return this.http.put<Blogs>(url, blog)
  }

  addImage(formData: FormData){
    return this.http.post<Image>('https://localhost:44333/api/blog/image', formData);
  }

  getImage(filePath: string): Observable<Blob> {
    const params = new HttpParams().set('filePath', filePath);
    return this.http.get<Blob>('https://localhost:44333/api/blog/image', { params, responseType: 'blob' as 'json' });
  }
}
