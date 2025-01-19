import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Equipment } from '../../administration/model/equipment.model';
import { AdministrationService } from '../../administration/administration.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TourService } from '../tour.service';
import { ActivatedRoute } from '@angular/router';
import { Tour } from '../model/tour.model';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-tour-equipment',
  templateUrl: './tour-equipment.component.html',
  styleUrls: ['./tour-equipment.component.css']
})
export class TourEquipmentComponent implements OnInit {
  added_equipment: Equipment[] = [];
  all_equipment: Equipment[] = [];
  tourId: string;
  tourName: string;
  tourDescription: string;
  tour: Tour;
  @Input() enteredTourId: number;
  @Output() stopShowing= new EventEmitter<null>();
  constructor(private service: TourService, private route: ActivatedRoute, private router: Router,private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    // this.route.params.subscribe(params => {
    //   this.tourId = params['id'];
    //   this.getTour();
    // });
    this.getTour();
  }

  getEquipment(): void {
    this.service.getEquipments().subscribe({
      next: (res) =>{
        this.all_equipment = res.results.filter(equipment => {
          return !this.added_equipment.some(added => added.id === equipment.id);
        });
      }
    })
  }

  getTour(): void {
    this.service.getTours().subscribe({
      next: (res) =>{
        const tourIdNumber = Number(this.tourId);

        const tour = res.results.find((t: Tour) => t.id == this.enteredTourId);
        if (tour) {
          this.tour = tour;
          this.added_equipment = tour.equipments;
          this.tourName = tour.name;
          this.tourDescription = tour.description;
          this.getEquipment();
        }
      }
    })
  }

  deleteEquipment(equipment: Equipment): void {
    const index = this.added_equipment.findIndex(eq => eq.id === equipment.id);
    if (index !== -1) {
      const [removedEquipment] = this.added_equipment.splice(index, 1);
      this.all_equipment.push(removedEquipment);
      this.snackBar.open(`Equipment ${equipment.name} removed from tour, press save changes to update.`, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'info',
      });
    }
  }

  addEquipment(equipment: Equipment): void {
    const index = this.all_equipment.findIndex(eq => eq.id === equipment.id);
    if (index !== -1) {
      const [removedEquipment] = this.all_equipment.splice(index, 1);
      this.added_equipment.push(removedEquipment);
      this.snackBar.open(`Equipment ${equipment.name} added to tour, press save changes to update.`, 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'info',
      });
    }
  }

  updateTourEquipment(): void{
    this.tour.equipments = this.added_equipment;
    console.log("ulazim u metodu");
    console.log(this.tour);

    this.service.updatedTour(this.tour).subscribe({
      next: (response) => {
        this.router.navigate(["tour"]);
        console.log('Update successful:', response);
        console.log("DESAVA SE NESTO");
        this.stopShowing.emit();
        this.snackBar.open(`Update completed successfully.`, 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      }
    })

  }
}
