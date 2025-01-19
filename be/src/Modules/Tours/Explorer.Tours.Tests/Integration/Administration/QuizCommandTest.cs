using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class QuizCommandTests : BaseToursIntegrationTest
{
    public QuizCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates_quiz()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var newQuiz = new QuizDto
        {
            TourId = -1,
            Title = "Test Quiz",
            Questions = new List<QuizQuestionDto>
        {
            new QuizQuestionDto
            {
                QuestionText = "Sample Question",
                Answers = new List<QuizAnswerDto>
                {
                    new QuizAnswerDto { AnswerText = "Answer 1" },
                    new QuizAnswerDto { AnswerText = "Answer 2" }
                },
                CorrectAnswerIndex = 0
            }
        },
            Reward = new RewardDto // Dodaj nagradu
            {
                Type = "XP",
                Amount = 100
            }
        };

        // Act
        var result = ((ObjectResult)controller.Create(newQuiz).Result)?.Value as QuizResponseDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.QuizId.ShouldNotBe(0);
        result.Message.ShouldBe("Quiz created successfully.");
        result.Reward.Type.ShouldBe("XP");
        result.Reward.Amount.ShouldBe(100);

        // Assert - Database
        var storedQuiz = dbContext.Quizzes.FirstOrDefault(q => q.Title == newQuiz.Title);
        storedQuiz.ShouldNotBeNull();
        storedQuiz.Title.ShouldBe(newQuiz.Title);
        storedQuiz.Reward.Amount.ShouldBe(100);
    }

    [Fact]
    public void Create_quiz_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidQuiz = new QuizDto
        {
            Title = "", // Invalid title
            TourId = -1
        };

        // Act
        var result = (ObjectResult)controller.Create(invalidQuiz).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    private static QuizController CreateController(IServiceScope scope)
    {
        return new QuizController(scope.ServiceProvider.GetRequiredService<IQuizService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
