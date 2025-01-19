import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { RouterModule } from '@angular/router';
import { TouristClubComponent } from './tourist-club/tourist-club.component';
import { TouristClubFormComponent } from './tourist-club-form/tourist-club-form.component';
import { ProfileAdministrationComponent } from './profile-administration/profile-administration.component';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { UserNotificationsModule } from '../user-notifications/user-notifications.module';
import { ProfileMessageComponent } from './profile-message/profile-message.component';
import { ProfileChatComponent } from './profile-chat/profile-chat.component';
import { ProfileMessageFormComponent } from './profile-message-form/profile-message-form.component';
import { ClubMessageFormComponent } from './club-message-form/club-message-form.component';
import { TouristClubChatComponent } from './tourist-club-chat/tourist-club-chat.component';
import { ClubMessageComponent } from './club-message/club-message.component';
import { LuckyWheelComponent } from './lucky-wheel/lucky-wheel.component';
import { TouristClubCardComponent } from './tourist-club-card/tourist-club-card.component';


@NgModule({
  declarations: [
    HomeComponent,
    NavbarComponent,
    TouristClubComponent,
    TouristClubFormComponent,
    ProfileAdministrationComponent,
    ProfileMessageComponent,
    ProfileChatComponent,
    ProfileMessageFormComponent,
    ClubMessageFormComponent,
    TouristClubChatComponent,
    ClubMessageComponent,
    LuckyWheelComponent,
    TouristClubCardComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatMenuModule,
    MatButtonModule,
    MatSidenavModule,
    FormsModule,
    UserNotificationsModule,

  ],
  exports: [
    NavbarComponent,
    HomeComponent,
    TouristClubComponent,
    TouristClubFormComponent,
    ProfileAdministrationComponent,
    LuckyWheelComponent,
  ]
})
export class LayoutModule { }
