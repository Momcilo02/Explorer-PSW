import { Component, Input } from '@angular/core';
import { CommentService } from '../comment.service';
import { Comment } from '../model/comment.model'; // Ensure this import is correct
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { BlogService } from '../blog.service';
import { Blogs } from '../model/blog.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'xp-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent {
  @Input() blogId!: number
  comments: Comment[] = [];
  blog: Blogs;
  addingComment: boolean = false;
  editingComment = false;
  selectedComment: Comment; // Služi za čuvanje komentara koji se uređuje
  imageUrls: string[] = []
  imageMap: Map<string, string> = new Map();
  constructor(private service: CommentService,private blogService: BlogService,private route: ActivatedRoute,private router: Router) {

   }
   ngOnInit(): void {
    // Retrieve blogId from the route parameters
    this.route.paramMap.subscribe(params => {
      const blogIdParam = params.get('blogId');
      if (blogIdParam) {
        this.blogId = +blogIdParam; // Convert to a number
        this.getComments();
      } else {
        console.error('blogId not found in route');
      }
    });
  }

  getComments() : void{
    console.log("RA")
    console.log(this.blogId);
    this.blogService.getBlogById(this.blogId).subscribe({
      next: (blog) => {
        this.blog = blog;
        console.log(blog.comments);
        blog.imageUrl.forEach(element => {
          this.getImage(element);
        });
          
        
      },
      error: (err) => {
        console.error('Error fetching blog:', err);
      }
    }); 
  }
  addCommentClick(): void {
    this.addingComment = !this.addingComment
  }
  editComment(comment: Comment): void {
    this.selectedComment = comment;
    this.editingComment = true;
    this.addingComment = false;
  }
  deleteComment(commentId: number): void {
    if (confirm('Are you sure you want to delete this comment?')) {
      this.blogService.deleteComment(this.blogId, commentId).subscribe({
        next: () => {
          console.log('Comment deleted successfully');
          this.comments = this.comments.filter((c) => c.id !== commentId);
          this.router.navigate([`blogs`]); // Redirect to the blog page

        },
        error: (err) => {
          console.error('Error deleting comment:', err);
        }
      });
    }}
  
    getImage(path: string): void {
      this.blogService.getImage(path).subscribe(blob => {
        const image = URL.createObjectURL(blob);
        this.imageMap.set(path, image);
      });
    }
  
}