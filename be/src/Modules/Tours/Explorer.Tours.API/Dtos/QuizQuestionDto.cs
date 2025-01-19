using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class QuizQuestionDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string ?QuestionText { get; set; }
        public List<QuizAnswerDto> ?Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
    }
}
