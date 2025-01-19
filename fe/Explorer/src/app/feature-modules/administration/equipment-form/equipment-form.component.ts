import { Component, EventEmitter, Inject, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Equipment } from '../model/equipment.model';
import { AdministrationService } from '../administration.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'xp-equipment-form',
  templateUrl: './equipment-form.component.html',
  styleUrls: ['./equipment-form.component.css']
})
export class EquipmentFormComponent implements OnChanges {

  @Output() equimpentUpdated = new EventEmitter<null>();
  @Input() equipment: Equipment;
  @Input() shouldEdit: boolean = false;

  constructor(private service: AdministrationService,private snackBar: MatSnackBar) {
  }

  ngOnChanges(): void {
    this.equipmentForm.reset();
    if(this.shouldEdit) {
      this.equipmentForm.patchValue(this.equipment);
    }
  }

  equipmentForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
  });

  addEquipment(): void {
    if(this.equipmentForm.invalid){
      this.snackBar.open('Fill in all fields!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
        
      });
      return;
    }
    const equipment: Equipment = {
      name: this.equipmentForm.value.name || "",
      description: this.equipmentForm.value.description || "",
    };
    this.service.addEquipment(equipment).subscribe({
      next: () => { 
        this.equimpentUpdated.emit();
        this.snackBar.open('Successfully added equipment!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      }
      
    });
  }

  updateEquipment(): void {
    if(this.equipmentForm.invalid){
      this.snackBar.open('Fill in all fields!', 'Close', {
        duration: 3000,
        verticalPosition: 'top',
        panelClass: 'error',
        
      });
      return;
    }
    const equipment: Equipment = {
      name: this.equipmentForm.value.name || "",
      description: this.equipmentForm.value.description || "",
    };
    equipment.id = this.equipment.id;
    this.service.updateEquipment(equipment).subscribe({
      next: () => { 
        this.equimpentUpdated.emit();
        this.snackBar.open('Successfully updated equipment!', 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: 'success',
        });
      }
    });
  }
}
