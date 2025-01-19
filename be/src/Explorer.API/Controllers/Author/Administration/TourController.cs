using Microsoft.AspNetCore.Authorization;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Stakeholders.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using Explorer.Stakeholders.Infrastructure.Authentication;

namespace Explorer.API.Controllers.Author.Administration
{
    [Route("api/administration/tour")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        [Authorize(Policy = "authorPolicy")]
        public ActionResult<TourDto> Create([FromBody] TourDto tour)
        {
            tour.AuthorId = this.User.PersonId();
            var result = _tourService.Create(tour);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "authorPolicy")]
        public ActionResult<TourDto> Update([FromBody] TourDto tour)
        {
            if (this.User.PersonId() != tour.AuthorId)
                return Unauthorized("You can not update this tour because this tour is not yours!");
            _tourService.DeleteEquipments(tour.Id);
            var result = _tourService.Update(tour);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "authorPolicy")]
        public ActionResult Delete(long id)
        {
            var result = _tourService.Delete(id);
            return CreateResponse(result);
        }
        [HttpGet("{id:int}")]
        public ActionResult<PagedResult<TourDto>> Get(long id)
        {
            var result = _tourService.Get(id);
            return CreateResponse(result);
        }

        [HttpPut]
        [Route("publish/{id:int}")]
        [Authorize(Policy = "authorPolicy")]
        public ActionResult<TourDto> Publish([FromBody] TourDto tour)
        {
            if(this.User.PersonId() != tour.AuthorId)
                return Unauthorized("You can not publish this tour because this tour is not yours!");
            var result = _tourService.Publish(tour);
            return CreateResponse(result);
        }

        [HttpPut]
        [Route("archive/{id:int}")]
        public ActionResult<TourDto> Archive([FromBody] TourDto tour)
        {
            if (this.User.PersonId() != tour.AuthorId)
                return Unauthorized("You can not archive this tour because this tour is not yours!");
            var result = _tourService.Archive(tour);
            return CreateResponse(result);
        }

        [HttpPut]
        [Route("length/{id:int}")]
        public ActionResult<TourDto> UpdateTourLength([FromBody] TourDto tour)
        {
            if (this.User.PersonId() != tour.AuthorId)
                return Unauthorized("You can not update length on this tour because this tour is not yours!");
            var result = _tourService.UpdateTourLength(tour);
            return CreateResponse(result);
        }

        [HttpPut]
        [Route("reactivate/{id:int}")]
        public ActionResult<TourDto> ReactivateTour([FromBody] TourDto tour)
        {
            if (this.User.PersonId() != tour.AuthorId)
                return Unauthorized("You can not reactive this tour because this tour is not yours!");
            var result = _tourService.ReactivateTour(tour);
            return CreateResponse(result);
        }

        [HttpGet]
        [Route("user")]
        public ActionResult<PagedResult<TourDto>> GetForUser([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetByAuthorId(this.User.PersonId(), page, pageSize);
            return CreateResponse(result);
        }
        [HttpGet]
        [Route("public/{id:int}")]
        public ActionResult<List<KeyPointDto>> GetPublic(int id)
        {
            var result = _tourService.GetAvailableKeyPoints(id);
            return CreateResponse(result);
        }
    }
}
