using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace Explorer.Tours.Core.Domain;

public enum TransportType { Walk,Bike,Car};
public enum TimeUnit { Second,Minute,Hour};
public class TourDuration : ValueObject<TourDuration>
{
    public double Duration { get; private set; }
    public TransportType TransportType { get; private set; }
    public TimeUnit TimeUnit { get; private set; }

    [JsonConstructor]
    public TourDuration(double duration, TransportType transportType, TimeUnit timeUnit)
    {
        Duration = duration;
        TransportType = transportType;
        TimeUnit = timeUnit;
    }

    protected override bool EqualsCore(TourDuration other)
    {
       return Duration == other.Duration && TransportType == other.TransportType && TimeUnit == other.TimeUnit;
    }

    protected override int GetHashCodeCore()
    {
        unchecked
        {
            int hashCode = Duration.GetHashCode();
            hashCode = (hashCode * 397) ^ TransportType.GetHashCode();
            return hashCode;
        }
    }
}
