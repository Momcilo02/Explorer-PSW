import { Component, OnInit } from '@angular/core';
import { AdministrationService } from '../administration.service';
import { PagedResults } from 'src/app/shared/model/paged-results.model';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { Message} from '../model/message.model';
import { forkJoin, map, Observable, switchMap } from 'rxjs';
import { Tour } from '../../tour-authoring/model/tour.model';
import { ReportedTourProblems, ReportedTourProblem } from '../model/reported-tour-problems.model';
import { Person } from '../model/person.model';

@Component({
  selector: 'xp-reported-tour-problems',
  templateUrl: './reported-tour-problems.component.html',
  styleUrls: ['./reported-tour-problems.component.css']
})
export class ReportedTourProblemsComponent implements OnInit {
  reportedTourProblems: any[] = [];
  loggedInUser: User;
  selectedReport: ReportedTourProblems | null = null;
  newMessageContent: string = '';
  currentDate: Date;
  selectedDate: Date;
  usernamesMap: { [key: number]: string } = {};
  isCommentVisible: { [id: number]: boolean } = {};
  unsolvedComment: { [id: number]: string } = {};
  isModalOpen = false;

  constructor(
    private service : AdministrationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
      this.authService.user$.subscribe((user) => {
        this.loggedInUser = user;
      });
      this.loadReportedTourProblems();
      this.currentDate = new Date();

      this.service.getAllUsers(this.loggedInUser).subscribe({
        next: (result: PagedResults<Person>) => {
          result.results.forEach(user => {
            this.usernamesMap[user.id] = user.name + ' ' + user.surname;
          });
        },
        error: (err) => {
          console.error('Greška prilikom učitavanja korisnika:', err);
        }
      });
  }

  loadReportedTourProblems(): void {
    this.service.getReportedTourProblems(this.loggedInUser).pipe(
      switchMap((result: PagedResults<ReportedTourProblems>) => {
        const problemList = result.results;

        const tourRequests = problemList.map((problem) =>
          this.service.getTourById(+problem.tourId).pipe(
            map((tour) => ({
              ...problem,
              tourName: tour.name
            }))
          )
        );

        return forkJoin(tourRequests);
      })
    ).subscribe({
      next: (problems) => {
        this.reportedTourProblems = problems;
        console.log("****************************  ", problems);

      },
      error: (err) => console.error('Error loading problems:', err)
    });
  }

  getPriorityLabel(priority: number): string {
    switch (priority) {
      case 2:
        return 'HIGH';
      case 1:
        return 'MEDIUM';
      case 0:
        return 'LOW';
      default:
        return 'UNKNOWN';
    }
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 4:
        return 'CLOSED';
      case 3:
        return 'UNSOLVED';
      case 2:
        return 'SOLVED';
      case 1:
        return 'SOLVING';
      case 0:
        return 'REPORTED';
      default:
        return 'UNKNOWN';
    }
  }

  selectReport(report: ReportedTourProblems): void {
    this.selectedReport = report;
  }

  onTourProblemReportUpdated(reportedTourProblem: ReportedTourProblem ) {
    this.getAllReports();
  }

  getAllReports(): void{
    this.service.getReportedTourProblems(this.loggedInUser).subscribe({
      next: (result: PagedResults<ReportedTourProblems>) => {
        this.reportedTourProblems = result.results;
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  addMessage(): void {
    if (!this.selectedReport || !this.newMessageContent.trim()) return;

    const newMessage: Message = {
      userId: this.loggedInUser.id,
      reportId: this.selectedReport.id,
      content: this.newMessageContent.trim()
    };

    this.service.addMessage(newMessage, this.loggedInUser).subscribe({
      next: () => {
        this.selectedReport?.messages.push(newMessage);
        this.newMessageContent = '';
      },
      error: (err) => {
        console.error('Error adding message:', err);
      }
    });
  }

  isLoggedInUserAdmin(): boolean {
    return this.loggedInUser && this.loggedInUser.role === 'administrator';
  }

  canPenalizeAndClose(rtp: ReportedTourProblems): boolean {
    return this.isLoggedInUserAdmin() && rtp.status !== 2 && rtp.status !== 4 && rtp.solvingDeadline !== null && new Date(rtp.solvingDeadline) < this.currentDate;
  }

  penalizeAuthorAndCloseProblem(report: ReportedTourProblems) {
    this.service.penalizeAuthorAndCloseProblem(report).subscribe({
      next: () => {
        this.loadReportedTourProblems();
      },
      error: (err) => {
        console.error('Error closing problem and penalizing author:', err);
      }
    });
  }

  setAsSolvedOrUnsolved(reportId: number, isSolved: boolean) {
    var comment = '';
    if(isSolved)
      comment =  'It`s solved.';
    else
      comment = this.unsolvedComment[reportId];

    this.service.setAsSolvedOrUnsolved(reportId, isSolved, comment).subscribe({
      next: () => {
        this.loadReportedTourProblems();
      },
      error: (error) => {
        console.error('Error setting status or adding comment', error);
      }
    });

    if (!isSolved) {
      this.isCommentVisible[reportId] = true;
    }
  }

  markAsUnsolved(reportId: number) {
    this.isCommentVisible[reportId] = true;
  }
  markAsSolved(reportId: number) {
    this.setAsSolvedOrUnsolved(reportId, true);
  }




  /** IVA **/
  openModal(report: any) {
    this.selectedReport = report;
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedReport = null;
  }
}
