using AutoVerdikt.Application.Research;
using AutoVerdikt.Application.Users;
using AutoVerdikt.Application.Whitelist;
using AutoVerdikt.Store.Configuration;
using AutoVerdikt.Store.Research;
using AutoVerdikt.Store.Users;
using AutoVerdikt.Store.Waitlist;
using AutoVerdikt.Store.Whitelist;
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

        services.AddSingleton<IMongoCollection<WaitlistRequestDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<WaitlistRequestDocument>("waitlist_requests"));

        services.AddSingleton<IMongoCollection<ResearchDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<ResearchDocument>("research"));

        services.AddSingleton<IMongoCollection<NoteDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<NoteDocument>("research_notes"));

        services.AddSingleton<IMongoCollection<DetailDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<DetailDocument>("research_details"));

        services.AddSingleton<IMongoCollection<AttachedFileDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<AttachedFileDocument>("research_files"));

        services.AddSingleton<IMongoCollection<QuestionDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<QuestionDocument>("research_questions"));

        services.AddSingleton<IMongoCollection<BadgeDocument>>(sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<BadgeDocument>("research_badges"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWhitelistRepository, WhitelistRepository>();
        services.AddScoped<IWaitlistRequestRepository, WaitlistRequestRepository>();
        services.AddScoped<IResearchRepository, ResearchRepository>();
        services.AddHostedService<WhitelistMongoDbInitializer>();
        services.AddHostedService<ResearchMongoDbInitializer>();

        return services;
    }
}
