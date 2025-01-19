using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.TourExecutions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.Tours.Core.Domain
{
    public enum TourStatus
    {
        Draft = 0,
        Published = 1,
        Archived = 2,
        Closed = 3
    }

    public enum TourDifficulty
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
        Hell = 3
    }
    public class Tour : Entity
    {
        public string Name { get; private set; }
        public TourDifficulty Difficulty { get; private set; }
        public string Description { get; private set; }
        public double Cost { get; private set; }
        public TourStatus Status { get; private set; }
        public string Tags { get; private set; }
        public List<KeyPoint> KeyPoints { get; set; } = new List<KeyPoint>();
        public List<Equipment> Equipments { get; set; } = new List<Equipment>();
        public List<TourDuration> TourDurations { get; set;} = new List<TourDuration>();
        public List<TourReview> TourReviews { get; set; } = new List<TourReview>();
        public double Length { get; private set; }
        public int AuthorId { get; private set; }
        public DateTime? PublishTime { get; private set; } = null;
        public DateTime? ArchiveTime { get; private set; } = null;
        public string Image { get; private set; }
        public bool ?HasQuiz {  get;set; }

        public Tour(string name, TourDifficulty difficulty, string description, double cost, TourStatus status, string tags, double length,int authorId,string image,bool hasQuiz)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            Name = name;
            Difficulty = difficulty;
            Description = description;
            Cost = cost;
            Status = status;
            Tags = tags;
            Length = length;
            AuthorId = authorId;
            Image = image;
            HasQuiz = hasQuiz;
        }

        public Tour()
        {
        }

        public void AddKeyPoint(KeyPoint keyPoint)
        {
            if (keyPoint == null) throw new ArgumentNullException(nameof(keyPoint));
            KeyPoints.Add(keyPoint);
        }

        public Tour Publish()
        {
            if (!CanPublish())
                throw new ArgumentException("Tura nije ispunila uslove za objavljivanje.");
            Status = TourStatus.Published;
            PublishTime = DateTime.UtcNow;
            return this;
        }

        public bool CanPublish()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Description) &&
                   !string.IsNullOrEmpty(Tags) && KeyPoints.Count >= 2 && TourDurations.Count >= 1;
        }

        public Tour Archive()
        {
            if (!CanArchive())
                throw new ArgumentException("Nije moguce arhivirati turu jer");
            Status = TourStatus.Archived;
            ArchiveTime = DateTime.UtcNow;
            return this;
        }
        
        public Tour UpdateTourLength(double length)
        {
            Length= length;
            return this;
        }

        public bool CanArchive()
        {
            return Status == TourStatus.Published;
        }

        public Tour ReactivateTour()
        {
            if (!CanReactivate())
                throw new ArgumentException("Nije moguce re-aktivirati ovu turu jer nije arhivirana");
            Status = TourStatus.Published;
            return this;
        }
        public bool CanReactivate()
        {
            return Status == TourStatus.Archived;
        }
        public double getAverageRate()
        {
            if (TourReviews == null || !TourReviews.Any())
            {
                return 0;
            }

            return TourReviews.Average(review => review.Rating);
        }

        public void CloseTour()
        {
            Status = TourStatus.Closed;
        }

        public Tour Preview()
        {
            var keyPoints = new List<KeyPoint>();
            keyPoints.Add(KeyPoints.FirstOrDefault());
            return new Tour
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Tags = Tags,
                Image = Image,
                Cost = Cost,
                KeyPoints = keyPoints
            };
        }
    }
}

