import { Component, OnInit } from '@angular/core';
import { ApplicationGradeService } from '../application-grade.service';

@Component({
  selector: 'app-application-grade-list',
  templateUrl: './application-grade-list.component.html',
})
export class ApplicationGradeListComponent implements OnInit {
  grades: any[] = [];
  filteredGrades: any[] = [];
  
  constructor(private gradeService: ApplicationGradeService) {}

  ngOnInit() {
    this.gradeService.getGrades().subscribe(
      (response) => {
        this.grades = response.items; // Pristupi 'items'
        this.filteredGrades = this.grades; // Postavljamo inicijalno filtrirane ocene
      },
      (error) => {
        console.error('Error fetching grades:', error);
      }
    );
  }

  filterComments(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredGrades = this.grades.filter(grade => grade.comment.toLowerCase().includes(filterValue));
  }

  sortByRating() {
    this.filteredGrades.sort((a, b) => b.rating - a.rating);
  }

  sortByDate() {
    this.filteredGrades.sort((a, b) => new Date(b.created).getTime() - new Date(a.created).getTime());
  }
}
