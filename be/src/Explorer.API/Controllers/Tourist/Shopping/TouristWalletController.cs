using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "administratorAndTouristPolicy")]
    [Route("api/tourist/wallet")]
    public class TouristWalletController : BaseApiController
    {
        private readonly ITouristWalletService _service;

        public TouristWalletController(ITouristWalletService touristWalletService)
        {
            _service = touristWalletService;
        }

        [HttpGet("get-adventure-coins/{userId:long}")]
        public ActionResult<TouristWalletDto> GetAdventureCoins(int userId)
        {
            var result = _service.GetAdventureCoins(userId);
            return CreateResponse(result);
        }

        [HttpPut("payment-adventure-coins/{userId:long}/{adventureCoins:int}")]
        public ActionResult<TouristWalletDto> PaymentAdventureCoins(int userId, int adventureCoins)
        {
            var result = _service.PaymentAdventureCoins(userId, adventureCoins);
            return CreateResponse(result);
        }

        [HttpPost("create-wallet/{touristId}")]
        public ActionResult<TouristWalletDto> CreateWallet(int touristId)
        {;
            var result = _service.CreateWallet(touristId);
            return CreateResponse(result);
        }
    }
}
