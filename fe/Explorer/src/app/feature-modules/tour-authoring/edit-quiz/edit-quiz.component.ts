import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from '../quiz.service';
import { Quiz, RewardType } from '../model/quiz.model';
import { TourService } from '../tour.service';

@Component({
  selector: 'app-edit-quiz',
  templateUrl: './edit-quiz.component.html',
  styleUrls: ['./edit-quiz.component.css']
})
export class EditQuizComponent implements OnInit {
  rewardTypes = Object.keys(RewardType);
  quiz: Quiz;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private quizService: QuizService,
    private tourService: TourService
  ) {}

  ngOnInit(): void {
    const tourId = +this.route.snapshot.paramMap.get('tourId')!;

    this.quizService.getQuiz(tourId).subscribe({
      next: (q: Quiz) => {
        this.quiz = q;
      },
      error: (err) => {
        alert('Failed to load quiz: ' + err.message);
        this.router.navigate(['/tour']);
      }
    });
  }

  addQuestion(): void {
    this.quiz.questions.push({
      questionText: '',
      answers: [{ answerText: '' }, { answerText: '' }],
      correctAnswerIndex: 0
    });
  }

  addAnswer(questionIndex: number): void {
    this.quiz.questions[questionIndex].answers.push({ answerText: '' });
  }

  validateQuiz(): boolean {
    if (!this.quiz.title?.trim()) return false;
    if (this.quiz.reward.amount <= 0) return false;

    for (const question of this.quiz.questions) {
      if (!question.questionText.trim() || question.answers.length < 2) return false;
      for (const answer of question.answers) {
        if (!answer.answerText.trim()) return false;
      }
    }
    return true;
  }

  onSubmit(): void {
    if (!this.validateQuiz()) {
      alert('Please fill in all fields properly before submitting.');
      return;
    }

    if (!this.quiz?.id) {
      alert('No quiz found, cannot update');
      return;
    }

    // UPDATE kviza
    this.quizService.updateQuiz(this.quiz.id, this.quiz).subscribe({
      next: () => {
        alert('Quiz updated successfully!');
        // Vrati na listu tura
        this.router.navigate(['/tour']);
      },
      error: (err) => {
        alert('Failed to update quiz: ' + err.message);
      }
    });
  }

  // NOVA METODA: brisanje kviza
  onDeleteQuiz(): void {
    if (!this.quiz?.id) return;
    if (!confirm('Are you sure you want to delete this quiz?')) return;

    this.quizService.deleteQuiz(this.quiz.id).subscribe(() => {
      this.tourService.setHasQuiz(this.quiz.tourId, false).subscribe(() => {
        alert('Quiz deleted successfully!');
        this.router.navigate(['/tour']);
      });
    });

  }
}
