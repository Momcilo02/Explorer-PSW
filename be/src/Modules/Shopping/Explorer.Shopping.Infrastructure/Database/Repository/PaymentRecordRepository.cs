using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Infrastructure.Database.Repository;

public class PaymentRecordRepository : IPaymentRecordRepository
{
    private readonly ShoppingContext _shoppingContext;
    private readonly DbSet<PaymentRecord> _dbSet;

    public PaymentRecordRepository(ShoppingContext shoppingContext)
    {
        _shoppingContext = shoppingContext;
        _dbSet = _shoppingContext.Set<PaymentRecord>();
    }
    public void DeleteRange(IEnumerable<PaymentRecord> records)
    {
        _dbSet.RemoveRange(records);
        _shoppingContext.SaveChanges();
    }
    public List<PaymentRecord> GetByUser(int userId)
    {
        return _dbSet.Where(pr => pr.TouristId == userId).ToList();
    }

    public PaymentRecord Create(PaymentRecord paymentRecord)
    {
        _dbSet.Add(paymentRecord);
        _shoppingContext.SaveChanges();
        return paymentRecord;
    }
}
