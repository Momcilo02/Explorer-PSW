import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { CouponService } from '../coupon.service';
import { Tour } from '../model/tour.model';
import { TourService } from '../tour.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { Coupon } from '../model/coupon.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { add } from 'date-fns';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-coupons-form',
  templateUrl: './coupons-form.component.html',
  styleUrls: ['./coupons-form.component.css']
})
export class CouponsFormComponent implements OnInit, OnChanges {

  minDate: Date;
  user: User;
  @Output() couponUpdated = new EventEmitter<null>();
  @Input() selectedCoupon: Coupon;
  @Input() shouldEdit: boolean=false;
  @Input() shouldAdd: boolean=true;
  tours: Tour[];
  isChecked: boolean = false;

  couponsForm = new FormGroup({
    identifier: new FormControl('', [Validators.required]),
    percentage: new FormControl(10, [Validators.required]),
    expirationDate: new FormControl<Date | null>(null, [Validators.required, this.futureDateValidator()]),
    toursEligible: new FormControl(),
    areAllToursIncluded: new FormControl(false)
  })

  constructor(private couponService: CouponService, 
    private tourService: TourService,
    private authService: AuthService,
    private snackBar: MatSnackBar) {
    this.minDate = new Date();
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log('ovo je kupon koji se apdejtuje: ', this.selectedCoupon);
    this.couponsForm.reset();
    this.tourService.getMyTours().subscribe({
      next: (result: PagedResults<Tour>) => {
        this.tours = result.results;
        this.setFormFields();
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  setFormFields(){
        
    if(this.shouldEdit){
      if(this.selectedCoupon!.toursEligible.length === this.tours.length){
        this.couponsForm.patchValue({
          identifier: this.selectedCoupon.identifier,
          percentage: this.selectedCoupon.percentage,
          expirationDate: this.selectedCoupon.expirationDate,
          toursEligible: this.selectedCoupon.toursEligible,
          areAllToursIncluded: true
        })
      }else{
        this.couponsForm.patchValue({
          identifier: this.selectedCoupon.identifier,
          percentage: this.selectedCoupon.percentage,
          expirationDate: this.selectedCoupon.expirationDate,
          toursEligible: this.selectedCoupon.toursEligible,
          areAllToursIncluded: false
        })
      }
    }else if(this.shouldAdd){
      this.couponsForm.patchValue({
          identifier: "",
          percentage: 10,
          expirationDate: null,
          toursEligible: null,
          areAllToursIncluded: false
      })
    }
  }

  ngOnInit(): void {
    this.getTours();
    this.authService.user$.subscribe((user) => {
      this.user = user;
    });
  }

  generateIdentifier(length: number = 8): void {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * characters.length);
        result += characters[randomIndex];
    }
    this.couponsForm.patchValue({
      identifier: result
    })
}

  futureDateValidator(): ValidatorFn {
    return (control: AbstractControl) => {
        const today = new Date();
        const inputDate = new Date(control.value);

        return inputDate > today ? null : { invalidDate: true };
    };
  }

  getTours() : void {
    this.tourService.getMyTours().subscribe({
      next: (result: PagedResults<Tour>) => {
        this.tours = result.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  addCoupon() : void {
    if(this.couponsForm.invalid){
      console.log(this.couponsForm.value);
      this.snackBar.open('Fill in all fields', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
      return;
    }
    const tourss = Array.isArray(this.couponsForm.value.toursEligible)
      ? this.couponsForm.value.toursEligible
      : [this.couponsForm.value.toursEligible];
    const addingCoupon: Coupon = {
      id: 0,
      identifier: this.couponsForm.value.identifier || "",
      percentage: this.couponsForm.value.percentage || 0,
      authorId: this.user.id,
      expirationDate: new Date(this.couponsForm.value.expirationDate || ""),
      toursEligible: tourss || [],
      couponStatus: 0
    }
    if(this.couponsForm.value.areAllToursIncluded){
      addingCoupon.toursEligible = [];
      for(let t of this.tours){
        addingCoupon.toursEligible.push(t.id || 0);
      }
    }

    console.log('Ovo je addingCoupon: ', addingCoupon);
    this.couponService.addCoupon(addingCoupon).subscribe({
      next: (result: Coupon) => {
        this.couponService.getCoupons();
        this.shouldAdd = false;
        this.couponsForm.reset();
        this.couponUpdated.emit();
        this.snackBar.open('Successfully added coupon!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  updateCoupon() : void {
    if(this.couponsForm.invalid){
      console.log(this.couponsForm.value);
      this.snackBar.open('Fill in all fields', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
      return;
    }

    const tourss = Array.isArray(this.couponsForm.value.toursEligible)
      ? this.couponsForm.value.toursEligible
      : [this.couponsForm.value.toursEligible];

    const updatingCoupon: Coupon = {
      id: this.selectedCoupon.id,
      identifier: this.couponsForm.value.identifier || "",
      percentage: this.couponsForm.value.percentage || 0,
      authorId: this.user.id,
      expirationDate: new Date(this.couponsForm.value.expirationDate || ""),
      toursEligible: tourss || [],
      couponStatus: 0
    }
    if(this.couponsForm.value.areAllToursIncluded){
      updatingCoupon.toursEligible = [];
      for(let t of this.tours){
        updatingCoupon.toursEligible.push(t.id || 0);
      }
    }

    console.log('Ovo je addingCoupon: ', updatingCoupon);
    this.couponService.updateCoupon(updatingCoupon).subscribe({
      next: (result: Coupon) => {
        // this.couponService.getCoupons();
        this.shouldAdd = false;
        this.couponsForm.reset();
        this.couponUpdated.emit();
        this.snackBar.open('Successfully updated coupon!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  cancel(){
    this.couponUpdated.emit();
  }
}
