import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TouristEquipmentComponent } from './tourist-equipment/tourist-equipment.component';
import { MatIconModule } from '@angular/material/icon';
import { ReportingTourProblemComponent } from './reporting-tour-problem/reporting-tour-problem.component';
import { TourReviewComponent } from './tour-review/tour-review.component';
import { TourReviewFormComponent } from './tour-review-form/tour-review-form.component';
import { AdministrationModule } from "../administration/administration.module";
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CompletedKeyPointComponent } from './completed-key-point/completed-key-point.component';
import { PositionSimulatorComponent } from './position-simulator/position-simulator.component';
import { SharedModule } from "../../shared/shared.module";
import { MapComponent } from 'src/app/shared/map/map.component';
import { TouristToursComponent } from './tourist-tours/tourist-tours.component';


@NgModule({
  declarations: [
    ReportingTourProblemComponent,
    TourReviewComponent,
    TourReviewFormComponent,
    TouristEquipmentComponent,
    CompletedKeyPointComponent,
    PositionSimulatorComponent,
    TouristToursComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatOptionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    AdministrationModule,
    ReactiveFormsModule,
    MaterialModule,
    SharedModule
],
  exports: [
    ReportingTourProblemComponent,
    TourReviewComponent,
    TouristEquipmentComponent,
    CompletedKeyPointComponent,
    PositionSimulatorComponent,
    TouristToursComponent
  ]

})
export class TourExecutionModule { }
