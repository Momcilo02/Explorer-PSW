using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository;

public class BundleRepository : IBundleRepository
{
    private readonly ShoppingContext _shoppingContext;
    private readonly DbSet<Bundle> _dbSet;

    public BundleRepository(ShoppingContext shoppingContext)
    {
        _shoppingContext = shoppingContext;
        _dbSet = shoppingContext.Set<Bundle>();
    }

    public Bundle Create(Bundle bundle)
    {
        _dbSet.Add(bundle);
        _shoppingContext.SaveChanges();
        return bundle;
    }

    public void Delete(long id)
    {
        var entity = _dbSet.FirstOrDefault(b => b.Id == id);
        if (entity == null) 
            throw new Exception("Bundle with this id does not exist.");
        if(entity.Status == BundleStatus.Published)
            throw new Exception("Bundle has been published, you cannot delete it. You can archive it.");
        _dbSet.Remove(entity);
        _shoppingContext.SaveChanges();
    }

    public Bundle Get(long id)
    {
        return _dbSet.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
    }

    public PagedResult<Bundle> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.Include(b => b.Products).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public Bundle Update(Bundle bundle)
    {
        var existingBundle = _dbSet
            .Include(b => b.Products)
            .FirstOrDefault(b => b.Id == bundle.Id);

        if (existingBundle == null)
            throw new Exception("Bundle not found.");

        _shoppingContext.Entry(existingBundle).CurrentValues.SetValues(bundle);

        foreach (var product in bundle.Products)
        {
            var existingProduct = existingBundle.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                existingBundle.Products.Add(product);

        }

        var productsToRemove = existingBundle.Products
            .Where(p => !bundle.Products.Any(bp => bp.Id == p.Id))
            .ToList();

        foreach (var product in productsToRemove)
        {
            existingBundle.Products.Remove(product);
            _shoppingContext.Products.Remove(product);
            _shoppingContext.SaveChanges();
        }
        _shoppingContext.SaveChanges();

        return existingBundle;
    }


    public PagedResult<Bundle> GetPagedByCreatorId(long creatorId, int page, int pageSize)
    {
        var task = _dbSet.Include(b => b.Products).Where(b => b.CreatorId == creatorId).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Bundle> GetPublished(int page, int pageSize)
    {
        var task = _dbSet.Include(b => b.Products).Where(b => b.Status == BundleStatus.Published).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public Bundle GetById(long id)
    {
        return _dbSet.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
    }
}
