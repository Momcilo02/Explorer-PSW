import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Encounter } from '../encounter-authoring/model/encounter.model';
import { Observable } from 'rxjs';
import { EncounterExecution } from './model/encounter-execution.model';
import { ProfileInfo } from '../layout/model/profileInfo.model';

@Injectable({
  providedIn: 'root'
})
export class EncounterExecutionService {

  constructor(private http: HttpClient) { }

  getEncounterById(id: number): Observable<Encounter> {
    return this.http.get<Encounter>(`https://localhost:44333/api/tourist/encounters/${id}`);
  }

  HasTouristCompletedEncounter(idEncounter: number, idTourist: number): Observable<Boolean> {
    return this.http.get<Boolean>(`https://localhost:44333/api/tourist/encounterExecution/tourist/${idTourist}/encounter/${idEncounter}`);
  }

  getActivatedEncounter(id: number): Observable<EncounterExecution> {
    return this.http.get<EncounterExecution>(`https://localhost:44333/activatedByTourist/${id}`);
  }

  updateUserLocation(encounterExecution: EncounterExecution, numberOfPeople: number): Observable<any> {
    return this.http.put(`https://localhost:44333/api/tourist/encounterExecution/${numberOfPeople}`, encounterExecution);
  }

  setEncounterOnCompleted(encounterExecution: EncounterExecution): Observable<any> {
    return this.http.put('https://localhost:44333/api/tourist/encounterExecution/completedEncounter', encounterExecution);
  }

  leaveEncounter(encounterExecution: EncounterExecution): Observable<any> {
    return this.http.put(`https://localhost:44333/api/tourist/encounterExecution/leave`, encounterExecution);
  }

  updateCurrentUser(currentUser: ProfileInfo): Observable<ProfileInfo>{
    return this.http.put<ProfileInfo>('https://localhost:44333/api/profile-administration/edit/currentUser',currentUser);
  }
}
