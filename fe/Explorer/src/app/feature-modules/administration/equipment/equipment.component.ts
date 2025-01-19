import { Component, OnInit } from '@angular/core';
import { AdministrationService } from '../administration.service';
import { Equipment } from '../model/equipment.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-equipment',
  templateUrl: './equipment.component.html',
  styleUrls: ['./equipment.component.css']
})
export class EquipmentComponent implements OnInit {
  equipment: Equipment[] = [];
  // equipment: Equipment[] = [{id: 0, name: 'Patike', description: 'Nepromocive patike'},
  //   {id: 1, name: 'Flasica vode', description: 'Bilo koje osvezavajuce pice'}
  // ]
  selectedEquipment: Equipment;
  shouldRenderEquipmentForm: boolean = false;
  shouldEdit: boolean = false;

  constructor(private service: AdministrationService,private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.getEquipment();
  }

  deleteEquipment(id: number): void {
    const snackBarRef = this.snackBar.open('Are you sure you want to delete this equipment?', 'Confirm', {
      duration: 5000, 
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: 'info',
    });
  
    snackBarRef.onAction().subscribe(() => {
    this.service.deleteEquipment(id).subscribe({
      next: () => {
        this.getEquipment();
        this.snackBar.open('Deleted equipment!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'info',
          
        });
      },
    }) 
  });
  }

  getEquipment(): void {
    this.service.getEquipment().subscribe({
      next: (result: PagedResults<Equipment>) => {
        this.equipment = result.results;
      },
      error: () => {
      }
    })
    this.shouldRenderEquipmentForm = false;
  }

  onEditClicked(equipment: Equipment): void {
    this.selectedEquipment = equipment;
    this.shouldRenderEquipmentForm = true;
    this.shouldEdit = true;
  }

  onAddClicked(): void {
    this.shouldEdit = false;
    this.shouldRenderEquipmentForm = true;
  }
  closeModal() {
    this.shouldRenderEquipmentForm = false;
  }
}
