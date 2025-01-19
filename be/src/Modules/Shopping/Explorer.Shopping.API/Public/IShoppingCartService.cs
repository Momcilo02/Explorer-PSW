using Explorer.Shopping.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Public
{
    public interface IShoppingCartService
    {
        Result<List<ItemDto>> GetPurchasedTours(long userId);
        Result<ShoppingCartDto> GetByUser(long userId);
        Result<ShoppingCartDto> AddItem(ItemDto itemDto, int userId);
        Result<ShoppingCartDto> RemoveItem(ItemDto itemDto, int userId);
        Result<ShoppingCartDto> CheckOut(long userId);
        Result<ShoppingCartDto> CreateCart(int touristId);
        Result<ItemDto> CreateItem(ItemDto item);
        Result<ItemDto> GetById(int id);
        Result<ItemDto> UpdateItemByTourId(int tourId,int price);
        Result<List<PaymentRecordDto>> GetPaymentRecordsByUser(int touristId);
        Result ClearPaymentRecordsByUser(int touristId);

    }
}
