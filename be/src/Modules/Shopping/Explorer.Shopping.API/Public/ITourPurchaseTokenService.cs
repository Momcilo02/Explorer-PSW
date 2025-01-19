using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Public;

public interface ITourPurchaseTokenService
{
    bool ExistsByTourAndUser(int tourId, int touristId);

    List<long> GetTouristPurchases(int touristId);
}
