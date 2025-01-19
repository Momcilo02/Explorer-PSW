using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ShoppingContext _shoppingContext;
    private readonly DbSet<Product> _dbSet;

    public ProductRepository(ShoppingContext shoppingContext)
    {
        _shoppingContext = shoppingContext;
        _dbSet = _shoppingContext.Set<Product>();
    }

    public Product Create(Product product)
    {
        _dbSet.Add(product);
        _shoppingContext.SaveChanges();
        return product;
    }

    public void Delete(long id)
    {
        var entity = _dbSet.FirstOrDefault(p => p.Id == id);
        _dbSet.Remove(entity);
        _shoppingContext.SaveChanges();
    }
}
