using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class QuizAnswer : ValueObject<QuizAnswer>
    {
        public string AnswerText { get; private set; }

        public QuizAnswer(string answerText)
        {
            if (string.IsNullOrWhiteSpace(answerText)) throw new ArgumentNullException(nameof(answerText));
            AnswerText = answerText;
        }

        protected override bool EqualsCore(QuizAnswer other)
        {
            return AnswerText == other.AnswerText;
        }

        protected override int GetHashCodeCore()
        {
            return AnswerText.GetHashCode();
        }
    }

}
