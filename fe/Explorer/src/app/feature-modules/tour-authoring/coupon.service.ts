import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Coupon } from './model/coupon.model';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { environment } from 'src/env/environment';

@Injectable({
    providedIn: 'root'
  })
  export class CouponService {

    constructor(private http: HttpClient) {}

    getCoupons() : Observable<PagedResults<Coupon>>{
      return this.http.get<PagedResults<Coupon>>(environment.apiHost + 'coupons');
    }

    deleteCoupon(coupon: Coupon) : Observable<Coupon> {
      return this.http.delete<Coupon>(environment.apiHost + 'coupons/' + coupon.id);
    }

    addCoupon(coupon: Coupon) : Observable<Coupon> {
      return this.http.post<Coupon>(environment.apiHost + 'coupons', coupon);
    }

    updateCoupon(coupon: Coupon) : Observable<Coupon> {
      return this.http.put<Coupon>(environment.apiHost + `coupons/${coupon.id}`, coupon);
    }
  }