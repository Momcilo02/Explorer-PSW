import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TourEquipmentComponent } from './tour-equipment/tour-equipment.component';
import { MatIconModule } from '@angular/material/icon';
import { KeyPointComponent } from './key-point/key-point.component';
import { KeyPointFormComponent } from './key-point-form/key-point-form.component';
import { TourComponent } from './tour/tour.component';
import { TourFormComponent } from './tour-form/tour-form.component';
import { AdministrationModule } from "../administration/administration.module";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from "../../shared/shared.module";
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ObjectComponent } from './object/object.component';
import { ObjectFormComponent } from './object-form/object-form.component';
import { ShowTourReviewsComponent } from './show-tour-reviews/show-tour-reviews.component';
import { MatMenuModule } from '@angular/material/menu';
import {MatChipsModule} from '@angular/material/chips';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { CouponsComponent } from './coupons/coupons.component';
import { CouponsFormComponent } from './coupons-form/coupons-form.component';
import { MatDatepicker, MatDatepickerModule } from '@angular/material/datepicker';
import { PublicKeyPointComponent } from './public-key-point/public-key-point.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CreateQuizComponent } from './create-quiz/create-quiz.component';
import { MatRadioModule } from '@angular/material/radio';
import { EditQuizComponent } from './edit-quiz/edit-quiz.component';

@NgModule({
  declarations: [
    TourEquipmentComponent,
    KeyPointComponent,
    KeyPointFormComponent,
    TourComponent,
    TourFormComponent,
    ObjectComponent,
    ObjectFormComponent,
    ShowTourReviewsComponent,
    CouponsComponent,
    CouponsFormComponent,
    PublicKeyPointComponent,
    CreateQuizComponent,
    EditQuizComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    AdministrationModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MaterialModule,
    MatSelectModule,
    MatButtonModule,
    SharedModule,
    RouterModule,
    MatMenuModule,
    MatCheckboxModule,
    MatRadioModule,
    MatDatepickerModule,
    CommonModule,
    MatChipsModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatTooltipModule,
    FormsModule,
],
  exports:[
    TourEquipmentComponent,
    KeyPointComponent,
    TourComponent,
    ObjectComponent

  ]
})
export class TourAuthoringModule { }
