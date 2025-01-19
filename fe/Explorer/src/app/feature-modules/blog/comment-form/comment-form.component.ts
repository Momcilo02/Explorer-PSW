import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CommentService } from '../comment.service';
import { Comment } from '../model/comment.model';
import { BlogService } from '../blog.service';
import { Router } from '@angular/router';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'app-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})
export class CommentFormComponent implements OnChanges {

  @Input() comment: Comment ; // Izabrani komentar za uređivanje
  @Input() blogId: number ; // Izabrani komentar za uređivanje
  user : User;
  userId : number;
  @Output() commentAdded = new EventEmitter<null>();
  @Output() commentUpdated = new EventEmitter<null>(); // Emituje kada je komentar ažuriran

  markdown = '';

  // Definisanje Reactive Forms grupe sa poljima za unos komentara
  commentForm = new FormGroup({
    userId: new FormControl('', [Validators.required, Validators.min(1)]), // Polje za unos userId (kao string)
    text: new FormControl('', [Validators.required]) // Polje za unos teksta komentara
  });

  constructor(private service: CommentService,private blogService: BlogService,private router: Router,private authService : AuthService) { }

  ngOnChanges(changes: SimpleChanges): void {
    // Ako postoji komentar za editovanje, popuni formu
    if (this.comment) {
      this.commentForm.patchValue({
        userId: String(this.comment.userId), // Konvertujemo userId u string za formu
        text: this.comment.text
      });
    } else {
      // Resetuje formu kada nema izabranog komentara
      this.commentForm.reset();
    }
  }

  // Metoda za dodavanje ili ažuriranje komentara
  addOrUpdateComment(): void {
    // Convert userId from string to number
  
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.userId = user.id;
    });
  
    const currentDate = new Date().toISOString(); // Get current date in ISO format
  
    if (this.comment) {
      // Update existing comment
      const updatedComment: Comment = {
        ...this.comment,
        userId: this.userId,
        text: this.commentForm.value.text || '',
        lastModified: currentDate
      };
      const newText = this.commentForm.value.text || '';
      this.blogService.updateComment(this.blogId, this.comment.id, newText).subscribe({
        next: (updatedComment) => {
          console.log('Comment updated successfully:', updatedComment);
          this.router.navigate([`blogs`]); // Redirect to the blog page
        },
        error: (error) => {
          console.error('Error updating comment:', error);
        }
      });
    } else {
      // Add new comment
      const newComment: Comment = {
        id: 0, // The backend will generate the ID
        userId: this.userId,
        text: this.commentForm.value.text || '',
        username: "dwajhdjk", // Replace with actual username if available
        createdAt: currentDate,
        lastModified: currentDate
      };
  
      this.blogService.addComment(this.blogId, newComment).subscribe({
        next: (comment) => {
          console.log('Comment added successfully:', comment);
          this.router.navigate([`blogs`]); // Redirect to the blog page
    
        },
        error: (err) => {
          console.error('Error adding comment:', err);
        }
      });
    }
  }

  // Ažurira markdown kad se tekst komentara promeni
  descUpdated(): void {
    this.markdown = this.commentForm.value.text || '';
  }
}
