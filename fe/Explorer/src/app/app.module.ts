import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './infrastructure/routing/app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './feature-modules/layout/layout.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './infrastructure/material/material.module';
import { AdministrationModule } from './feature-modules/administration/administration.module';
import { BlogModule } from './feature-modules/blog/blog.module';
import { MarketplaceModule } from './feature-modules/marketplace/marketplace.module';
import { TourAuthoringModule } from './feature-modules/tour-authoring/tour-authoring.module';
import { TourExecutionModule } from './feature-modules/tour-execution/tour-execution.module';
import { AuthModule } from './infrastructure/auth/auth.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { JwtInterceptor } from './infrastructure/auth/jwt/jwt.interceptor';
import { TouristClubComponent } from './feature-modules/layout/tourist-club/tourist-club.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, Routes } from '@angular/router';
import { ApplicationGradeListComponent } from './feature-modules/administration/application-grade-list/application-grade-list.component';
import { ApplicationGradeFormComponent } from './feature-modules/administration/application-grade-form/application-grade-form.component';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt'; // Added JWT_OPTIONS
import 'leaflet-routing-machine';
import { MatIconModule } from '@angular/material/icon';
import { LMarkdownEditorModule } from 'ngx-markdown-editor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EncounterAuthoringModule } from './feature-modules/encounter-authoring/encounter-authoring.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { EncounterExecutionModule } from './feature-modules/encounter-execution/encounter-execution.module';


const routes: Routes = [
  { path: 'grades', component: ApplicationGradeListComponent },
  { path: 'grades/add', component: ApplicationGradeFormComponent }, // Path for adding a grade
  // Add other routes as needed
];


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    FlexLayoutModule,
    BrowserAnimationsModule,
    MaterialModule,
    AdministrationModule,
    BlogModule,
    MarketplaceModule,
    TourAuthoringModule,
    TourExecutionModule,
    AuthModule,
    HttpClientModule,
    LMarkdownEditorModule,
    FormsModule,
    ReactiveFormsModule,

    RouterModule.forRoot(routes), // Added routing,


    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    EncounterAuthoringModule,
    EncounterExecutionModule,

  ],
  providers: [
    {
      provide: JWT_OPTIONS,
      useValue: JWT_OPTIONS, // Added for JWT options
    },
    JwtHelperService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
