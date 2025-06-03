using FUNewsSystem.Domain.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace FUNewsSystem.Domain.Configs.OData
{
    public static class ODataConfiguration
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            var categoryEntity = builder.EntitySet<Category>("CategoryOData").EntityType;
            var newsEntity = builder.EntitySet<NewsArticle>("NewsArticleOData").EntityType;
            var tagEntity = builder.EntitySet<Tag>("TagOData").EntityType;
            var accountEntity = builder.EntitySet<SystemAccount>("SystemAccountOData").EntityType;
            accountEntity.HasKey(sa => sa.AccountId);

            categoryEntity.HasMany(c => c.NewsArticles);
            newsEntity.HasMany(n => n.Tags);
            tagEntity.HasMany(t => t.NewsArticles);
            newsEntity.HasOptional(n => n.CreatedBy);
            accountEntity.HasMany(sa => sa.NewsArticles);

            return builder.GetEdmModel();
        }
    }
}
