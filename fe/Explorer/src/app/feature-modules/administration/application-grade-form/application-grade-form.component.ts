import { Component } from '@angular/core';
import { ApplicationGradeService } from '../application-grade.service'; // Servis koji ćemo kreirati

@Component({
  selector: 'app-application-grade-form',
  templateUrl: './application-grade-form.component.html',
})
export class ApplicationGradeFormComponent {
  rating: number | null = null;
  comment: string = '';

  constructor(private gradeService: ApplicationGradeService) {}

  onSubmit() {
    const grade = {
      rating: this.rating,
      comment: this.comment || 'Nema komentara', // Postavi default vrednost za komentar ako je prazan
      created: new Date()
    };
    this.gradeService.addGrade(grade).subscribe(response => {
      console.log('Grade submitted:', response);
    });
  }
}

