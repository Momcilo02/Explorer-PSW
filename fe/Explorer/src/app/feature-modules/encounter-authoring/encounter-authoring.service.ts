import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SocialEncounters } from './model/social-encounter.model';
import { Encounter } from './model/encounter.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { EncounterExecution } from '../encounter-execution/model/encounter-execution.model';
import { HiddenEncounters } from './model/hidden-encounter.model';
import { MiscEncounters } from './model/misc-encounter.model';

@Injectable({
  providedIn: 'root'
})
export class EncounterAuthoringService {

  constructor(private http: HttpClient) { }

  addSocialEncounter(socialEncounter: SocialEncounters, loggedUser: User): Observable<SocialEncounters> {
    if(loggedUser.role == 'administrator')
    return this.http.post<SocialEncounters>(`https://localhost:44333/api/administration/encounters`, socialEncounter);
    else 
    return this.http.post<SocialEncounters>(`https://localhost:44333/api/tourist/encounters`, socialEncounter);
  }
  addHiddenEncounter(hiddenEncounter: HiddenEncounters, loggedUser: User): Observable<HiddenEncounters> {
    if(loggedUser.role == 'administrator')
    return this.http.post<HiddenEncounters>(`https://localhost:44333/api/administration/encounters`, hiddenEncounter);
    else
    return this.http.post<HiddenEncounters>(`https://localhost:44333/api/tourist/encounters`, hiddenEncounter);
  }

  addMiscEncounter(miscEncounter: MiscEncounters, loggedUser: User): Observable<MiscEncounters> {
    if(loggedUser.role == 'administrator'){
    return this.http.post<MiscEncounters>(`https://localhost:44333/api/administration/encounters`, miscEncounter);
    }
    else
    return this.http.post<MiscEncounters>(`https://localhost:44333/api/tourist/encounters`, miscEncounter);
  }


  //**********IVA************//
  addSocialEncounterAuthor(socialEncounter: SocialEncounters): Observable<SocialEncounters> {
    return this.http.post<SocialEncounters>(`https://localhost:44333/api/user/encounters`, socialEncounter);
  }
  addHiddenEncounterAuthor(hiddenEncounter: HiddenEncounters): Observable<HiddenEncounters> {
    return this.http.post<HiddenEncounters>(`https://localhost:44333/api/user/encounters`, hiddenEncounter);
  }

  addMiscEncounterAuthor(miscEncounter: MiscEncounters): Observable<MiscEncounters> {
    return this.http.post<MiscEncounters>(`https://localhost:44333/api/user/encounters`, miscEncounter);
  }
  //////////////////////////////

  getEncounters(loggedInUser: User): Observable<Encounter[]> {
    if(loggedInUser.role === 'administrator')
      return this.http.get<Encounter[]>(`https://localhost:44333/api/administration/encounters`);
    else
      return this.http.get<Encounter[]>(`https://localhost:44333/api/tourist/encounters`); //dodati kasnije i za ulogu autora
  }

  getTouristEncounter(): Observable<Encounter[]> {
    return this.http.get<Encounter[]>(`https://localhost:44333/api/administration/encounters/requestedEncounters`);
  }

  updateEncounter(toursitEncounter: Encounter): Observable<Encounter>{  
    return this.http.put<Encounter>(`https://localhost:44333/api/administration/encounters/${toursitEncounter.id}`, toursitEncounter);
  }

  activateEncounter(encounterExecution: EncounterExecution): Observable<EncounterExecution> {
    return this.http.post<EncounterExecution>(`https://localhost:44333/api/tourist/encounterExecution`, encounterExecution);
  }

}
