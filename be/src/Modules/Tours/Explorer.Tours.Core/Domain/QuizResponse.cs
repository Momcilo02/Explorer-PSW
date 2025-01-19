using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class QuizResponse
    {
        public int QuizId { get; private set; }
        public string Message { get; private set; }
        public Reward Reward { get; private set; }

        public QuizResponse(int quizId, string message, Reward reward)
        {
            if (quizId <= 0) throw new ArgumentException("Invalid Quiz ID.");
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message cannot be empty.");
            Reward = reward ?? throw new ArgumentNullException(nameof(reward));

            QuizId = quizId;
            Message = message;
        }

        public override string ToString()
        {
            return $"{Message} (Quiz ID: {QuizId}, Reward: {Reward})";
        }
    }
}
