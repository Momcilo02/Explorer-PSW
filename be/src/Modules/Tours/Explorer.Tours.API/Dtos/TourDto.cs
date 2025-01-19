using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
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
    public class TourDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TourDifficulty Difficulty { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Cost { get; set; }
        public TourStatus Status { get; set; }
        public string Tags { get; set; } = string.Empty;
        public double Length { get; set; }
        public int AuthorId { get; set; }
        public double AverageRate { get; set; }
        public IEnumerable<KeyPointDto> KeyPoints { get; set; } = new List<KeyPointDto>();
        public IEnumerable<EquipmentDto> Equipments { get; set; } = new List<EquipmentDto>();
        public IEnumerable<TourDurationDto> TourDurations { get; set; } = new List<TourDurationDto>();
        public DateTime? PublishTime { get;  set; } = null;
        public DateTime? ArchiveTime { get; set; } = null;
        public string Image { get; set; } = string.Empty;
        public bool HasQuiz {  get; set; }
    }
}
