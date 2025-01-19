using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Explorer.Shopping.Core.Domain;
using FluentResults;
using Explorer.Shopping.Core.Domain.ShoppingCarts;

namespace Explorer.Shopping.Core.UseCases
{
    public class TouristWalletService : BaseService<TouristWalletDto, TouristWallet>, ITouristWalletService
    {
        private readonly ITouristWalletRepository _repository;
        private readonly INotificationHandler _notificationHandler;

        public TouristWalletService(ITouristWalletRepository repository, IMapper mapper, INotificationHandler notificationHandler)
            : base(mapper)
        {
            _repository = repository;
            _notificationHandler = notificationHandler;
        }

        public Result<TouristWalletDto> CreateWallet(int touristId)
        {
            TouristWallet wallet = new TouristWallet(touristId);
            return MapToDto(_repository.Create(wallet));
        }

        public Result<TouristWalletDto> GetAdventureCoins(long userId)
        {
            var wallet = _repository.GetByUser(userId);
            return MapToDto(wallet);
        }

        public Result<TouristWalletDto> PaymentAdventureCoins(int userId, int coins)
        {
            var wallet = _repository.PaymentAdventureCoins(userId, coins);

            _notificationHandler.Notify(userId, "PAYMENT");

            return MapToDto(wallet);
        }
    }
}
