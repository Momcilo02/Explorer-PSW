
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AdministrationService } from '../administration.service';
import { ReportedTourProblemsComponent } from '../reported-tour-problems/reported-tour-problems.component';
import { ReportedTourProblems, ReportedTourProblem } from '../model/reported-tour-problems.model';
import { PagedResults } from 'src/app/shared/model/paged-results.model';

@Component({
  selector: 'xp-solving-deadline-form',
  templateUrl: './solving-deadline-form.component.html',
  styleUrls: ['./solving-deadline-form.component.css']
})
export class SolvingDeadlineFormComponent implements OnInit {
  form: FormGroup;
  @Input() reportedTourProblemId: number;
  @Output() tourProblemReportUpdated = new EventEmitter<ReportedTourProblem>();
  formattedDate: string;
  reportTourProblem: ReportedTourProblem;
  minDate: Date = new Date();

  constructor(
    private fb: FormBuilder,
    private service: AdministrationService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      releasedAt: [null, [Validators.required, this.dateFormatValidator()]],
    });
  }

  logClick(): void {
    console.log('Dugme kliknuto!');
  }

  get releasedAt() {
    return this.form.get('releasedAt');
  }

  makeUpdate(): void{
    this.service.setSolvingDate(this.reportedTourProblemId, this.reportTourProblem).subscribe({
      next: (response) => {
        //this.router.navigate(["tour"]);
        console.log("OVO SALJEM EEEJJJ");
        console.log(this.reportTourProblem);
        console.log("APDEJTUJEMOOO", response);
        this.tourProblemReportUpdated.emit(this.reportTourProblem);
      }
    })
  }

  setDate(): void {
    const selectedDate = this.form.get('releasedAt')?.value;
    if (selectedDate) {
      this.service.getReportedTourProblem(this.reportedTourProblemId).subscribe({
        next: (response) => {
          console.log(response);

          this.reportTourProblem = response;
          this.reportTourProblem.solvingDeadline = selectedDate;
          this.reportTourProblem.status = 1;
          this.makeUpdate();
        }
      })
    }
  }

  dateFormatValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

      const dateValue = control.value instanceof Date ? control.value : new Date(control.value);

      // Formatiramo datum u željeni MM/dd/yyyy format
      this.formattedDate = dateValue.toLocaleDateString('en-US');

      const dateRegex = /^(0[1-9]|1[0-2])\/([0-2][0-9]|3[01]|[1-9])\/\d{4}$/;
      const valid = dateRegex.test(this.formattedDate);
      console.log(valid);
      console.log("Vrednost ", control.value);
      return valid ? null : { invalidDateFormat: true };
    }
  };
}
