import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationComponent } from './notification/notification.component';
import { RankUpComponent } from './rank-up/rank-up.component';



@NgModule({
  declarations: [
    NotificationComponent,
    //RankUpComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NotificationComponent
  ]
})
export class UserNotificationsModule { }
