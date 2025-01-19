INSERT INTO tours."Quizzes" ("Id", "TourId", "Title", "Reward_Type", "Reward_Amount") 
VALUES (-1, -1, 'General Knowledge Quiz', 'XP', 100),
       (-2, -1, 'Math Quiz', 'Coupon', 50),
       (-3, -1, 'Geography Quiz', 'AC', 200);
INSERT INTO tours."QuizQuestions" ("Id", "QuizId", "QuestionText", "CorrectAnswerId") 
VALUES (-1, -1, 'What is 2+2?', -2),
       (-2, -2, 'What is the capital of France?', -4),
       (-3, -3, 'Which is the largest ocean?', -6);
INSERT INTO tours."QuizAnswers" ("Id", "QuizQuestionId", "AnswerText") 
VALUES (-1, -1, '3'),
       (-2, -1, '4'), -- Correct Answer for Question -1
       (-3, -2, 'London'),
       (-4, -2, 'Paris'), -- Correct Answer for Question -2
       (-5, -3, 'Atlantic'),
       (-6, -3, 'Pacific'); -- Correct Answer for Question -3
