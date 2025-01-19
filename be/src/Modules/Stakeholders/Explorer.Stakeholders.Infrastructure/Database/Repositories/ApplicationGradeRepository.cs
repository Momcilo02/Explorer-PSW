using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Infrastructure.Database;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ApplicationGradeRepository : IApplicationGradeRepository
    {
        private readonly StakeholdersContext _context;

        public ApplicationGradeRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public void Add(ApplicationGrade applicationGrade)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicationGrade Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationGrade> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationGrade> GetByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationGrade applicationGrade)
        {
            throw new NotImplementedException();
        }

        //public ApplicationGrade Get(int id)
        //{
        //    return _context.ApplicationGrades.Find(id);
        //}

        //public IEnumerable<ApplicationGrade> GetAll()
        //{
        //    return _context.ApplicationGrades.ToList();
        //}

        //public IEnumerable<ApplicationGrade> GetByUserId(long userId)
        //{
        //    return _context.ApplicationGrades.Where(g => g.UserId == userId).ToList();
        //}

        //public void Add(ApplicationGrade applicationGrade)
        //{
        //    _context.ApplicationGrades.Add(applicationGrade);
        //    _context.SaveChanges();
        //}

        //public void Update(ApplicationGrade applicationGrade)
        //{
        //    _context.ApplicationGrades.Update(applicationGrade);
        //    _context.SaveChanges();
        //}

        //public void Delete(int id)
        //{
        //    var applicationGrade = _context.ApplicationGrades.Find(id);
        //    if (applicationGrade != null)
        //    {
        //        _context.ApplicationGrades.Remove(applicationGrade);
        //        _context.SaveChanges();
        //    }
        //}
    }
}
