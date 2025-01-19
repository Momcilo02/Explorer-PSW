import { Component, OnInit } from '@angular/core';
import { TourPreview } from '../model/tour-preview';
import { MarketplaceService } from '../marketplace.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Product } from '../model/product.model';
import { Bundle } from '../model/bundle.model';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'xp-bundle-form',
  templateUrl: './bundle-form.component.html',
  styleUrls: ['./bundle-form.component.css']
})
export class BundleFormComponent implements OnInit {
  tours: TourPreview[] = [];
  selectedTourIndices: number[] = [];
  totalPrice: number = 0.0;
  next: boolean = false;
  bundleForm = new FormGroup({
    name : new FormControl('', [Validators.required]),
    price : new FormControl(0, [Validators.required, Validators.min(0)])
  });
  products: Product[] = [];
  bundleId:number;
  bundle:Bundle;
  constructor(private service: MarketplaceService, private router: Router,private activatedRoute: ActivatedRoute,private snackBar: MatSnackBar){}

  ngOnInit(): void {
    this.service.getToursForAuthor().subscribe({
      next: (res: PagedResults<TourPreview>) =>{
        this.tours = res.results;
        this.loadBundle();
      }
  });
  }

  loadBundle(){
    this.bundleId = Number(this.activatedRoute.snapshot.paramMap.get('id'));

    if(this.bundleId === 0)
      return;

    this.service.getBundlesForAuthor().subscribe({
      next: (res: PagedResults<Bundle>) => {
        this.bundle = res.results.filter(b => b.id == this.bundleId)[0];
        console.log(this.bundle);
        this.bundleForm.patchValue({
          name: this.bundle.name,
          price: this.bundle.price
        })
        for(let p of this.bundle.products){
          this.selectedTourIndices.push(p.tourId);
          this.totalPrice+= this.tours.filter(t => t.id === p.tourId)[0].cost;
          this.products.push(p);
        }
      }
    });
  }

  toggleSelection(tour: any): void {
    if (this.selectedTourIndices.includes(tour.id)){
      this.selectedTourIndices = this.selectedTourIndices.filter(i => i !== tour.id);
      this.totalPrice-=tour.cost;
      this.products = this.products.filter(p => p.tourId !== tour.id);
    }
    else {
      this.selectedTourIndices.push(tour.id);
      this.totalPrice+=tour.cost;
      let product: Product =  {id: 0, tourId: tour.id, price: tour.cost};
      this.products.push(product);
    }
    
  }

  isSelected(index: number): boolean {
    return this.selectedTourIndices.includes(index);
  }
  nextOrPrevious(){
    if(!this.next && this.selectedTourIndices.length < 2){
      this.snackBar.open('You have to select 2 or more tours!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
      });
      return;
    }
    this.next = !this.next;
  }

  addOrEdit(){
    if(this.bundleId === 0)
      this.addBundle()
    else
      this.editBundle()
  }
  addBundle() {
    if(!this.bundleForm.valid || this.products.length === 0)
      return;
    let bundle:Bundle= {
      id: 0,
      name: this.bundleForm.value.name || "",
      price: this.bundleForm.value.price || 0.0,
      products: this.products,
      creatorId: 0,
      status: 0
    };

    this.service.createBundle(bundle).subscribe({
      next: () => {
        this.router.navigate(["bundle"])
        this.snackBar.open('Successfully added bundle!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      }
    })
  }

  editBundle(){
    if(!this.bundleForm.valid || this.products.length === 0)
      return;
    let b:Bundle= {
      id: this.bundle.id,
      name: this.bundleForm.value.name || "",
      price: this.bundleForm.value.price || 0.0,
      products: this.products,
      creatorId: this.bundle.creatorId,
      status: this.bundle.status
    };

    console.log(this.products.length)
    this.service.updateBundle(b).subscribe({
      next: () => {
        this.router.navigate(["bundle"])
        this.snackBar.open('Successfully updated bundle!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      }
    })
  }

  goToBundle(){
    this.router.navigate(["bundle"]);
  }

}
