import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { environment } from 'src/env/environment';
import { BehaviorSubject, filter, Observable, tap } from 'rxjs';
import { OrderItem } from './model/order-item.model';
import { ShoppingCart } from './model/shopping-cart.model';
import { Tour } from '../tour-authoring/model/tour.model';
import { TourPreview } from './model/tour-preview';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TourPurchaseToken } from './model/tourPurchasetoken.model';
import { TouristWallet } from './model/tourist-wallet.model';
import { PaymentRecord } from './model/payment-record.model';
import { Bundle } from './model/bundle.model';
import { User } from '../../infrastructure/auth/model/user.model';
import { AuthService } from '../../infrastructure/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class MarketplaceService {
  [x: string]: any;
  currentUser: User | null = null;
  private cartItemCountSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  cartItemCount$ = this.cartItemCountSubject.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) {
    this.authService.user$
    .pipe(
      filter((user): user is User => user.username !== ""), // Čekaj da korisnik bude definisan
      tap((user) => {
        const initialCount = this.getInitialCount(user.username);
        this.cartItemCountSubject.next(initialCount); // Postavi inicijalnu vrednost
      })
    )
    .subscribe();
  }

  getShoppingCart(touristId: number): Observable<ShoppingCart> {
    const params = new HttpParams().set('touristId', touristId.toString());
    return this.http.get<ShoppingCart>(environment.apiHost + 'shopping/shopping-cart/' + touristId);
  }

  addItemToShoppingCart(item: OrderItem,touristId: number): Observable<ShoppingCart> {
    return this.http.put<ShoppingCart>(environment.apiHost + 'shopping/shopping-cart/add/' +touristId, item);
  }
  getPaymentRecords(touristId: number): Observable<PaymentRecord[]> {
    return this.http.get<PaymentRecord[]>(environment.apiHost + `shopping/shopping-cart/payment-records/${touristId}`);
  }
  clearPaymentRecords(touristId: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiHost}shopping/shopping-cart/payment-records/${touristId}`);
  }

  public createItem(tour: Tour): Observable<OrderItem> {
    return this.http.post<OrderItem>(environment.apiHost + 'shopping/shopping-cart/create/item', tour);
  }
  public createCart(touristId: number): Observable<ShoppingCart> {
    return this.http.post<ShoppingCart>(
      `${environment.apiHost}shopping/shopping-cart/create/${touristId}`,
      null,
      { responseType: 'json' }
    );
  }
  public createWallet(touristId: number): Observable<TouristWallet> {
    return this.http.post<TouristWallet>(
      `${environment.apiHost}tourist/wallet/create-wallet/${touristId}`,
      null,
      { responseType: 'json' }
    );
  }
  removeItemFromShoppingCart(item: OrderItem,touristId: number): Observable<ShoppingCart> {
    return this.http.put<ShoppingCart>(environment.apiHost + 'shopping/shopping-cart/remove/'+touristId, item);
  }

  shoppingCartCheckOut(touristId: number): Observable<ShoppingCart> {
    const url = `https://localhost:44333/api/shopping/shopping-cart/checkout/${touristId}`;
    return this.http.put<ShoppingCart>(url, {});
  }

  getPublishedTours() : Observable<PagedResults<TourPreview>>{
    return this.http.get<PagedResults<TourPreview>>('https://localhost:44333/api/tours/published');
  }
  getPublishedTourPreview(id:number): Observable<TourPreview> {
    return this.http.get<TourPreview>('https://localhost:44333/api/tours/published/preview/' + id);
  }

  getPurchasedTour(id:number): Observable<TourPreview> {
    return this.http.get<TourPreview>('https://localhost:44333/api/tours/published/' + id);
  }
  getPurchasedTours(userId: number): Observable<TourPreview[]> {
    return this.http.get<TourPreview[]>('https://localhost:44333/api/shopping/shopping-cart/purchased/'+userId);
  }
  
  
  // private cartItemCountSubject = new BehaviorSubject<number>(this.getInitialCount(this.authService.user$.getValue()?.username || ""));
  // cartItemCount$ = this.cartItemCountSubject.asObservable();

  private getInitialCount(username: string): number {
    return parseInt(localStorage.getItem(username) || '0', 10);
  }

  addItem(username: string): void {
    const newCount = this.getInitialCount(username) + 1;
    localStorage.setItem(username, newCount.toString());
    this.cartItemCountSubject.next(newCount);
  }

  removeItem(username: string): void {
    const newCount = Math.max(this.getInitialCount(username) - 1, 0);
    localStorage.setItem(username, newCount.toString());
    this.cartItemCountSubject.next(newCount);
  }
  
  clearCart(username: string): void{
    const newCount = 0;
    localStorage.setItem(username, newCount.toString());
    this.cartItemCountSubject.next(newCount);
  }

  tokenExists(tourId: number, userId: number): Observable<boolean>{
    return this.http.get<boolean>('https://localhost:44333/api/shopping/tour-purchase-token/'+tourId + '/' + userId)
  }

  getTouristToursIDs(touristId: number): Observable<number[]>{
    return this.http.get<number[]>('https://localhost:44333/api/shopping/tour-purchase-token/mytours/' + touristId)
  }
  getAdventureCoins(id:number): Observable<TouristWallet> {
    return this.http.get<TouristWallet>(environment.apiHost + 'tourist/wallet/get-adventure-coins/' + id)
  }

  paymentAdventureCoins(id:number, coins: number): Observable<TouristWallet> {
    return this.http.put<TouristWallet>(environment.apiHost + 'tourist/wallet/payment-adventure-coins/' + id + '/' + coins, null)
  }

  searchTours(distance: number|undefined, latitude: number|undefined, longitude: number|undefined): Observable<TourPreview[]>{
    return this.http.get<TourPreview[]>(environment.apiHost + `tours/search?latitude=${latitude}&longitude=${longitude}&distance=${distance}`);
  }

  getToursForAuthor(): Observable<PagedResults<TourPreview>>{
    return this.http.get<PagedResults<TourPreview>>(environment.apiHost + 'administration/tour/user');
  }

  createBundle(bundle: Bundle): Observable<Bundle>{
    return this.http.post<Bundle>(environment.apiHost + "administration/bundle", bundle);
  }

  getBundlesForAuthor(): Observable<PagedResults<Bundle>>{
    return this.http.get<PagedResults<Bundle>>(environment.apiHost + 'administration/bundle');
  }

  deleteBundle(id:number){
    return this.http.delete(environment.apiHost + 'administration/bundle/'+ id)
  }

  getTour(id:number): Observable<TourPreview>{
    return this.http.get<TourPreview>(environment.apiHost + 'administration/tour/'+ id);
  }

  updateBundle(bundle: Bundle): Observable<Bundle>{
    return this.http.put<Bundle>(environment.apiHost + `administration/bundle/${bundle.id}`, bundle);
  }

  publishBundle(bundle: Bundle): Observable<Bundle>{
    return this.http.put<Bundle>(environment.apiHost + `administration/bundle/publish/${bundle.id}`, bundle);
  }

  archiveBundle(bundle: Bundle): Observable<Bundle>{
    return this.http.put<Bundle>(environment.apiHost + `administration/bundle/archive/${bundle.id}`, bundle);
  }

  getBundlesForUsers(): Observable<PagedResults<Bundle>>{
    return this.http.get<PagedResults<Bundle>>(environment.apiHost + 'shopping/bundle');
  }

  createBundleItem(bundle: Bundle): Observable<OrderItem> {
    return this.http.post<OrderItem>(environment.apiHost + 'shopping/shopping-cart/create/item/bundle', bundle);
  }
}
