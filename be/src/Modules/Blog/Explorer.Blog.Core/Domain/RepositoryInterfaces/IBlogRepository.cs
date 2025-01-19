using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces
{
    public interface IBlogRepository
    {
        Blog Get(long id);
        List<Blog> GetByActivityStatus(BlogActivityStatus status);
        PagedResult<Blog> GetPaged(int page, int pageSize);
        Blog Update(Blog blog);
        Blog Create(Blog blog);
        List<Blog> GetActiveBlogs();
        List<Blog> GetFamousBlogs();
    }
}
