import { Component, EventEmitter, Inject, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TourExecutionService } from '../tour-execution.service';
import { ReportingTourProblem } from '../model/reporting-tour-problem.model';
import { MatSnackBar } from '@angular/material/snack-bar'; 
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { ActivatedRoute, Router } from '@angular/router'; 
import { Tour } from '../../tour-authoring/model/tour.model';

@Component({
  selector: 'xp-reporting-tour-problem',
  templateUrl: './reporting-tour-problem.component.html',
  styleUrls: ['./reporting-tour-problem.component.css']
})
export class ReportingTourProblemComponent implements OnChanges, OnInit {
  @Output() reportChanged = new EventEmitter<null>();
  loggedInUser: User; 
  tourName: string='';

  constructor(
    private service: TourExecutionService, 
    private snackBar: MatSnackBar,
    private authService: AuthService, 
    private router: Router,
    private route: ActivatedRoute 
  ) {}


  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      this.loggedInUser = user;
    });
    const tourId = this.route.snapshot.paramMap.get('id');
    if (tourId) {
      this.loadTourDetails(Number(tourId));
    }
  }

  loadTourDetails(tourId: number): void {
    this.service.getTourById(tourId).subscribe({
      next: (tour: Tour) => {
        this.tourName = tour.name;
        this.problemReportForm.patchValue({
          tourName: this.tourName,
        });
      },
      error: (err) => {
        console.error('Error fetching tour:', err);
      },
    });
  }

  problemReportForm = new FormGroup({
    tourName: new FormControl({ value: this.tourName, disabled: true }),
    category: new FormControl('', [Validators.required]),
    priority: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    time: new FormControl('', [Validators.required]),
    date: new FormControl('', [Validators.required]),
  });
  
  ngOnChanges(): void {
    this.problemReportForm.reset();
  }

  createReport(): void {
    const dateValue = this.problemReportForm.value.date;
  const timeValue = this.problemReportForm.value.time;

  if (!dateValue) {
    console.error('Date is required');
    return;
  }

  const [hours, minutes] = timeValue ? timeValue.split(':').map(Number) : [0, 0];
  
  const combinedDateTime = new Date(dateValue);
  combinedDateTime.setHours(hours, minutes);


    const report: ReportingTourProblem = {
      tourId: Number(this.route.snapshot.paramMap.get('id')),
      category: this.problemReportForm.value.category || "",
      priority: (Number(this.problemReportForm.value.priority) === 0 || 
        Number(this.problemReportForm.value.priority) === 1 || 
        Number(this.problemReportForm.value.priority) === 2) 
        ? Number(this.problemReportForm.value.priority) 
       : 2,
      description: this.problemReportForm.value.description || "",
      time: combinedDateTime,
      touristId: this.loggedInUser.id,
    };
    
    this.service.addReport(report).subscribe({
      next: () => { 
        this.reportChanged.emit();
        this.problemReportForm.reset();
        this.snackBar.open('Report successfully submitted!', 'Close', {
          duration: 5000, 
        });
        this.router.navigate([`reported-tour-problems`]); 
      },
      error: (err) => {
        console.error('Error submitting report:', err);
        this.snackBar.open('Failed to submit report. Please try again.', 'Close', {
          duration: 3000,
        });
      }
    });
  }
}