using AutoMapper;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Mappers
{
    public class ShoppingProfile:Profile
    {
        public ShoppingProfile()
        {
            CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
            CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
            CreateMap<ItemDto, OrderItem>().ReverseMap();
            CreateMap<ItemDto, Item>().ReverseMap();
            CreateMap<CouponDto, Coupon>().ReverseMap();
            CreateMap<TouristWalletDto, TouristWallet>().ReverseMap();
            CreateMap<BundleDto, Bundle>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<PaymentRecord, PaymentRecordDto>().ReverseMap();
        }
    }
}
