export interface Quiz {
  id?: number; // Opcioni ID ako se automatski generiše
  tourId: number;
  title: string;
  questions: QuizQuestion[];
  reward: Reward;
}
export enum RewardType {
  XP = 'XP',
  Coupon = 'Coupon',
  AC = 'AC'
}

export interface Reward {
  type: RewardType;
  amount: number;
}

export interface QuizQuestion {
  id?: number;
  quizId?: number;
  questionText: string;
  answers: QuizAnswer[];
  correctAnswerIndex: number;
}

export interface QuizAnswer {
  id?: number;
  questionId?: number;
  answerText: string;
}
