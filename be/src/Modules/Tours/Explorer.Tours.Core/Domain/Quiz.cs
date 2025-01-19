using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;

namespace Explorer.Tours.Core.Domain
{
    public class Quiz : Entity
    {
        public int TourId { get; init; }
        public string Title { get;  set; }
        public List<QuizQuestion> Questions { get; set; } = new();
        public Reward Reward { get; set; } 

        private Quiz() { }

        public Quiz(int tourId, string title, List<QuizQuestion> questions, Reward reward)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));
            if (questions == null || questions.Count == 0) throw new ArgumentNullException(nameof(questions));
            if (reward == null) throw new ArgumentNullException(nameof(reward));

            TourId = tourId;
            Title = title;
            Questions = questions;
            Reward = reward;
        }
    }

}
