using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class KeyPointRepository : IKeyPointRepository
{
    private readonly ToursContext _dbContext;
    private readonly DbSet<KeyPoint> _dbSet;

    public KeyPointRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<KeyPoint>();
    }

    public KeyPoint Create(KeyPoint keyPoint)
    {
        _dbSet.Add(keyPoint);
        _dbContext.SaveChanges();
        return keyPoint;
    }

    public void Delete(long id)
    {
        var entity = _dbSet.FirstOrDefault(k => k.Id == id);
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
    }

    public List<KeyPoint> GetAll()
    {
        return _dbSet.ToList();
    }

    public KeyPoint Update(KeyPoint keyPoint)
    {
        try
        {
            if(keyPoint.Id == 0)
                Create(keyPoint);
            _dbContext.Update(keyPoint);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        return keyPoint;
    }
    public KeyPoint Get(long id)
    {
        var entity = _dbSet.FirstOrDefault(k => k.Id == id);
        return entity;
    }

    public bool DoesExistByCoordinates(float longitude, float latitude)
    {
        var entity = _dbSet.FirstOrDefault(k => k.Longitude == longitude && k.Latitude == latitude);
        if (entity == null) return false;
        return true;
    }
    public PagedResult<KeyPoint> GetPublicKeyPoints(int page, int pageSize)
    {
        var entity = _dbContext.KeyPoints.Where(k => k.Status == KeyPoint.PublicStatus.PUBLIC).GetPagedById(page, pageSize);
        return entity.Result;
    }
}
