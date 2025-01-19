using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using AutoMapper;
using Explorer.Shopping.API.Dtos;

namespace Explorer.Shopping.Core.UseCases;

public class TourPurchaseTokenService : BaseService<TourPurchaseTokenDto, TourPurchaseToken>, ITourPurchaseTokenService
{
    private readonly IMapper _mapper;
    private readonly ITourPurchaseTokenRepository _purchaseTokenRepository;

    public TourPurchaseTokenService(ITourPurchaseTokenRepository repository, IMapper mapper) : base(mapper)
    {
        _mapper = mapper;
        _purchaseTokenRepository = repository;
    }

    public bool ExistsByTourAndUser(int tourId, int touristId)
    {
        return _purchaseTokenRepository.ExistsByTourAndUser(tourId, touristId);
    }

    public List<long> GetTouristPurchases(int touristId)
    {
        return _purchaseTokenRepository.GetItemsByTouristId(touristId);
    }
}
