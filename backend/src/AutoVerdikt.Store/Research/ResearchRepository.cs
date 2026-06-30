using AutoVerdikt.Application.Common;
using AutoVerdikt.Application.Research;
using AutoVerdikt.Application.Research.Errors;
using AutoVerdikt.Domain.Research;
using FluentResults;
using MongoDB.Driver;

namespace AutoVerdikt.Store.Research;

internal sealed class ResearchRepository(IMongoCollection<ResearchDocument> collection) : IResearchRepository
{
    public async Task<Result> CreateAsync(ResearchRecord record, CancellationToken cancellationToken = default)
    {
        try
        {
            await collection.InsertOneAsync(ToDocument(record), cancellationToken: cancellationToken);
            return Result.Ok();
        }
        catch (MongoException)
        {
            return Result.Fail(new Error("Failed to create research record."));
        }
    }

    public async Task<Result<ResearchRecord?>> GetByIdAndAuthIdAsync(
        Guid id,
        string authId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ResearchDocument>.Filter.And(
                Builders<ResearchDocument>.Filter.Eq(r => r.Id, id),
                Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId));

            var document = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return Result.Ok(document is null ? null : ToDomain(document));
        }
        catch (MongoException)
        {
            return Result.Fail<ResearchRecord?>(new Error("Failed to load research record."));
        }
    }

    public async Task<Result> UpdateAsync(
        ResearchRecord record,
        string authId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ResearchDocument>.Filter.And(
                Builders<ResearchDocument>.Filter.Eq(r => r.Id, record.Id),
                Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId));

            var result = await collection.ReplaceOneAsync(
                filter,
                ToDocument(record),
                cancellationToken: cancellationToken);

            if (result.MatchedCount == 0)
                return Result.Fail(new ResearchNotFoundError());

            return Result.Ok();
        }
        catch (MongoException)
        {
            return Result.Fail(new Error("Failed to update research record."));
        }
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        string authId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ResearchDocument>.Filter.And(
                Builders<ResearchDocument>.Filter.Eq(r => r.Id, id),
                Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId));

            var result = await collection.DeleteOneAsync(filter, cancellationToken);
            if (result.DeletedCount == 0)
                return Result.Fail(new ResearchNotFoundError());

            return Result.Ok();
        }
        catch (MongoException)
        {
            return Result.Fail(new Error("Failed to delete research record."));
        }
    }

    public async Task<Result<PaginatedResult<ResearchRecord>>> ListByAuthIdAsync(
        string authId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ResearchDocument>.Filter.Eq(r => r.AuthId, authId);
            var skip = Math.Max(0, (page - 1) * pageSize);
            var total = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var documents = await collection
                .Find(filter)
                .SortByDescending(r => r.UpdatedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            var items = documents.Select(ToDomain).ToList();
            return Result.Ok(new PaginatedResult<ResearchRecord>(items, total));
        }
        catch (MongoException)
        {
            return Result.Fail<PaginatedResult<ResearchRecord>>(new Error("Failed to list research records."));
        }
    }

    private static ResearchDocument ToDocument(ResearchRecord record) =>
        new()
        {
            Id = record.Id,
            AuthId = record.AuthId,
            Name = record.Name,
            Status = ToStatusString(record.Status),
            RiskLevel = ToRiskLevelString(record.RiskLevel),
            InputMethod = ToInputMethodString(record.InputMethod),
            Car = record.Car is null ? null : ToCarDocument(record.Car),
            Description = record.Description,
            DescriptionSource = ToDescriptionSourceString(record.DescriptionSource),
            InitialPrompt = record.InitialPrompt,
            CreditsSpent = record.CreditsSpent,
            IsNameManual = record.IsNameManual,
            CreatedAt = record.CreatedAt.UtcDateTime,
            UpdatedAt = record.UpdatedAt.UtcDateTime
        };

    private static ResearchRecord ToDomain(ResearchDocument document) =>
        new()
        {
            Id = document.Id,
            AuthId = document.AuthId,
            Name = document.Name,
            Status = ParseStatus(document.Status),
            RiskLevel = ParseRiskLevel(document.RiskLevel),
            InputMethod = ParseInputMethod(document.InputMethod),
            Car = document.Car is null ? null : ToCarDomain(document.Car),
            Description = document.Description,
            DescriptionSource = ParseDescriptionSource(document.DescriptionSource),
            InitialPrompt = document.InitialPrompt,
            CreditsSpent = document.CreditsSpent,
            IsNameManual = document.IsNameManual,
            CreatedAt = new DateTimeOffset(document.CreatedAt, TimeSpan.Zero),
            UpdatedAt = new DateTimeOffset(document.UpdatedAt, TimeSpan.Zero)
        };

    private static CarDataDocument ToCarDocument(CarData car) =>
        new()
        {
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            MileageKm = car.MileageKm,
            Price = car.Price,
            Currency = car.Currency,
            Vin = car.Vin,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            EngineDisplacement = car.EngineDisplacement,
            Color = car.Color,
            Condition = car.Condition
        };

    private static CarData ToCarDomain(CarDataDocument document) =>
        new()
        {
            Make = document.Make,
            Model = document.Model,
            Year = document.Year,
            MileageKm = document.MileageKm,
            Price = document.Price,
            Currency = document.Currency,
            Vin = document.Vin,
            FuelType = document.FuelType,
            Transmission = document.Transmission,
            EngineDisplacement = document.EngineDisplacement,
            Color = document.Color,
            Condition = document.Condition
        };

    private static string ToInputMethodString(InputMethod inputMethod) => inputMethod switch
    {
        InputMethod.Form => "form",
        InputMethod.Text => "text",
        _ => throw new ArgumentOutOfRangeException(nameof(inputMethod), inputMethod, null)
    };

    private static InputMethod ParseInputMethod(string value) => value switch
    {
        "form" => InputMethod.Form,
        "text" or "paste" => InputMethod.Text,
        _ => InputMethod.Form
    };

    private static string ToStatusString(ResearchStatus status) => status switch
    {
        ResearchStatus.Draft => "draft",
        ResearchStatus.Pending => "pending",
        ResearchStatus.Analyzing => "analyzing",
        ResearchStatus.Analyzed => "analyzed",
        ResearchStatus.Failed => "failed",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static ResearchStatus ParseStatus(string? value) => value switch
    {
        null or "draft" => ResearchStatus.Draft,
        "pending" => ResearchStatus.Pending,
        "analyzing" => ResearchStatus.Analyzing,
        "analyzed" => ResearchStatus.Analyzed,
        "failed" => ResearchStatus.Failed,
        _ => ResearchStatus.Draft
    };

    private static string? ToRiskLevelString(RiskLevel? riskLevel) => riskLevel switch
    {
        null => null,
        RiskLevel.Low => "low",
        RiskLevel.Medium => "medium",
        RiskLevel.High => "high",
        _ => throw new ArgumentOutOfRangeException(nameof(riskLevel), riskLevel, null)
    };

    private static RiskLevel? ParseRiskLevel(string? value) => value switch
    {
        null => null,
        "low" => RiskLevel.Low,
        "medium" => RiskLevel.Medium,
        "high" => RiskLevel.High,
        _ => null
    };

    private static string? ToDescriptionSourceString(DescriptionSource? descriptionSource) =>
        descriptionSource switch
        {
            null => null,
            DescriptionSource.AiGeneratedFromText => "aiGeneratedFromText",
            _ => throw new ArgumentOutOfRangeException(nameof(descriptionSource), descriptionSource, null)
        };

    private static DescriptionSource? ParseDescriptionSource(string? value) => value switch
    {
        null => null,
        "aiGeneratedFromText" => DescriptionSource.AiGeneratedFromText,
        _ => null
    };
}
