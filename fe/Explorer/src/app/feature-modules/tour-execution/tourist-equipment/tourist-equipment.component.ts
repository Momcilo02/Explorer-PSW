import { Component, OnInit } from '@angular/core';
import { Equipment } from '../../administration/model/equipment.model';
import { AdministrationService } from '../../administration/administration.service';
import { TourExecutionService } from '../tour-execution.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { TouristEquipment } from '../model/tourist-equipment.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'xp-tourist-equipment',
  templateUrl: './tourist-equipment.component.html',
  styleUrls: ['./tourist-equipment.component.css']
})
export class TouristEquipmentComponent implements OnInit {
  equipment: Equipment[] = [];
  touristEquipment: TouristEquipment[] = [];
  touristEquipmentSource: TouristEquipment[]=[];
  user: User | undefined;
  
  
  constructor(private service: AdministrationService,
    private touristService:TourExecutionService,
    private authService: AuthService
  ){}

  ngOnInit():void{
    this.getEquipment();
    this.getTouristEquipment();
    this.authService.user$.subscribe(user => {
      this.user = user;
      console.log(user.id)
    });
  }
  getEquipment():void{
    this.service.getEquipment().subscribe({
      next: (result: PagedResults<Equipment>)=>{
        this.equipment = result.results;
      },
      error: () => {
      }
    })
  }
  getTouristEquipment(): void {
    this.touristService.getTouristEquipment().subscribe({
      next: (result: PagedResults<TouristEquipment>) => {
        this.touristEquipment = result.results;
        this.touristEquipment.forEach(element => {
          if(element.touristId===this.user?.id){
            this.touristEquipmentSource.push(element);
          }
        });
        console.log(this.touristEquipmentSource);
      },
      error: () => {
        console.error('Error loading tourist equipment');
      }
    });
  }
  deleteTouristEquipment(id:number):void{
    let pom=0;
    this.touristEquipmentSource.forEach(element => {
      if(element.equipmentId===id){
        pom=element.id;
      }
    });
    this.touristService.deleteTouristEquipment(pom).subscribe({
      next: () => {
        this.getTouristEquipment();
      },
    })
  }
  addTouristEquipment(newEquipment:any):void{
    let i = 1;
    this.touristEquipment.forEach(element => {
      if(element.id>=i){
        i=element.id+1;
      }
    });
    const tourist_equipment:TouristEquipment= {
      id: i,
      touristId: this.user?.id ?? 0,
      equipmentId: newEquipment 
    } 
    this.touristService.addTouristEquipment(tourist_equipment).subscribe({
      next:() => {
        this.getTouristEquipment();
      },
      error: (err) => {
        console.error('Greska prilikom dodavanje opreme:',err);
      }
    });
  }
  isEquipmentSelected(equipmentId: number): boolean {
    // Check if the equipment exists in the touristEquipmentSource
    return this.touristEquipmentSource.some(te => te.equipmentId === equipmentId);
  }
  
  
  onCheckboxChange(equipment: Equipment, event: any): void {
    // When the checkbox is checked, add equipment, otherwise remove
    if (event.target.checked) {
      this.addTouristEquipment(equipment.id);
    } else {
      const equipmentId = equipment.id ??0;
      this.deleteTouristEquipment(equipmentId);
    }
  }
  
}

