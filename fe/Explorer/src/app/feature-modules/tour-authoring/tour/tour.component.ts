import { Component, OnInit } from '@angular/core';
import { TourService } from '../tour.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Tour } from '../model/tour.model';

import { ActivatedRoute } from '@angular/router';
import { KeyPoint } from '../model/key-point.model';

import { Router } from '@angular/router';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { ImagesService } from '../images.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-tour',
  templateUrl: './tour.component.html',
  styleUrls: ['./tour.component.css']
})
export class TourComponent implements OnInit {
  tours: Tour[]=[];
  selectedTour: Tour | null = null;
  shouldEdit: boolean;
  shouldRenderTourForm: boolean=false;
  loggedInUser: User;
  addTour: boolean = false;
  showEquipment: boolean = false;
  imageMap: Map<string, string> = new Map();

  constructor( private imageService: ImagesService,private service: TourService, private router: Router, private authService: AuthService,private snackBar: MatSnackBar){}

  ngOnInit(): void{
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.getTours();
  }

  getImage(path: string): void {
    this.imageService.getImage(path).subscribe(blob => {
      const image = URL.createObjectURL(blob);
      this.imageMap.set(path, image);
    });
  }

  getTours(): void {
    this.shouldRenderTourForm=false;
    this.addTour = false;
    if(this.loggedInUser.role === 'author'){
      this.service.getMyTours().subscribe({
        next: (result: PagedResults<Tour>) => {
          this.tours = result.results;

          for (let to of this.tours) {
            this.getImage(to.image);
          }
        },
        error:(err: any) =>{
          console.log(err)
        }
      })
    }
    else{
      this.service.getTours().subscribe({
        next: (result: PagedResults<Tour>) => {
          this.tours = result.results;
        },
        error:(err: any) =>{
          console.log(err)
        }
      })
    }

  }
  onTourCreated(tour: Tour): void {
    const createQuiz = confirm("Do you want to create a quiz for this tour?");
    if (createQuiz) {
      this.router.navigate([`/create-quiz/${tour.id}`]);
    } else {
      this.getTours();
    }
  }
  onEditClicked(tours:Tour): void{
    this.shouldEdit =  true;
    this.selectedTour = tours;
    this.shouldRenderTourForm=true;
    console.log(tours);
  }

  onAddClicked(): void{
    this.addTour=true;
    this.shouldEdit=false;
    this.shouldRenderTourForm = true;
  }
  
  onDeleteClicked(tour: Tour){
    const snackBarRef = this.snackBar.open('Are you sure you want to delete this bundle?', 'Confirm', {
      duration: 5000, 
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'info',
    });

    snackBarRef.onAction().subscribe(() => {
      this.deleteTour(tour);
    });
    
  }

  deleteTour(tour: Tour): void{
    var conifrm = this.snackBar.open('Are you sure you want to delete this tour?', 'Confirm', {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: 'confirm-snack-bar',
      
    });
    if(!conifrm)
      return;

    this.service.deleteTour(tour).subscribe({
      next: (_) =>{
        this.getTours();
        this.snackBar.open('Deleted tour!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
          
        });
      }
    })
  }
  viewKeyPoints(id: number|undefined) {
    this.router.navigate([`${id}/keypoint`])
  }
  viewPublicKeyPoints(id: number|undefined) {
    this.router.navigate([`${id}/publickeypoint`])
  }

  getStatus(id: number) : string {
    if(id === 0)
      return "Draft";
    else if(id === 1)
      return "Published";
    else
      return "Archived";
  }
  reportProblem(tour: Tour): void {
    this.router.navigate([`reporting-tour-problem/${tour.id}`])

  }

  showTourReviews(id: number|undefined) : void{
    this.router.navigate([`show-tour-reviews/${id}`])
  }

  publish(tour: Tour) {
    this.service.publishTour(tour).subscribe({
      next: (res: Tour) => {
        this.getTours();
        this.snackBar.open('Successfully published tour', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
          
        });
      },
      error: (err: any) => {
        // alert("You cannot post this tour. The tour must have 2 key points, one duration...");
        this.snackBar.open('You cannot post this tour. The tour must have 2 key points, one duration...', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
          
        });
      }
    })
  }

  archive(tour: Tour) {
    this.service.archiveTour(tour).subscribe({
      next: (res: Tour) => {
        this.getTours();
        this.snackBar.open('Archived tour', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'info',
          
        });
      },
      error: (err: any) => {
        //alert("Ne mozes objaviti ovu turu");
        this.snackBar.open('You cannot publish this tour', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
          
        });
      }
    })
  }

  reactivateTour(tour:Tour){
    this.service.reactivateTour(tour).subscribe({
      next: (res:Tour) =>{
        this.getTours();
        this.snackBar.open('Reactivated tour', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'info',
          
        });
      },
      error: (err: any)=>{
        //alert("Ne mozes reaktivirati ovu turu");
        this.snackBar.open('You cannot reactivate this tour', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'error',
          
        });
      }
    })
  }

  hasTourDuration(t: Tour): number{
    if(t.tourDurations === undefined)
      return 0;
    if(t.tourDurations[0] === undefined)
      return 0;
    return t.tourDurations[0].duration;
  }

  showTourEquipment(t: Tour) {
    this.selectedTour = t;
    this.showEquipment = true;
  }
  stopShowEquipment() {
    this.selectedTour = null;
    this.showEquipment = false;
  }
  getDifficulty(diff: number): string{
    if(diff === 0)
      return "Easy";
    else if(diff === 1)
      return "Medium";
    else if(diff === 2)
      return "Hard";
    else if(diff === 3)
      return "Hell";
    else
      return "Not set"
  }

  closeModal() {
    this.shouldRenderTourForm = false;
    this.showEquipment = false;
  }

  getTimeUnit(time: number): string {
    if(time === 0)
      return "Seconds";
    else if(time === 1)
      return "Minutes";
    else
      return "Hours";
  }
}
