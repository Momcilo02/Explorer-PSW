using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public long Percentage { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long AuthorId { get; set; }
        public List<int> ToursEligible { get; set; }
        public int CouponStatus { get; set;}
    }
}
