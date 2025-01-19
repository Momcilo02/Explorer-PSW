import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { TourOverviewComponent } from './tour-overview/tour-overview.component';
import { TourOverviewDetailsComponent } from './tour-overview-details/tour-overview-details.component';
import { TourAuthoringModule } from '../tour-authoring/tour-authoring.module';
import { TourKeypointPreviewComponent } from './tour-keypoint-preview/tour-keypoint-preview.component';
import { SharedModule } from "../../shared/shared.module";
import {  FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BundleComponent } from './bundle/bundle.component';
import { BundleFormComponent } from './bundle-form/bundle-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenu, MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { ReceiptComponent } from './receipt/receipt.component';
import {MatTooltipModule} from '@angular/material/tooltip';
@NgModule({
  declarations: [
    ShoppingCartComponent,
    TourOverviewComponent,
    TourOverviewDetailsComponent,
    TourKeypointPreviewComponent,
    BundleComponent,
    BundleFormComponent,
    ReceiptComponent
  ],
  imports: [
    CommonModule,
    TourAuthoringModule,
    SharedModule,
    FormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatIconModule,
    MatTooltipModule
]
})
export class MarketplaceModule { }
