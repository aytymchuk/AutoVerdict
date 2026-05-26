using AutoVerdikt.Application.Users;
using AutoVerdikt.Store.Configuration;
using AutoVerdikt.Store.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AutoVerdikt.Store;

public static class StoreServiceExtensions
{
    public static IServiceCollection AddStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoDbOptions>()
            .Bind(configuration.GetSection(MongoDbOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
                "MongoDb:ConnectionString is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName),
                "MongoDb:DatabaseName is required.")
            .ValidateOnStart();

        services.AddSingleton<IMongoClient>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            return new MongoClient(opts.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(opts.DatabaseName);
        });

        services.AddSingleton<IMongoCollection<UserDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<UserDocument>("users"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddHostedService<MongoDbInitializer>();

        return services;
    }
}
