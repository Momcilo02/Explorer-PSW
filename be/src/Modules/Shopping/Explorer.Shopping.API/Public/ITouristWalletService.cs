using Explorer.Shopping.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Public
{
    public interface ITouristWalletService
    {
        Result<TouristWalletDto> GetAdventureCoins(long userId);
        Result<TouristWalletDto> PaymentAdventureCoins(int userId, int coins);
        Result<TouristWalletDto> CreateWallet(int touristId);
    }
}
