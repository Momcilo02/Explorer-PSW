using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Infrastructure.Database.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext _context;
        private readonly DbSet<Core.Domain.Blog> _dbSet;
        public BlogRepository(BlogContext context)
        {
            _context = context;
            _dbSet = _context.Set<Core.Domain.Blog>();
        }

        public Core.Domain.Blog Create(Core.Domain.Blog blog)
        {
            _dbSet.Add(blog);
            _context.SaveChanges();
            return blog;
        }

        public Core.Domain.Blog Get(long id)
        {
            return _context.Blogs.Include(b => b.Comments).FirstOrDefault(b => b.Id == id);
        }

        public List<Core.Domain.Blog> GetByActivityStatus(BlogActivityStatus status)
        {
            return _context.Blogs.Include(b => b.Comments).Where(b => b.ActivityStatus == status).ToList();
        }

        public PagedResult<Core.Domain.Blog> GetPaged(int page, int pageSize)
        {
            var task = _context.Blogs.Include(b => b.Comments).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public Core.Domain.Blog Update(Core.Domain.Blog blog)
        {
            try
            {
                _context.Entry(blog).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch(DbUpdateException e)
            {
                throw new KeyNotFoundException(e.Message);
            }
            return blog;
        }
        

        List<Core.Domain.Blog> IBlogRepository.GetActiveBlogs()
        {
            return _context.Blogs
                           .Where(b => b.ActivityStatus == BlogActivityStatus.active)
                           .ToList();
        }

        List<Core.Domain.Blog> IBlogRepository.GetFamousBlogs()
        {
            return _context.Blogs
                           .Where(b => b.ActivityStatus == BlogActivityStatus.famous)
                           .ToList();
        }
    }
}
