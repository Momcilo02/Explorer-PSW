import { NgModule } from "@angular/core";
import { EncountersComponent } from './encounters/encounters.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from "src/app/shared/shared.module";
import { EncounterViewComponent } from './encounter-view/encounter-view.component';


@NgModule({
  declarations: [
    EncountersComponent,
    EncounterViewComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
],
  exports: [
    EncountersComponent,
    EncounterViewComponent
  ]
})
export class EncounterAuthoringModule { }
