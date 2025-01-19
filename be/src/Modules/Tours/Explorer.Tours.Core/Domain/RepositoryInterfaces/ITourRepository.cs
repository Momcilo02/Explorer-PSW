using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        Tour Get(long id);
        List<Tour> GetByStatus(int status);
        PagedResult<Tour> GetPaged(int page, int pageSize);
        void Delete(long id);
        void DeleteEquipmenmts(long id);
        PagedResult<Tour> GetPublishedTours(int page, int pageSize);
        Tour Create(Tour tour);
        Tour Update(Tour tour);
        PagedResult<Tour> GetByAuthorId(int id, int page, int pageSize);
        List<Tour> GetPublishedToursList();
        List<Tour> GetAll();
        Tour GetPublishedTourByid(long id);
    }
}
