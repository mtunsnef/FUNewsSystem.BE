using FUNewsSystem.Domain.Models;

namespace FUNewsSystem.Infrastructure.Repositories.Categories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(FunewsSystemApiContext context) : base(context)
        {
        }
    }
}
