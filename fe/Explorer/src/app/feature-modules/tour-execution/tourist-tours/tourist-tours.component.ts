import { Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { TourExecutionService } from '../tour-execution.service';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { TourExecution } from '../model/tour-execution';
import { Tour } from '../../tour-authoring/model/tour.model';
import { TourService } from '../../tour-authoring/tour.service';
import { Router } from '@angular/router';

@Component({
  selector: 'xp-tourist-tours',
  templateUrl: './tourist-tours.component.html',
  styleUrls: ['./tourist-tours.component.css']
})
export class TouristToursComponent implements OnInit {
  loggedInUser: User;
  boughtTourIDs: number[] = [];

  AllTourExecutions: TourExecution[] = [];
  CompletedExecutions: TourExecution[] = [];
  AbondedExecutions: TourExecution[] = [];
  NotStartedTours: Tour[] = [];

  OnGoningTourExecution: TourExecution;
  OnGoingTour: Tour;

  NotStartedToursVisibility: boolean = true;
  CompletedToursVisibility: boolean = false;
  AbondedToursVisibility: boolean = false;

  constructor(private tourExecutionService:TourExecutionService,private authService: AuthService, private marketPlaceService :MarketplaceService,
              private tourService: TourService,private router: Router,
  ){}
ngOnInit(): void {
  this.authService.user$.subscribe((user) => {
    this.loggedInUser = user; 
  });

  this.getOnGoingTourExecution(this.loggedInUser.id);
  this.getTouristToursIDs(this.loggedInUser.id);
  this.getTourExecutions(this.loggedInUser.id);
}

getTouristToursIDs(id: number){
  this.marketPlaceService.getTouristToursIDs(id).subscribe((data: number[]) => {
    this.boughtTourIDs = data;
  }, error => {
    console.error('Error fetching tour IDs', error);}
  );}

getTourExecutions(id: number){
  this.tourExecutionService.getAllTouristTours(id).subscribe((data: TourExecution[]) =>{
    this.AllTourExecutions = data;
    this.getUnexecutedTours(this.boughtTourIDs, this.AllTourExecutions);
  },error =>{
    console.error('Error ocured durring fetching tour executions', error); }
  );}

getUnexecutedTours(tourIds: number[], executions: TourExecution[]){
  const executedToursIds =  new Set(executions.map(execution => execution.tourId));
  const ids = tourIds.filter(tourId => !executedToursIds.has(tourId));

  this.sortTourExecutions(this.AllTourExecutions);

  ids.forEach(element => {
    this.tourService.getByid(element).subscribe((data:Tour) =>{
        this.NotStartedTours.push(data);
        console.log(data)
    },error =>{
        console.error('Error occured during fetching not Started tour', error);
    })
  });
}

sortTourExecutions(Executions : TourExecution[]){
  Executions.forEach(element => { 
     if(element.status == 0) {
      this.CompletedExecutions.push(element);
     } else if (element.status == 1){
      this.AbondedExecutions.push(element);
     } 
  });
}

getOnGoingTourExecution(id : number){
  this.tourExecutionService.getOnGoingTour(id).subscribe((data: TourExecution) =>{
    this.OnGoningTourExecution = data;

    if(this.OnGoningTourExecution != null){
      this.getOnGoingTourInfo(this.OnGoningTourExecution.tourId);
    }
  },error =>{
    console.error('Error occured during fetching on going tour execution',error);
  })};

getOnGoingTourInfo(id : number){
  this.tourService.getByid(id).subscribe((data:Tour) =>{
    this.OnGoingTour = data;
},error =>{
    console.error('Error occured during fetching on going tour info', error);
})};

ShowTours(VisibilityOption: number){

  if(VisibilityOption == 1){
      this.NotStartedToursVisibility = true;
      this.AbondedToursVisibility = false;
      this.CompletedToursVisibility = false;

  } else if (VisibilityOption == 2){
    this.NotStartedToursVisibility = false;
      this.AbondedToursVisibility = true;
      this.CompletedToursVisibility = false;

  } else if (VisibilityOption == 3){
    this.NotStartedToursVisibility = false;
      this.AbondedToursVisibility = false;
      this.CompletedToursVisibility = true;
  }
}

StartTour(selectedTour : Tour){
  if(this.OnGoningTourExecution != null)
    alert("You can't start new Tour untill you finish on going one!")
  else{
    this.OnGoingTour = selectedTour;
    this.tourExecutionService.startNewTourExecution(selectedTour,this.loggedInUser.id).subscribe((data: TourExecution)=>{
        this.OnGoningTourExecution = data;
        alert("You have succesfully started a new tour!");
        this.NotStartedTours = this.NotStartedTours.filter(tour => tour.id !== selectedTour.id);
        this.getOnGoingTourExecution(this.loggedInUser.id);
    },error=>{
      console.error('Error occured while starting new tour!', error);
    });
  }
}
OpenTourExecution(ongoingExecution: TourExecution) {
  this.router.navigate(['completed-key-points'], {
    queryParams: {
      tourExecution: JSON.stringify(ongoingExecution),
      tour: JSON.stringify(this.OnGoingTour)
    }
  });
}

// Function to check if a tour's last activity is within the last 7 days
isLastActivityWithinWeek(lastActivity: Date): boolean {
  const lastActivityDate = new Date(lastActivity);
  const oneWeekAgo = new Date();
  oneWeekAgo.setDate(oneWeekAgo.getDate() - 7);
  return lastActivityDate >= oneWeekAgo;
}

loadTourData() {
  this.getOnGoingTourExecution(this.loggedInUser.id);
  this.getTouristToursIDs(this.loggedInUser.id);
  this.getTourExecutions(this.loggedInUser.id);
}

addReview(lastActivity: Date, tourId: number, completedPercentage: number) {
  this.router.navigate(['tour-review'], {
    queryParams: {
      lastActivity: lastActivity.toString(),
      tourId: tourId,
      completedPercentage: completedPercentage
    }
  });
}

addReport(tourId : number){
  this.router.navigate(['reporting-tour-problem/' + tourId]);
  }

}
