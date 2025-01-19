import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from 'src/app/feature-modules/layout/home/home.component';
import { LoginComponent } from '../auth/login/login.component';
import { EquipmentComponent } from 'src/app/feature-modules/administration/equipment/equipment.component';
import { AuthGuard } from '../auth/auth.guard';
import { ShoppingCartComponent } from 'src/app/feature-modules/marketplace/shopping-cart/shopping-cart.component';
import { RegistrationComponent } from '../auth/registration/registration.component';
import { TourEquipmentComponent } from 'src/app/feature-modules/tour-authoring/tour-equipment/tour-equipment.component';
import { TouristClubComponent } from 'src/app/feature-modules/layout/tourist-club/tourist-club.component';
import { KeyPointComponent } from '../../feature-modules/tour-authoring/key-point/key-point.component';
import { TourReviewComponent } from 'src/app/feature-modules/tour-execution/tour-review/tour-review.component';
import { BlogsComponent } from 'src/app/feature-modules/blog/blogs/blogs.component';
import { ReportedTourProblemsComponent } from 'src/app/feature-modules/administration/reported-tour-problems/reported-tour-problems.component';
import { ReportingTourProblemComponent } from 'src/app/feature-modules/tour-execution/reporting-tour-problem/reporting-tour-problem.component';
import { TourComponent } from 'src/app/feature-modules/tour-authoring/tour/tour.component';
import { TourOverviewComponent } from 'src/app/feature-modules/marketplace/tour-overview/tour-overview.component';
import { TourOverviewDetailsComponent } from 'src/app/feature-modules/marketplace/tour-overview-details/tour-overview-details.component';
import { ProfileAdministrationComponent } from 'src/app/feature-modules/layout/profile-administration/profile-administration.component';
import { ObjectComponent } from 'src/app/feature-modules/tour-authoring/object/object.component';
import { CommentComponent } from 'src/app/feature-modules/blog/comment/comment.component';
import { ShowTourReviewsComponent } from 'src/app/feature-modules/tour-authoring/show-tour-reviews/show-tour-reviews.component';
import { TouristEquipmentComponent } from 'src/app/feature-modules/tour-execution/tourist-equipment/tourist-equipment.component';
import { PositionSimulatorComponent } from 'src/app/feature-modules/tour-execution/position-simulator/position-simulator.component';
import { CompletedKeyPointComponent } from 'src/app/feature-modules/tour-execution/completed-key-point/completed-key-point.component';
import { TouristToursComponent } from 'src/app/feature-modules/tour-execution/tourist-tours/tourist-tours.component';
import { EncountersComponent } from 'src/app/feature-modules/encounter-authoring/encounters/encounters.component';
import { AccountsManagementComponent } from 'src/app/feature-modules/administration/accounts-management/accounts-management.component';
import { ProfileChatComponent } from 'src/app/feature-modules/layout/profile-chat/profile-chat.component';
import { CouponsComponent } from 'src/app/feature-modules/tour-authoring/coupons/coupons.component';
import { AuthorRequestsComponent } from 'src/app/feature-modules/administration/author-requests/author-requests.component';
import { EncounterViewComponent } from 'src/app/feature-modules/encounter-authoring/encounter-view/encounter-view.component';
import { StartedEncounterComponent } from 'src/app/feature-modules/encounter-execution/started-encounter/started-encounter.component';
import { BundleComponent } from 'src/app/feature-modules/marketplace/bundle/bundle.component';
import { BundleFormComponent } from 'src/app/feature-modules/marketplace/bundle-form/bundle-form.component';
import { PublicKeyPointComponent } from 'src/app/feature-modules/tour-authoring/public-key-point/public-key-point.component';
import { ReceiptComponent } from 'src/app/feature-modules/marketplace/receipt/receipt.component';
import { LuckyWheelComponent } from 'src/app/feature-modules/layout/lucky-wheel/lucky-wheel.component';
import { TouristClubChatComponent } from 'src/app/feature-modules/layout/tourist-club-chat/tourist-club-chat.component';
import { CreateQuizComponent } from 'src/app/feature-modules/tour-authoring/create-quiz/create-quiz.component';
import { EditQuizComponent } from 'src/app/feature-modules/tour-authoring/edit-quiz/edit-quiz.component';
const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegistrationComponent},
  {path: 'equipment', component: EquipmentComponent, canActivate: [AuthGuard],},
  {path: 'tour-equipment/:id', component: TourEquipmentComponent},
  {path: 'blogs', component: BlogsComponent},
  { path: 'receipt', component: ReceiptComponent },
  {path: 'touristClub', component: TouristClubComponent },
  {path: ':id/keypoint', component: KeyPointComponent, canActivate: [AuthGuard]},
  {path: 'tour-review', component: TourReviewComponent},
  {path: 'blogs', component: BlogsComponent},
  {path: 'account-details',component:AccountsManagementComponent},
  {path: 'blogs/:blogId/comments', component: CommentComponent},
  {path: 'coupons', component: CouponsComponent, canActivate: [AuthGuard]},
  {path: 'comment', component: CommentComponent},
  {path: 'show-tour-reviews/:id', component: ShowTourReviewsComponent},
  {path: 'lucky-wheel', component:LuckyWheelComponent},
  {path: 'edit-quiz/:tourId', component:EditQuizComponent},
  {path: 'position-simulator', component: PositionSimulatorComponent},
  {path: 'reported-tour-problems', component: ReportedTourProblemsComponent, canActivate: [AuthGuard],},
  {path: 'reporting-tour-problem/:id', component: ReportingTourProblemComponent, canActivate: [AuthGuard],},
  {path: 'tour', component: TourComponent},
  {path: 'profile-info', component: ProfileAdministrationComponent, canActivate: [AuthGuard]},
  {path: 'object', component: ObjectComponent, canActivate: [AuthGuard]},
  {path: 'profile-info', component: ProfileAdministrationComponent, canActivate: [AuthGuard],},
  {path: 'tourist-equipment', component: TouristEquipmentComponent},
  { path: 'tour-overview', component: TourOverviewComponent, canActivate:[AuthGuard]},
  { path: 'tour-overview-details/:id', component:TourOverviewDetailsComponent, canActivate: [AuthGuard]},
  { path: 'shopping-cart', component: ShoppingCartComponent, canActivate: [AuthGuard], },
  { path: 'completed-key-points', component: CompletedKeyPointComponent, canActivate: [AuthGuard]},
  { path: 'tourist-tours', component: TouristToursComponent, canActivate:[AuthGuard]},
  { path: 'encounters', component: EncountersComponent},
  { path: 'profile-chat/:id', component: ProfileChatComponent},
  { path: 'authorRequests' , component:AuthorRequestsComponent},
  { path: 'encounter-view', component: EncounterViewComponent},
  { path: 'started-encounter', component: StartedEncounterComponent},
  {path: ':id/publickeypoint', component: PublicKeyPointComponent, canActivate: [AuthGuard]},
  { path: 'create-quiz/:tourId', component: CreateQuizComponent },
  { path: 'bundle', component: BundleComponent},
  { path: 'create-bundle/:id', component: BundleFormComponent},
  { path: 'tourist-club-chat/:touristClubId/:touristClubName/:ownerId', component: TouristClubChatComponent, canActivate:[AuthGuard]},
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
