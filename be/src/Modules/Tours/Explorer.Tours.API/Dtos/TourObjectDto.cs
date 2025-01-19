using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours;

namespace Explorer.Tours.API.Dtos
{
    public class TourObjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int category { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public ObjectStatus Status { get; set; }
        public string Comment { get; set; }
        public int AuthorId { get; set; }

        public enum ObjectStatus
        {
            PRIVATE,
            REQUESTED,
            PUBLIC
        }
    }
}
