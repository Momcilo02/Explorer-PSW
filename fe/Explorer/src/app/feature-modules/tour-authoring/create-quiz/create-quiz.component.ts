import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from '../quiz.service';
import { Quiz,RewardType} from '../model/quiz.model';
import { TourService } from '../tour.service';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.css']
})
export class CreateQuizComponent implements OnInit {
  rewardTypes = Object.keys(RewardType); // Enum kao niz za prikaz
  quiz: Quiz = {
    tourId: 0,
    title: '',
    questions: [],
    reward: {
      type: RewardType.XP, // Podrazumevani tip nagrade
      amount: 0
    }
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private quizService: QuizService,
    private tourService: TourService
  ) {}

  ngOnInit(): void {
    this.quiz.tourId = +this.route.snapshot.paramMap.get('tourId')!;
    this.addQuestion();
    this.addQuestion();
    this.addQuestion();
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

  onCorrectAnswerSelected(questionIndex: number, answerIndex: number): void {
    this.quiz.questions[questionIndex].correctAnswerIndex = answerIndex;
  }

  onSubmit(): void {
    console.log(this.quiz.questions);
    if (!this.validateQuiz()) {
      alert('Please fill in all fields properly before submitting.');
      return;
    }

    this.quizService.createQuiz(this.quiz).subscribe({
      next: () => {
        this.tourService.setHasQuiz(this.quiz.tourId, true).subscribe(() => {
          alert('Quiz created successfully!');
          this.router.navigate(['/tour']);
        });
      },
      error: (err) => alert('Failed to create quiz: ' + err.message)
    });

  }


  validateQuiz(): boolean {
    if (!this.quiz.title.trim()) return false;
    if (this.quiz.reward.amount <= 0) return false;

    for (const question of this.quiz.questions) {
      if (!question.questionText.trim() || question.answers.length < 2) return false;
      for (const answer of question.answers) {
        if (!answer.answerText.trim()) return false;
      }
    }
    return true;
  }
}
