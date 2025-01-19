using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain
{
    public class Rating : ValueObject<Rating>
    {
        public bool Grade { get; init; }
        public int UserId { get; init; }
        public DateTime CreationTime {  get; init; }

        [JsonConstructor]
        public Rating(bool grade, int userId, DateTime creationTime) {
            Grade = grade;
            UserId = userId;
            CreationTime = creationTime;
        }

        protected override bool EqualsCore(Rating other)
        {
            return Grade == other.Grade && UserId == other.UserId;
        }

        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }
    }
}
