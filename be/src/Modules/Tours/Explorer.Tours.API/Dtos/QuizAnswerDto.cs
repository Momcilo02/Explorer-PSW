﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class QuizAnswerDto
    {
        public int Id {  get; set; }
        public int QuestionId { get; set; }
        public string ?AnswerText { get; set; }
    }
}
