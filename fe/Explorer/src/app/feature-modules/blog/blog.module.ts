import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogsComponent } from './blogs/blogs.component';
import { BlogFormComponent } from './blog-form/blog-form.component';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MarkdownModule } from 'ngx-markdown';
import { SharedModule } from "../../shared/shared.module";
import { CommentComponent } from './comment/comment.component';
import { CommentFormComponent } from './comment-form/comment-form.component';
import { LMarkdownEditorModule } from 'ngx-markdown-editor';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { NgModel } from '@angular/forms';


@NgModule({
  declarations: [
    BlogsComponent,
    BlogFormComponent,
    CommentComponent,
    CommentFormComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MarkdownModule.forRoot(),
    SharedModule,
    LMarkdownEditorModule,
    BrowserModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    

],
  exports: [
    BlogsComponent,
    BlogFormComponent
  ]
})
export class BlogModule { }
