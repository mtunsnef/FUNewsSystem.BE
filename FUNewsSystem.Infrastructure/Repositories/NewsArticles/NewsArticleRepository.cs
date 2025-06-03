using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Infrastructure.Repositories.NewsArticles
{
    public class NewsArticleRepository : RepositoryBase<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FunewsSystemContext context) : base(context)
        {
        }
    }
}
