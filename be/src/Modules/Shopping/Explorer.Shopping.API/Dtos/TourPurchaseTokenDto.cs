using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos
{
    public class TourPurchaseTokenDto
    {
        public long UserId { get; set; }
        public long TourId { get; set; }
    }
}
