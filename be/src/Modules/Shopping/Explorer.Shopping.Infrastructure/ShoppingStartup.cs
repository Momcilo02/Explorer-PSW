using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.UseCases;
using Explorer.Shopping.API.Public;
using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.RepositoryInterfaces;
using Explorer.Shopping.Core.Mappers;
using Explorer.Shopping.Core.UseCases;
using Explorer.Shopping.Infrastructure.Database;
using Explorer.Shopping.Infrastructure.Database.Repository;
using Explorer.Shopping.Infrastructure.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Shopping.Infrastructure
{
    public static class ShoppingStartup
    {
        public static IServiceCollection ConfigureShoppingModule(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ShoppingProfile).Assembly);
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ITourPurchaseTokenService, TourPurchaseTokenService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<ITouristWalletService, TouristWalletService>();
            services.AddScoped<INotificationHandler, NotificationHandler>();
            services.AddScoped<IBundleService, BundleService>();
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<ITourPurchaseTokenRepository, TourPurchaseTokenDatabaseRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICrudRepository<Coupon>, CrudDatabaseRepository<Coupon, ShoppingContext>>(); 
            services.AddScoped<ITouristWalletRepository, TouristWalletRepository>();
            services.AddScoped<IBundleRepository, BundleRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentRecordRepository, PaymentRecordRepository>();
            services.AddDbContext<ShoppingContext>(opt =>
                opt.UseNpgsql(DbConnectionStringBuilder.Build("shopping"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "shopping")));
        }
    }
}
