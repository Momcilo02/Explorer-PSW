import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EquipmentFormComponent } from './equipment-form/equipment-form.component';
import { EquipmentComponent } from './equipment/equipment.component';
import { MaterialModule } from 'src/app/infrastructure/material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { ReportedTourProblemsComponent } from './reported-tour-problems/reported-tour-problems.component';
import { ApplicationGradeListComponent } from './application-grade-list/application-grade-list.component';
import { ApplicationGradeService } from './application-grade.service';
import { ApplicationGradeFormComponent } from './application-grade-form/application-grade-form.component';  // Dodata linija za servis
import { FormsModule } from '@angular/forms';
import { SolvingDeadlineFormComponent } from './solving-deadline-form/solving-deadline-form.component';

import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthorRequestsComponent } from './author-requests/author-requests.component';
import { AccountsManagementComponent } from './accounts-management/accounts-management.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';



@NgModule({
  declarations: [
    EquipmentFormComponent,
    EquipmentComponent,
    ReportedTourProblemsComponent,
    ApplicationGradeListComponent,
    ApplicationGradeFormComponent,
    SolvingDeadlineFormComponent,
    AuthorRequestsComponent,
    AccountsManagementComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatFormFieldModule,
    MatTooltipModule,
    MatMenuModule
  ],
  providers: [ApplicationGradeService],  // Dodata linija u providers
  exports: [
    EquipmentComponent,
    EquipmentFormComponent,
    ReportedTourProblemsComponent
  ]
})
export class AdministrationModule { }
