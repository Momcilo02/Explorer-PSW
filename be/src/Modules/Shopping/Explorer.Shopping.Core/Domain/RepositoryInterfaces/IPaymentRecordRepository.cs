using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain.RepositoryInterfaces;

public interface IPaymentRecordRepository
{
    PaymentRecord Create(PaymentRecord paymentRecord);
    List<PaymentRecord> GetByUser(int userId);

    void DeleteRange(IEnumerable<PaymentRecord> records);

}
