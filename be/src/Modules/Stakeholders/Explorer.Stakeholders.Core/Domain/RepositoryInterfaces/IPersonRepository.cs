using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IPersonRepository
    {
        ICollection<Person> GetAll();
        Person GetByUserId(int id);
        void Create(Person person);
    }
}
