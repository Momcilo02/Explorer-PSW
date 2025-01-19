using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain
{
    public class Blog : Entity
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Rating> Ratings { get; set; }
        public BlogStatus Status { get; private set; }
        public List<string> ImageUrl { get; set; }
        public DateOnly Date {  get; init; }
        public BlogActivityStatus ActivityStatus { get; private set; }
        public int OwnerId { get; init; }

        public Blog(string title, string description, BlogStatus status, DateOnly date, BlogActivityStatus activityStatus, int ownerId, List<string> imageUrl)
        {
            Title = title;
            Description = description;
            Status = status;
            Date = date;
            ActivityStatus = activityStatus;
            OwnerId = ownerId;
            Ratings = new List<Rating>();
            ImageUrl = imageUrl;
            Validate();
            UpdateActivityStatus();
        }

        private void Validate()
        {
            if(string.IsNullOrWhiteSpace(Title)) throw new ArgumentException("Invalid title");
            if(string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid description");

        }
        public void UpdateRating(Rating rating) { 
            Rating r = Ratings.Find(rat => rat.UserId == rating.UserId);
            if(r!= null)
            {
                if(rating.Grade == r.Grade)
                {
                    Ratings.Remove(r);
                }
                else
                {
                    Ratings.Remove(r);
                    Ratings.Add(rating);
                }
            }
            else
            {
                Ratings.Add(rating);
            }
            UpdateActivityStatus();
        }
        public int CalculateScore()
        {
            int upvotes = Ratings.Count(r => r.Grade);
            int downvotes = Ratings.Count(r => !r.Grade);
            return upvotes - downvotes;
        }


        public void UpdateActivityStatus()
        {
            int score = CalculateScore();
            int commentCount = Comments.Count;

            if (score < -10)
            {
                ActivityStatus = BlogActivityStatus.closed;
            }
            else if (score > 500 && commentCount > 30)
            {
                ActivityStatus = BlogActivityStatus.famous;
            }
            else if (score > 100 || commentCount > 10)
            {
                ActivityStatus = BlogActivityStatus.active;
            }
            else
            {
                ActivityStatus = BlogActivityStatus.regular;
            }
        }

        public void AddComment(Comment comment)
        {
            Comments.Add(comment);
            // Recalculate the activity status after adding a comment
            UpdateActivityStatus();
        }
        public void UpdateComment(int commentId, string newText)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                throw new ArgumentException("Comment not found");

            comment.Text = newText;
            comment.LastModified = DateTime.UtcNow;

            // Recalculate the activity status after updating a comment
            UpdateActivityStatus();
        }
        public void DeleteComment(int commentId)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                throw new ArgumentException("Comment not found");

            Comments.Remove(comment);

            // Recalculate the activity status after deleting a comment
            UpdateActivityStatus();
        }

        public void PublishBlog()
        {
            this.Status = BlogStatus.published;
        }
        public void CloseBlog()
        {
            this.Status = BlogStatus.closed;
        }

    }

    public enum BlogStatus
    {
        draft,
        published,
        closed
    }
    public enum BlogActivityStatus
    {
        regular,
        active,
        famous,
        closed
    }
}
