import { Component, EventEmitter, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Blogs } from '../model/blog.model';
import {format} from 'date-fns'
import { BlogService } from '../blog.service';
import { CalendarDate } from 'calendar-date';
import { Rating } from '../model/rating.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import {Comment} from '../model/comment.model'

@Component({
  selector: 'xp-blog-form',
  templateUrl: './blog-form.component.html',
  styleUrls: ['./blog-form.component.css']
})
export class BlogFormComponent implements OnChanges {

  @Output() blogAdded = new EventEmitter<null>();
  public description : string = "text"
  imageUrls: string[] = []
  ratings: Rating[] = []
  comments: Comment[] = []
  today: CalendarDate 
  markdown = "";
  user: User;
  userId: number;
  imageMap: Map<string, string> = new Map();
  constructor(private service: BlogService, private authService: AuthService) { }

  checkUserRole(){
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.userId = user.id;
      console.log(this,this.userId)
    });
  }
  ngOnInit(): void {
    this.checkUserRole();
    
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.checkUserRole()
    this.blogForm.reset()
    this.imageUrls = []
  }

  blogForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    imageUrl: new FormControl('')
  })

  addBlog(): void{
    if(this.imageUrls.length ===0){
      this.imageUrls.push("")
    }
    
    const d = new Date()
    this.today = CalendarDate.fromDateUTC(d)
    console.log("Desc: " + this.description);
    const blog: Blogs = {
      id: 0,
      ownerId: this.userId,
      title: this.blogForm.value.title || "",
      //description: this.description,
      description: this.blogForm.value.description || "",
      imageUrl: this.imageUrls,
      status: 0,
      comments: this.comments,
      ratings: this.ratings,
      date: this.today.toFormat('yyyy-MM-dd'),
      activityStatus: 0
    };
    console.log(this.imageUrls)
    this.service.addBlog(blog).subscribe({
      next : () =>{
        
        this.blogAdded.emit()
      }
    })
  }

  descUpdated(): void{
    this.markdown = this.blogForm.value.description || "";
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files) {
      this.uploadImage(target.files[0]);
    }
  }
  uploadImage(file: File) {
    if (file) {
      const formData = new FormData();
      formData.append('file', file, file.name);
      console.log(formData)
      this.service.addImage(formData).subscribe({
        next: (res) =>{
          this.imageUrls.push(res.filePath)
          this.getImage(res.filePath);
        }
      });
    }
  }
  getImage(path: string): void {
    this.service.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }
  addComment(event: Event){
    const target = event.target as HTMLInputElement;
    if(target.textContent){
      console.log(target.textContent)
    }
  }

}
