using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.TourExecutions
{
    public class CompletedKeyPoints : ValueObject<CompletedKeyPoints>
    {
        public int CompletedKeyPointId { get; private set; }
        public DateTime ExecutionTime { get; private set; }
        public CompletedKeyPoints() { }
        [JsonConstructor]
        public CompletedKeyPoints(DateTime executionTime, int completedKeyPointId)
        {
            CompletedKeyPointId = completedKeyPointId;
            ExecutionTime = executionTime;
        }

        protected override bool EqualsCore(CompletedKeyPoints completedKeyPoints)
        {
            return CompletedKeyPointId == completedKeyPoints.CompletedKeyPointId
                       && ExecutionTime == completedKeyPoints.ExecutionTime;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = CompletedKeyPointId.GetHashCode();
                hashCode = (hashCode * 397) ^ ExecutionTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
