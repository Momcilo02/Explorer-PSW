import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Equipment } from './model/equipment.model';
import { environment } from 'src/env/environment';
import { Observable } from 'rxjs';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { ReportedTourProblems, ReportedTourProblem } from './model/reported-tour-problems.model';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Message } from './model/message.model';
import { Tour } from '../tour-authoring/model/tour.model';
import { ReportingTourProblem } from '../tour-execution/model/reporting-tour-problem.model';
import { Person } from './model/person.model';
import { Object } from '../administration/model/object.model';
import { KeyPoint } from './model/keyPoint.model';
import { Account } from './model/account.model';

@Injectable({
  providedIn: 'root'
})
export class AdministrationService {

  constructor(private http: HttpClient) { }

  getObjects(): Observable<PagedResults<Object>>{
    return this.http.get<PagedResults<Object>>(environment.apiHost + 'administration/object');
  }

  getKeyPoints(): Observable<KeyPoint[]>{
    return this.http.get<KeyPoint[]>(environment.apiHost + 'administration/keypoint');
  }

  updateObjectStatus(object: Object): Observable<Object>{
    return this.http.put<Object>(environment.apiHost + 'administration/object/' + object.id,object);
  }

  updateKeyPointStatus(keyPoint: KeyPoint): Observable<KeyPoint>{
    return this.http.put<KeyPoint>(environment.apiHost + 'administration/keyPoint/' + keyPoint.id, keyPoint);
  }

  getEquipment(): Observable<PagedResults<Equipment>> {
    return this.http.get<PagedResults<Equipment>>(environment.apiHost + 'administration/equipment')
  }

  deleteEquipment(id: number): Observable<Equipment> {
    return this.http.delete<Equipment>(environment.apiHost + 'administration/equipment/' + id);
  }

  addEquipment(equipment: Equipment): Observable<Equipment> {
    return this.http.post<Equipment>(environment.apiHost + 'administration/equipment', equipment);
  }

  updateEquipment(equipment: Equipment): Observable<Equipment> {
    return this.http.put<Equipment>(environment.apiHost + 'administration/equipment/' + equipment.id, equipment);
  }
  getAccounts(): Observable<PagedResults<Account>> {
    return this.http.get<PagedResults<Account>>('https://localhost:44333/api/profile-administrator');
  }
  getReportedTourProblems(loggedInUser : User): Observable<PagedResults<ReportedTourProblems>> {
    if (loggedInUser.role === 'tourist')
      return this.http.get<PagedResults<ReportedTourProblems>>(environment.apiHost + 'tourist/tour-problem/' + loggedInUser.id)
    else if (loggedInUser.role === 'author')
      return this.http.get<PagedResults<ReportedTourProblems>>(environment.apiHost + 'author/tour-problem/' + loggedInUser.id)
    else
      return this.http.get<PagedResults<ReportedTourProblems>>(environment.apiHost + 'administrator/tour-problem')
  }

  getReportedTourProblemById(reportId: number): Observable<ReportedTourProblems>{
    return this.http.get<ReportedTourProblems>(`https://localhost:44333/api/user/tour-problem/getReportById/${reportId}`);
  }

  getTourById(id: number): Observable<Tour> {
    return this.http.get<Tour>('https://localhost:44333/api/administration/tour/' + id);
  }

  getAllUsers(loggedInUser: User): Observable<PagedResults<Person>> {
    if (loggedInUser.role !== 'administrator') {
      return this.http.get<PagedResults<Person>>('https://localhost:44333/api/profile-administration/edit');
    } else {
      return this.http.get<PagedResults<Person>>('https://localhost:44333/api/profile-administrator');
    }
  }
  getAllUserss(): Observable<PagedResults<User>> {
    return this.http.get<PagedResults<User>>('https://localhost:44333/api/profile-administrator');
  }

  getReportedTourProblem(reportedTourProblemId: number): Observable<ReportedTourProblem>{
    return this.http.get<ReportedTourProblem>(environment.apiHost + 'administrator/tour-problem/' + reportedTourProblemId)
  }

  setSolvingDate(id: number, reportedTourProblem: ReportedTourProblem): Observable<ReportedTourProblem> {
      return this.http.put<ReportedTourProblem>(environment.apiHost + 'administrator/tour-problem/set-deadline/' + id, reportedTourProblem);
  }

  addMessage(message: Message, loggedInUser: User): Observable<Message> {
    if (loggedInUser.role === 'administrator')
      return this.http.put<Message>(environment.apiHost + 'administrator/tour-problem/addMessage/' + message.userId + '/' + message.reportId, message);
    else
      return this.http.put<Message>(environment.apiHost + 'user/tour-problem/addMessage/' + message.userId + '/' + message.reportId, message);
  }

  penalizeAuthorAndCloseProblem(report: ReportedTourProblems): Observable<ReportedTourProblems> {
    return this.http.put<ReportedTourProblems>(environment.apiHost + 'administrator/tour-problem/penalize-author-close-problem/' + report.id, report);
  }

  setAsSolvedOrUnsolved(reportId: number, isSolved: boolean, comment: string): Observable<ReportedTourProblems> {
    return this.http.put<ReportedTourProblems>(environment.apiHost + `tourist/tour-problem/setAsSolvedOrUnsolved/${reportId}?comment=${comment}`, isSolved);
  }
}
