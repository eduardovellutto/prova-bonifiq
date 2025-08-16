using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Contracts;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Extensions
{
    public static class BuilderExtension
    {
        public static void AddDatabaseContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<TestDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("ctx"))
                );
        }

        public static void AddServicesDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRandonService, RandomService>();
        }
    }
}
