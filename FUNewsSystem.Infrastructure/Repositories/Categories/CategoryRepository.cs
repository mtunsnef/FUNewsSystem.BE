using FUNewsSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsSystem.Infrastructure.Repositories.Categories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(FunewsSystemApiDbContext context) : base(context)
        { }
        //public override IQueryable<Category> GetAll()
        //{
        //    return _dbSet.Include(n => n.NewsArticles).AsQueryable();
        //}
    }
}
