using Explorer.BuildingBlocks.Core.Domain;
using System.Collections.Generic;

namespace Explorer.Tours.Core.Domain;

public class QuizQuestion : Entity
{
    public long QuizId { get; set; } // ID kviza kojem ovo pitanje pripada
    public string QuestionText { get; set; } // Tekst pitanja
    public List<QuizAnswer> Answers { get; set; } = new(); // Lista odgovora
    public int CorrectAnswerId { get; set; } // Indeks tačnog odgovora

    // Konstruktor bez parametara za EF Core
    private QuizQuestion() { }

    // Konstruktor za poslovnu logiku
    public QuizQuestion(long quizId, string questionText, List<QuizAnswer> answers, int correctAnswerId)
    {
        if (string.IsNullOrWhiteSpace(questionText)) throw new ArgumentNullException(nameof(questionText));
        if (answers == null || answers.Count < 2) throw new ArgumentException("At least two answers are required", nameof(answers));
        if (correctAnswerId < 0 || correctAnswerId >= answers.Count) throw new ArgumentOutOfRangeException(nameof(correctAnswerId), "Correct answer index is out of range");

        QuizId = quizId;
        QuestionText = questionText;
        Answers = answers;
        CorrectAnswerId = correctAnswerId;
    }
}
