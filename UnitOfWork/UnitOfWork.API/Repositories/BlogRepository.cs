using Microsoft.EntityFrameworkCore;
using UnitOfWork.API.Entities;
using UnitOfWork.Interfaces;

namespace UnitOfWork.API.Repositories
{
    public class BlogRepository : Repository<Blog>, IRepository<Blog>
    {
        public BlogRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
