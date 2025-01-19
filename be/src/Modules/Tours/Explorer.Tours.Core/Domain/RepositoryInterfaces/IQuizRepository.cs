using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface IQuizRepository
    {
        Quiz Create(Quiz quiz);
        Quiz Get(long id);
        IEnumerable<Quiz> GetAll();
        Quiz? GetById(int id);

        void Update(Quiz quiz);

        void Delete(Quiz quiz);

        Quiz? GetByTourId(int tourId);
    }
}
