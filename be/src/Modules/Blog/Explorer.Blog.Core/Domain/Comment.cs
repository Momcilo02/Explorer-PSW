using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain
{
    public class Comment : Entity 
    {
        public int UserId { get;  set; }  
        public string Username { get;  set; }
        public DateTime CreatedAt { get;  set; }  
        public string Text { get;  set; }  
        public DateTime? LastModified { get;  set; }  

        public Comment(int userId, string username, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Invalid Comment Text.");
            Username = username;
            UserId = userId;
            Text = text;
            CreatedAt = DateTime.UtcNow; 
            LastModified = null;  
        }

    }
}
