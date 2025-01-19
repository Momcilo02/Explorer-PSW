using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces;

public interface IProductRepository
{
    Product Create(Product product);
    void Delete(long id);
}
