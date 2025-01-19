using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos
{
    public class TouristWalletDto
    {
        public long Id { get; set; }
        public int AdventureCoins { get; set; }
        public long UserId { get; set; }
    }
}
