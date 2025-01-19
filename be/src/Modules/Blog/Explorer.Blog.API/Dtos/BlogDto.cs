using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Explorer.Blog.API.Dtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BlogStatus Status { get; set; }
        public IEnumerable<string>? ImageUrl { get; set; }
        public DateOnly Date { get; set; }
        public int OwnerId { get; set; }
        public BlogActivityStatus ActivityStatus { get; set; }
        public IEnumerable<RatingDto>? Ratings { get; set; }
        public IEnumerable<CommentDto>? Comments { get; set; } 
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
