using Explorer.Blog.Infrastructure.Database;
using Explorer.BuildingBlocks.Tests;
using Explorer.Shopping.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Tests
{
    public class ShoppingTestFactory: BaseTestFactory<ShoppingContext>
    {
        protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ShoppingContext>));
            services.Remove(descriptor!);
            services.AddDbContext<ShoppingContext>(SetupTestContext());

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ToursContext>));
            services.Remove(descriptor!);
            services.AddDbContext<ToursContext>(SetupTestContext());

            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<StakeholdersContext>));
            services.Remove(descriptor!);
            services.AddDbContext<StakeholdersContext>(SetupTestContext());

            return services;
        }
    }
}
