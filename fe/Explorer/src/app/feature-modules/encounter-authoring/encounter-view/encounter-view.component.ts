import { Component, OnInit } from '@angular/core';
import { EncounterAuthoringService } from '../encounter-authoring.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Encounter } from '../model/encounter.model';
import { EncounterExecution } from '../../encounter-execution/model/encounter-execution.model';
import { Router } from '@angular/router';

@Component({
  selector: 'xp-encounters-view',
  templateUrl: './encounter-view.component.html',
  styleUrls: ['./encounter-view.component.css']
})
export class EncounterViewComponent implements OnInit {
  encounters: Encounter[] = [];
  touristEncounters: Encounter[] = [];
  loggedInUser: User;
  ToShowAllEncounters: boolean = true;
  IsUserAdmin: boolean = false;
  selectedCoordinates: { lat: number; lng: number } | null = null;
  encounterMarkers: { lat: number; lng: number; name: string; encounterType: number}[] = [];
  expandedEncounterId: number | null = null;

  constructor(
    private service: EncounterAuthoringService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    this.IsUserAdminShow();
    this.getAllEncounters();
  }

  toggleExpand(encounterId: number): void {
    this.expandedEncounterId = this.expandedEncounterId === encounterId ? null : encounterId;
  }
  


  getAllEncounters(): void {
    this.service.getEncounters(this.loggedInUser).subscribe({
      next: (data) => {
        this.encounters = data;
        this.transformEncountersToMarkers();
        
      },
      error: (error) => {
        console.error('Error fetching encounters:', error);
      } 
    });
  }

  getAllTouristEncounters(): void {
    this.service.getTouristEncounter().subscribe({
      next: (data) => {
        this.touristEncounters = data;
        console.log(this.touristEncounters);
      },
      error: (error) => {
        console.error('Error fetching tourist encounters:', error);
      }
    });
  }

  getEncounterTypeLabel(type: number): string {
    switch (type) {
      case 0:
        return 'Social';
      case 1:
        return 'Hidden';
      case 2:
        return 'Misc';
      default:
        return 'Unknown';
    }
  }

  transformEncountersToMarkers(): void {
    this.encounterMarkers = this.encounters.map(encounter => ({
      lat: encounter.latitude,
      lng: encounter.longitude,
      name: encounter.name,
      encounterType: encounter.encounterType
    }));
  }


  onMapClick(event: { lat: number; lng: number }) {
    this.selectedCoordinates = event;
  }

  activateEncounter(encId: number) {
    if (this.selectedCoordinates) {
      const encounterExecution: EncounterExecution = {
        touristLatitude: this.selectedCoordinates.lat,
        touristLongitude: this.selectedCoordinates.lng,
        touristId: this.loggedInUser.id,
        status: 0,
        encounterId: encId
      };
      this.service.activateEncounter(encounterExecution).subscribe(
        (response) => {
          this.router.navigate(['/started-encounter']);
        },
        (error) => {
          console.error('Error activating encounter:', error);
        }
      );
      this.selectedCoordinates = null;
    }
  }

  IsUserAdminShow() {
    console.log(this.loggedInUser);
    if (this.loggedInUser.role == "administrator") {
      this.IsUserAdmin = true;
      this.getAllTouristEncounters();
    }
  }

  toggleEncountersView() {
    this.ToShowAllEncounters = !this.ToShowAllEncounters;
    if (this.ToShowAllEncounters) {
      this.getAllEncounters();
    } else {
      this.getAllTouristEncounters();
    }
  }

  updateTouristEncounter(encounter: Encounter, isApproved: boolean): void {
    if(isApproved)
      encounter.touristRequestStatus = 0;
    else
    encounter.touristRequestStatus = 2;
    this.service.updateEncounter(encounter).subscribe({
      next: () => {
        this.getAllTouristEncounters(); // Osveži listu encounter-a
        this.getAllEncounters();
      },
      error: (error) => {
        console.error('Error updating tourist encounter:', error);
      }
    });
  }

 }

