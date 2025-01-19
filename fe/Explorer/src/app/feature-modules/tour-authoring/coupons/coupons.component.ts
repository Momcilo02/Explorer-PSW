import { Component, OnInit, ViewChild } from '@angular/core';
import { CouponService } from '../coupon.service';
import { Coupon } from '../model/coupon.model';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { CouponsFormComponent } from '../coupons-form/coupons-form.component';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'xp-coupons',
  templateUrl: './coupons.component.html',
  styleUrls: ['./coupons.component.css']
})
export class CouponsComponent implements OnInit {
  @ViewChild('couponsForm') couponsForm!: CouponsFormComponent;
  
  coupons: Coupon[] = [];
  shouldRenderCouponsForm: boolean = false;
  shouldAdd: boolean = false;
  shouldEdit: boolean = false;
  selectedCoupon: Coupon;

  constructor(private couponService: CouponService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.getCoupons();
  }

  getCoupons() : void {
    this.couponService.getCoupons().subscribe({
      next: (result: PagedResults<Coupon>) => {
        this.coupons = result.results;
      },
      error:(err: any) =>{
        console.log(err)
      }
    })
  }

  onAddClicked() : void {
    this.shouldRenderCouponsForm = true;
    this.shouldEdit = false;
    this.shouldAdd = true;
  }

  onEditClicked(coupon: Coupon) : void {
    this.shouldRenderCouponsForm = true;
    this.selectedCoupon = coupon;
    this.shouldAdd = false;
    this.shouldEdit = true;

    setTimeout(() => {
      this.shouldRenderCouponsForm = true;
    });
  }

  deleteCoupon(coupon: Coupon) : void {
    const snackBarRef = this.snackBar.open('Are you sure you want to delete this bundle?', 'Confirm', {
      duration: 5000, 
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'info',
    });
    
    snackBarRef.onAction().subscribe(() => {
      this.couponService.deleteCoupon(coupon).subscribe({
        next: (_) => {
          window.location.reload();
        }
      })
    })
  }
  closeModal() {
    this.shouldRenderCouponsForm = false;
    this.getCoupons();
  }
}
