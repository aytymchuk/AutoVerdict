using AutoVerdikt.Domain.Research;

namespace AutoVerdikt.Store.Research;

internal static class CarDataMapper
{
    public static CarDataDocument ToDocument(CarData car) =>
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

    public static CarData ToDomain(CarDataDocument document) =>
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
}
