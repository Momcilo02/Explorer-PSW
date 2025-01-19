using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface IQuizService
    {
        Result<QuizResponseDto> CreateQuiz(QuizDto dto);
        PagedResult<QuizDto> GetAllQuizzes(int page, int pageSize);

        Result<QuizDto> UpdateQuiz(int id, QuizDto dto);
        Result DeleteQuiz(int id);
        Result<QuizDto> GetQuizByTourId(int tourId);

    }
}
