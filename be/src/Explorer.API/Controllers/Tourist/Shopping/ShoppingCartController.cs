using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping;

[Authorize(Policy = "userPolicy")]
[Route("api/shopping/shopping-cart")]
public class ShoppingCartController : BaseApiController
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [HttpGet("{touristId:int}")]
    public ActionResult<ShoppingCartDto> GetByUser(int touristId)
    {
        var result = _shoppingCartService.GetByUser(touristId);
        return CreateResponse(result);
    }
    [HttpGet("purchased/{touristId:int}")]
    public ActionResult<ShoppingCartDto> GetPurchasedTours(int touristId)
    {
        var result = _shoppingCartService.GetPurchasedTours(touristId);
        return CreateResponse(result);
    }
    [HttpPut("add/{touristId:int}")]
    public ActionResult<ShoppingCartDto> AddItem([FromBody] ItemDto orderItem, int touristId)
    {
        var result = _shoppingCartService.AddItem(orderItem, touristId);
        return CreateResponse(result);
    }
    [HttpPost("create/{touristId:int}")]
    public ActionResult<ShoppingCartDto> CreateCart(int touristId)
    {
        var result = _shoppingCartService.CreateCart(touristId);
        return CreateResponse(result);
    }
    [HttpDelete("payment-records/{touristId:int}")]
    public IActionResult ClearPaymentRecords(int touristId)
    {
        var result = _shoppingCartService.ClearPaymentRecordsByUser(touristId);
        return CreateResponse(result);
    }

    [HttpPost("create/item")]
    public ActionResult<ItemDto> CreateItem([FromBody] TourDto tour)
    {
        
        if (_shoppingCartService.GetById(tour.Id)!=null)
        {
            return CreateResponse(_shoppingCartService.UpdateItemByTourId(tour.Id,(int)tour.Cost));
        }
        else
        {
            ItemDto item = new ItemDto();
            item.SellerId = tour.AuthorId;
            item.ItemId = tour.Id;
            item.Price = tour.Cost;
            item.Name = tour.Name;
            item.Type = ItemType.Tour;
            return CreateResponse(_shoppingCartService.CreateItem(item));
        }
    }

    [HttpPut("remove/{touristId:int}")]
    public ActionResult<ShoppingCartDto> RemoveItem([FromBody] ItemDto orderItem, int touristId)
    {
        var result = _shoppingCartService.RemoveItem(orderItem, touristId);
        return CreateResponse(result);
    }
    [HttpPut("checkout/{touristId:int}")]
    public ActionResult<ShoppingCartDto> Checkout(int touristId)
    {
        var result = _shoppingCartService.CheckOut(touristId);
        return CreateResponse(result);
    }
    [HttpGet("payment-records/{touristId:int}")]
    public ActionResult<List<PaymentRecordDto>> GetPaymentRecords(int touristId)
    {
        var result = _shoppingCartService.GetPaymentRecordsByUser(touristId);
        return CreateResponse(result);
    }

    [HttpPost("create/item/bundle")]
    public ActionResult<ItemDto> CreateBundleItem([FromBody] BundleDto bundle)
    {

        if (_shoppingCartService.GetById((int)bundle.Id) != null)
        {
            return CreateResponse(_shoppingCartService.UpdateItemByTourId((int)bundle.Id, (int)bundle.Price));
        }
        else
        {
            ItemDto item = new ItemDto 
            { 
                SellerId = bundle.CreatorId,
                Name = bundle.Name,
                Price = bundle.Price,
                ItemId = bundle.Id,
                Type = ItemType.Bundle
            };
            return CreateResponse(_shoppingCartService.CreateItem(item));
        }
    }
}