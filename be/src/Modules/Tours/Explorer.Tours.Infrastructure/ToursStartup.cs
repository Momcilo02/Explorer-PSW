using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TourObject = Explorer.Tours.Core.Domain.TourObject;

namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<ITourEquipmentService, TourEquipmentService>();
        services.AddScoped<ITourReviewService, TourReviewService>();
        services.AddScoped<IKeyPointService, KeyPointService>();
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<ITourObjectService, TourObjectService>();
        services.AddScoped<ITourExecutionService, TourExecutionService>();
        services.AddScoped<IInternalTourService, TourService>();
        services.AddScoped<ITouristLocationService, TouristLocationService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<ITouristEquipmentService, TouristEquipmentService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourEquipment>), typeof(CrudDatabaseRepository<TourEquipment, ToursContext>));
        services.AddScoped(typeof(ITourReviewRepository), typeof(TourReviewRepository));
        services.AddScoped(typeof(ICrudRepository<TourReview>), typeof(CrudDatabaseRepository<TourReview, ToursContext>));
        services.AddScoped(typeof(IKeyPointRepository), typeof(KeyPointRepository)); 
        services.AddScoped(typeof(ICrudRepository<Tour>), typeof(CrudDatabaseRepository<Tour, ToursContext>));
        services.AddScoped(typeof(ITourRepository), typeof(TourRepository));
        services.AddScoped(typeof(ICrudRepository<TourObject>), typeof(CrudDatabaseRepository<TourObject, ToursContext>));
        services.AddScoped(typeof(ITourObjectRepository), typeof(TourObjectRepository));
        services.AddScoped(typeof(ITourExecutionRepository), typeof(TourExecutionRepository));
        services.AddScoped(typeof(ICrudRepository<TouristLocation>), typeof(CrudDatabaseRepository<TouristLocation, ToursContext>));
        services.AddScoped(typeof(IQuizRepository), typeof(QuizRepository));
        services.AddScoped(typeof(ICrudRepository<Quiz>),typeof(CrudDatabaseRepository<Quiz, ToursContext>));
        services.AddScoped(typeof(ITouristLocationRepository), typeof(TouristLocationRepository));

        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
        services.AddScoped(typeof(ICrudRepository<TouristEquipment>), typeof(CrudDatabaseRepository<TouristEquipment, ToursContext>));
    }
}