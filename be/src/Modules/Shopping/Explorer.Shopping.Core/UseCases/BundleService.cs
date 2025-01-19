using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Shopping.API.Dtos;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Internal;

namespace Explorer.Shopping.Core.UseCases;

public class BundleService : BaseService<BundleDto, Bundle>, IBundleService
{
    private readonly IBundleRepository _bundleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInternalTourService _internalTourService;
    public BundleService(IMapper mapper, IBundleRepository bundleRepository, IProductRepository productRepository, IInternalTourService internalTourService) : base(mapper)
    {
        _bundleRepository = bundleRepository;
        _productRepository = productRepository;
        _internalTourService = internalTourService;
    }

    public Result<BundleDto> Create(BundleDto bundle)
    {
        bundle.Status = API.Dtos.BundleStatus.Draft;
        var result = _bundleRepository.Create(MapToDomain(bundle));
        //foreach(var product in bundle.Products)
        //{
        //    _productRepository.Add(new Product);
        //}
        return MapToDto(result);
    }

    public Result Delete(long id)
    {
        try
        {
            var bundle = _bundleRepository.Get(id);
            if(bundle == null)
                throw new Exception("Bundle with this id does not exist.");
            var productIds = bundle.Products.Select(p => p.Id).ToList();
            foreach (var productId in productIds)
            {
                _productRepository.Delete(productId);
            }
            _bundleRepository.Delete(id);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(FailureCode.NotFound).WithError(ex.Message);
        }
    }

    public Result<BundleDto> Get(long id)
    {
        var res = _bundleRepository.Get(id);
        return MapToDto(res);
    }

    public Result<PagedResult<BundleDto>> GetPaged(int page, int pageSize)
    {
        var result = _bundleRepository.GetPaged(page, pageSize);
        return MapToDto(result);
    }

    public Result<BundleDto> Update(BundleDto bundleDto)
    {
        try
        {
            var result = _bundleRepository.Update(MapToDomain(bundleDto));
            return MapToDto(result);
        }
        catch (Exception e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
       
    }

    public Result<PagedResult<BundleDto>> GetPagedByCreatorId(long creatorId, int page, int pageSize)
    {
        var result = _bundleRepository.GetPagedByCreatorId(creatorId, page, pageSize);
        return MapToDto(result);
    }

    public Result<BundleDto> Publish(BundleDto bundleDto)
    {
        if(!CanPublish(bundleDto))
            return Result.Fail(FailureCode.InvalidArgument).WithError("If you want to publish bundle, number of published tours in bundle have to be 2 or more.");
        bundleDto.Status = API.Dtos.BundleStatus.Published;
        return Update(bundleDto);
    }

    public Result<BundleDto> Archive(BundleDto bundleDto)
    {
        bundleDto.Status = API.Dtos.BundleStatus.Archived;
        return Update(bundleDto);
    }

    private bool CanPublish(BundleDto bundle)
    {
        var tourIds = bundle.Products.Select(p => p.TourId).ToList();
        var tours = _internalTourService.GetMany(tourIds);
        var numberOfPublishedTours = tours.Value.Where(t => t.Status == Tours.API.Dtos.TourStatus.Published).Count();
        return numberOfPublishedTours >= 2;
    }

    public Result<PagedResult<BundleDto>> GetPublished(int page, int pageSize)
    {
        var result = _bundleRepository.GetPublished(page, pageSize);
        return MapToDto(result);
    }
}
