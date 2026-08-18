using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.WebApi.Endpoints.Research;

internal static class ResearchDtoMappings
{
    internal static InputMethod ToInputMethod(this string inputMethod) => inputMethod switch
    {
        "form" => InputMethod.Form,
        "text" => InputMethod.Text,
        _ => throw new ArgumentOutOfRangeException(nameof(inputMethod), inputMethod, null)
    };

    internal static CarData ToDomain(this CreateCarDataDto dto) =>
        new()
        {
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            MileageKm = dto.MileageKm,
            Price = dto.Price,
            Currency = dto.Currency,
            Vin = dto.Vin,
            FuelType = dto.FuelType,
            Transmission = dto.Transmission,
            EngineDisplacement = dto.EngineDisplacement,
            Color = dto.Color,
            Condition = dto.Condition
        };

    internal static ResearchDetailDto ToDetailDto(this ResearchRecord record) =>
        new(
            record.Id,
            record.Name,
            ToStatusString(record.Status),
            ToRiskLevelString(record.RiskLevel),
            ToInputMethodString(record.InputMethod),
            record.Car?.ToCarDataDto(),
            record.Description,
            ToDescriptionSourceString(record.DescriptionSource),
            record.InitialPrompt,
            record.CreditsSpent,
            record.CreatedAt,
            record.UpdatedAt);

    internal static ResearchListItemDto ToListItemDto(this ResearchRecord record) =>
        new(
            record.Id,
            record.Name,
            ToStatusString(record.Status),
            ToRiskLevelString(record.RiskLevel),
            record.Car?.ToCarSummaryDto(),
            record.CreditsSpent,
            record.UpdatedAt);

    private static CarDataDto ToCarDataDto(this CarData car) =>
        new(
            car.Make,
            car.Model,
            car.Year,
            car.MileageKm,
            car.Price,
            car.Currency,
            car.Vin,
            car.FuelType,
            car.Transmission,
            car.EngineDisplacement,
            car.Color,
            car.Condition);

    private static ResearchCarSummaryDto ToCarSummaryDto(this CarData car) =>
        new(car.Make, car.Model, car.Year, car.MileageKm, car.Price);

    private static string ToStatusString(ResearchStatus status) => status switch
    {
        ResearchStatus.Draft => "draft",
        ResearchStatus.Pending => "pending",
        ResearchStatus.Analyzing => "analyzing",
        ResearchStatus.Analyzed => "analyzed",
        ResearchStatus.Failed => "failed",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static string? ToRiskLevelString(RiskLevel? riskLevel) => riskLevel switch
    {
        null => null,
        RiskLevel.Low => "low",
        RiskLevel.Medium => "medium",
        RiskLevel.High => "high",
        _ => throw new ArgumentOutOfRangeException(nameof(riskLevel), riskLevel, null)
    };

    private static string ToInputMethodString(InputMethod inputMethod) => inputMethod switch
    {
        InputMethod.Form => "form",
        InputMethod.Text => "text",
        _ => throw new ArgumentOutOfRangeException(nameof(inputMethod), inputMethod, null)
    };

    private static string? ToDescriptionSourceString(DescriptionSource? descriptionSource) =>
        descriptionSource switch
        {
            null => null,
            DescriptionSource.AiGeneratedFromText => "aiGeneratedFromText",
            DescriptionSource.AiGeneratedFromFacts => "aiGeneratedFromFacts",
            _ => throw new ArgumentOutOfRangeException(nameof(descriptionSource), descriptionSource, null)
        };
}
