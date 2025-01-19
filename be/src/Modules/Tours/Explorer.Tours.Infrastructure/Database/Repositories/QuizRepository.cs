using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ToursContext _dbContext;
        private readonly DbSet<Quiz> _dbSet;

        public Quiz? GetById(int id)
        {
            return _dbSet.Include(q => q.Questions)
                         .ThenInclude(a => a.Answers)
                         .FirstOrDefault(q => q.Id == id);
        }
        public Quiz? GetByTourId(int tourId)
        {
            return _dbSet
                           .Include(q => q.Questions)
                           .ThenInclude(q => q.Answers)
                           .Include(q => q.Reward)
                           .FirstOrDefault(q => q.TourId == tourId);
        }
        public void Update(Quiz quiz)
        {
            _dbSet.Update(quiz);
            _dbContext.SaveChanges();
        }

        public void Delete(Quiz quiz)
        {
            _dbSet.Remove(quiz);
            _dbContext.SaveChanges();
        }

        public QuizRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Quiz>();
        }

        public Quiz Create(Quiz quiz)
        {
            _dbSet.Add(quiz);
            _dbContext.SaveChanges();
            return quiz;
        }

        public Quiz Get(long id)
        {
            return _dbSet.Include(q => q.Questions)
                         .ThenInclude(q => q.Answers)
                         .FirstOrDefault(q => q.Id== id);
        }
        public IEnumerable<Quiz> GetAll()
        {
            return _dbContext.Quizzes.ToList();
        }
    }
}
