import { Component, OnInit } from '@angular/core';
import { Object } from '../../administration/model/object.model';
import { AdministrationService } from '../administration.service';
import { KeyPoint } from '../model/keyPoint.model';
import { UserNotificationsService } from '../../user-notifications/user-notifications.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-author-requests',
  templateUrl: './author-requests.component.html',
  styleUrls: ['./author-requests.component.css']
})
export class AuthorRequestsComponent implements OnInit {

  constructor (private administrationService:AdministrationService,private snackBar: MatSnackBar){}

  objects: Object[]=[];
  keyPoints: KeyPoint[]=[];
  selectedObject: Object | null = null;
  selectedKeyPoint: KeyPoint | null = null;
  actionType: string = ''; 
  commentText: string = ''; 

  ngOnInit(): void {
    this.getObjects();
    this.getKeyPoints();
  }

  showCommentForm(item: any, action: string,instance: string): void {
    if (instance=== 'object') {
      this.selectedObject = item;
    } else {
      this.selectedKeyPoint = item;
    }
    this.actionType = action;
    this.commentText = ''; 
  }

  submitComment(item: any,instance:string): void {
    if (this.actionType === 'refuse') {
      if(instance ==='object')
      {  
        this.refuseRequestObject(item); 
      }
      else
        this.refuseRequestKeyPoint(item);
    }
    this.resetForm();
  }

  resetForm(): void {
    this.selectedObject = null;
    this.selectedKeyPoint = null;
    this.actionType = '';
    this.commentText = '';
  }

getObjects(){
  this.administrationService.getObjects().subscribe({
    next: (result)=>{
      this.objects = result.results.filter(ob=>ob.status===1);
    },error: (err)=>{
      this.snackBar.open('Error in retrieving objects: ' + err.status, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
    }
  })
}

getKeyPoints()
{
  this.administrationService.getKeyPoints().subscribe({
    next: (result)=>{
      this.keyPoints = result.filter(kp=>kp.status===1);
    },error: (err)=>{
      this.snackBar.open('Error in retrieving key points: ' + err.status, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
    }
  });
}

acceptRequestObject(ob:Object): void{
  ob.status=2;
  this.administrationService.updateObjectStatus(ob).subscribe({
    next: ()=>
    {
      this.getObjects();
      this.snackBar.open('Object request accepted!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'success',
      });
    },error: (err)=>{
      this.snackBar.open('Error in accepting the request: ' + err.status, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
    }
  });
}

refuseRequestObject(ob:Object): void
{
  ob.status=0;
  ob.comment=this.commentText;
  this.administrationService.updateObjectStatus(ob).subscribe({
    next:()=>{
      this.getObjects();
      this.snackBar.open('Object request refused!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'info',
      });
    },error: (err)=>{
      this.snackBar.open('Error in rejecting the request: ' + err.status, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
    }
  });
 }

acceptRequestKeyPoint(k: KeyPoint):void {
  k.status=2;
  this.administrationService.updateKeyPointStatus(k).subscribe({
    next:()=>{
      this.getKeyPoints();
      this.snackBar.open('KeyPoint request accepted!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'success',
      });
    },error: (err)=>{
      this.snackBar.open('Error in accepting the request: ' + err.status, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
    }
  });
}

refuseRequestKeyPoint(k:KeyPoint): void{
k.status=0;
k.comment = this.commentText;
this.administrationService.updateKeyPointStatus(k).subscribe({
  next:()=>{
    this.getKeyPoints();
    this.snackBar.open('KeyPoint request refused!', 'Close', {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: 'info',
    });
  },error: (err)=>{
    this.snackBar.open('Error in rejecting the request: ' + err.status, 'Close', {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: 'error',
    });
  }
});
}

getStatus(st: number) : string {
  if(st === 0)
    return "Private";
  else if(st === 1)
    return "Requested";
  else
    return "Public";
}

getCategory(cat: number) : string {
  if(cat === 0)
    return "WC";
  else if(cat === 1)
    return "Restaurant";
  else if(cat==2)
    return "Parking";
  else return "Other";
}
}
