using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Internal
{
    public interface IInternalTourService
    {
        Result<TourDto> Get(long tourId);
        Result<List<TourDto>> GetMany(List<long> tourIds);
        Result<TourDto> CloseTour(int id);
    }
}
