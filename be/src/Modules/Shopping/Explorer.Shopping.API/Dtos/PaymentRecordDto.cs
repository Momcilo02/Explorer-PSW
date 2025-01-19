using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos
{
    public class PaymentRecordDto
    {
        public long TouristId { get; set; }
        public long ItemId { get; set; }
        public double Price { get; set; }
        public DateTime ShoppingTime { get; set; }
    }
}
