import { Component, OnInit } from '@angular/core';
import { BlogDto, Blogs } from '../model/blog.model';
import { BlogService } from '../blog.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
//import { use } from 'marked';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { Rating } from '../model/rating.model';
import { Router } from '@angular/router';


@Component({
  selector: 'xp-blogs',
  templateUrl: './blogs.component.html',
  styleUrls: ['./blogs.component.css']
})
export class BlogsComponent implements OnInit {

  blogs: Blogs[] = []
  addingBlog: boolean = false;
  blogsDtos: BlogDto[] = [];
  userId: number;
  user: User;
  selectedBlogId: number;

  constructor(private service: BlogService, private authService: AuthService, private router : Router) { }

  ngOnInit(): void {
    this.checkUserRole();
    
  }

  checkUserRole(){
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.userId = user.id;
      this.getBlogs();
    });
  }

  filterByStatus(status: string): void {
    let apiEndpoint = '';

    if (status === 'active') {
        apiEndpoint = '/api/blogs/active';
    } else if (status === 'famous') {
        apiEndpoint = '/api/blogs/famous';
    }

    this.service.getFilteredBlogs(apiEndpoint).subscribe({
        next: (filteredBlogs: Blogs[]) => {
            this.blogsDtos = filteredBlogs.map(blog => ({
                blog,
                isUpvoted: false,
                isDownvoted: false,
                isOwner: blog.ownerId === this.userId,
                voteCount: blog.ratings ? blog.ratings.reduce((count, rating) => count + (rating.grade ? 1 : -1), 0) : 0
            }));
        },
        error: (err: any) => {
            console.log(err);
        }
    });
}
  getBlogs() : void{
    this.blogs = []
    this.blogsDtos = []
    this.service.getBlogs().subscribe({
      next: (result: PagedResults<Blogs>) => {
        this.blogs = result.results;
        this.addingBlog = false;
        this.checkUserData();
      },
      error: (err: any) => {
        console.log(err)
      }
    })
  }
  checkUserData():void {
    this.blogs.forEach(blog => {
      var b : BlogDto = {
          blog : blog,
          isDownvoted : false,
          isUpvoted : false,
          isOwner : false,
          voteCount: 0,
      }
      if(b.blog.ownerId === this.userId){
        b.isOwner = true;
            console.log(b.isOwner);
      }
      if(blog.ratings && blog.ratings.length>0){
        blog.ratings.forEach(rat => {
          if(rat.userId === this.userId){
            if(rat.grade === true){
              b.isUpvoted = true;
            }else{
              b.isDownvoted = true;
            }
            
          }
          if(rat.grade === true){
            b.voteCount+=1;
          }else{
            b.voteCount-=1;
          }
        });
      }
        if(blog.status !== 0 || blog.ownerId === this.userId){
          this.blogsDtos.push(b);
          
        }
        
    });
  }
  showComments(blogId: number): void {
    console.log(blogId)
    this.router.navigate([`/blogs/${blogId}/comments`]); 
  }
  addBlogClick(): void {
    this.addingBlog = !this.addingBlog
  }
  upvote(blog : BlogDto): void {
    if(blog.isUpvoted ===  true){
      blog.isUpvoted = false;
      blog.voteCount-=1;
    }else{
      blog.isUpvoted =true;
      if(blog.isDownvoted){
        blog.voteCount+=1;
      }
      blog.isDownvoted = false;
      blog.voteCount+=1;
      //dodaj poziv za updejtovanje
    }
    var rat : Rating = {
      grade : true,
      userId : this.userId,
      creationTime: "2024-11-04T20:40:17.083Z"
    }
    console.log(rat)
    this.service.updateRating(blog.blog.id, rat).subscribe({
      next:() =>{
        console.log("Dodao sam rating");
      }
    })
  }

  downvote(blog : BlogDto): void {
    if(blog.isDownvoted ===  true){
      blog.isDownvoted = false;
      blog.voteCount+=1;
    }else{
      
      if(blog.isUpvoted){
        blog.voteCount-=1;
      }
      blog.isUpvoted =false;
      blog.isDownvoted = true;
      blog.voteCount-=1;
      //dodaj poziv za updejtovanje
    }
    var rat : Rating = {
      grade : false,
      userId : this.userId,
      creationTime: ""
    }
    var rat : Rating = {
      grade : false,
      userId : this.userId,
      creationTime: "2024-11-04T20:40:17.083Z"
    }
    console.log(rat)
    this.service.updateRating(blog.blog.id, rat).subscribe({
      next:() =>{
        console.log("Dodao sam rating");
      }
    })

  }

  closeBlog(blog : BlogDto){
    this.service.closeBlog(blog.blog.id, blog.blog).subscribe({
      next:()=>{
        console.log("Uspesno zatvoren blog")
        this.blogsDtos = []
        this.getBlogs();
      }
    })
  }
  publishBlog(blog: BlogDto){
    this.service.publishBlog(blog.blog.id, blog.blog).subscribe({
      next:()=>{
        console.log("Uspesno pablishovan blog")
        this.blogsDtos = []
        this.getBlogs();
      }
    })
  }
}
  
