import { NgModule } from "@angular/core";
import { StartedEncounterComponent } from './started-encounter/started-encounter.component';
import { SharedModule } from "src/app/shared/shared.module";
import { CommonModule } from "@angular/common";
import { RankUpComponent } from "../user-notifications/rank-up/rank-up.component";


@NgModule({
  declarations: [
    StartedEncounterComponent,
    RankUpComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
],
  exports: [
    StartedEncounterComponent,
    RankUpComponent
  ]
})
export class EncounterExecutionModule { }
