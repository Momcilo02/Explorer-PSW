using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class QuizService : CrudService<QuizDto, Quiz>, IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public QuizService(ICrudRepository<Quiz> repository,IQuizRepository quizRepository, IMapper mapper)
            : base(repository, mapper)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
        }
        public Result<QuizDto> UpdateQuiz(int id, QuizDto dto)
        {
            try
            {
                // Dohvatanje kviza iz baze
                var quiz = _quizRepository.GetById(id);
                if (quiz == null) return Result.Fail("Quiz not found");

                // Ažuriranje podataka
                quiz.Title = dto.Title;
                quiz.Reward = new Reward(
                    (RewardType)Enum.Parse(typeof(RewardType), dto.Reward.Type),
                    dto.Reward.Amount
                );

                // Prvo brišemo postojeća pitanja i odgovore
                quiz.Questions.Clear();

                // Dodavanje novih pitanja sa odgovorima
                foreach (var questionDto in dto.Questions)
                {
                    // Kreiramo listu odgovora
                    var answers = questionDto.Answers
                                             .Select(a => new QuizAnswer(a.AnswerText))
                                             .ToList();

                    // Kreiramo pitanje sa listom odgovora
                    var question = new QuizQuestion(
                        quiz.Id,
                        questionDto.QuestionText,
                        answers,
                        questionDto.CorrectAnswerIndex
                    );

                    // Dodajemo pitanje direktno u listu pitanja kviza
                    quiz.Questions.Add(question);
                }

                // Ažuriranje u bazi
                _quizRepository.Update(quiz);

                // Mapiranje rezultata i vraćanje odgovora
                var updatedQuiz = _mapper.Map<QuizDto>(quiz);
                return Result.Ok(updatedQuiz);
            }
            catch (Exception ex)
            {
                return Result.Fail("Failed to update quiz").WithError(ex.Message);
            }
        }


        public Result DeleteQuiz(int id)
        {
            try
            {
                var quiz = _quizRepository.GetById(id);
                if (quiz == null) return Result.Fail("Quiz not found");

                _quizRepository.Delete(quiz);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("Failed to delete quiz").WithError(ex.Message);
            }
        }

        public PagedResult<QuizDto> GetAllQuizzes(int page, int pageSize)
        {
            try
            {
                // Dobavljanje kvizova iz baze
                var quizzes = _quizRepository.GetAll()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Mapiranje domen -> DTO
                var quizDtos = _mapper.Map<List<QuizDto>>(quizzes);

                // Ukupan broj kvizova
                var totalCount = _quizRepository.GetAll().Count();

                return new PagedResult<QuizDto>(quizDtos, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quizzes.", ex);
            }
        }
        public Result<QuizResponseDto> CreateQuiz(QuizDto dto)
        {
            try
            {
                // Mapiranje DTO -> Domen
                var quiz = _mapper.Map<Quiz>(dto);

                // Čuvanje kviza u bazi
                var createdQuiz = _quizRepository.Create(quiz);

                // Generisanje nagrade
                var reward = new Reward(RewardType.XP, 100);

                // Mapiranje domen -> Response DTO
                var response = new QuizResponseDto
                {
                    QuizId = ((int)createdQuiz.Id),
                    Message = "Quiz created successfully.",
                    Reward = _mapper.Map<RewardDto>(reward)
                };

                return Result.Ok(response);
            }
            catch (Exception ex)
            {
                return Result.Fail("Failed to create quiz").WithError(ex.Message);
            }
        }
        public Result<QuizDto> GetQuizByTourId(int tourId)
        {
            
              var result = _quizRepository.GetByTourId(tourId);
              return MapToDto(result);
            
        }
    }

}
