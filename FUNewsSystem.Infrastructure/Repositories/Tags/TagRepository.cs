using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.NewsArticles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Infrastructure.Repositories.Tags
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(FunewsSystemApiDbContext context) : base(context)
        {
        }
    }
}
