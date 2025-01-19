import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MapComponent } from './map/map.component';


import { Routes } from '@angular/router';
import { TouristClubComponent } from '../feature-modules/layout/tourist-club/tourist-club.component';



@NgModule({
  declarations: [
    MapComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [MapComponent]
})
export class SharedModule { }
