using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class PersonDatabaseRepository: IPersonRepository
    {
        private readonly StakeholdersContext _dbContext;

        public PersonDatabaseRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Person> GetAll()
        {
            return _dbContext.People.ToList();
        }

        public Person GetByUserId(int id)
        {
            return _dbContext.People.SingleOrDefault(p => p.UserId == id);
        }

        public void Create(Person person)
        {
            _dbContext.People.Add(person);
            _dbContext.SaveChanges();
        }
    }
}
