using FUNewsSystem.API.Middlewares;
using FUNewsSystem.Domain.Configs.OData;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Infrastructure.Repositories.Categories;
using FUNewsSystem.Services.AutoMapper;
using FUNewsSystem.Services.Services.Categories;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

namespace FUNewsSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddOData(options => options.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100)
                .AddRouteComponents("odata", ODataConfiguration.GetEdmModel())
            );
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Repository
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            //Service
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            // Add Database configuration
            builder.Services.AddDbContext<FunewsSystemApiContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            //Config automapper
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseGlobalExceptionHandler();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
