using FUNewsSystem.Domain.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Domain.Configs.OData
{
    public static class ODataConfiguration
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Category>("Categories");
            builder.EntitySet<NewsArticle>("NewsArticles");

            builder.EntitySet<SystemAccount>("SystemAccounts")
                   .EntityType.HasKey(sa => sa.AccountId);

            return builder.GetEdmModel();
        }

    }
}
