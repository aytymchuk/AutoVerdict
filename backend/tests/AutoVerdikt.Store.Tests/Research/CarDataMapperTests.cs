using AutoVerdikt.Domain.Research;
using AutoVerdikt.Store.Research;
using Shouldly;

namespace AutoVerdikt.Store.Tests.Research;

public class CarDataMapperTests
{
    [Fact]
    public void ToDomain_ToDocument_RoundTripsAllFields()
    {
        var car = new CarData
        {
            Make = "Volkswagen",
            Model = "Golf",
            Year = 2018,
            MileageKm = 87200,
            Price = 42900m,
            Currency = "PLN",
            Vin = "WVWZZZ1KZAW123456",
            FuelType = "Petrol",
            Transmission = "Manual",
            EngineDisplacement = "1.4",
            Color = "Blue",
            Condition = "Used"
        };

        var document = CarDataMapper.ToDocument(car);
        var roundTripped = CarDataMapper.ToDomain(document);

        roundTripped.Make.ShouldBe(car.Make);
        roundTripped.Model.ShouldBe(car.Model);
        roundTripped.Year.ShouldBe(car.Year);
        roundTripped.MileageKm.ShouldBe(car.MileageKm);
        roundTripped.Price.ShouldBe(car.Price);
        roundTripped.Currency.ShouldBe(car.Currency);
        roundTripped.Vin.ShouldBe(car.Vin);
        roundTripped.FuelType.ShouldBe(car.FuelType);
        roundTripped.Transmission.ShouldBe(car.Transmission);
        roundTripped.EngineDisplacement.ShouldBe(car.EngineDisplacement);
        roundTripped.Color.ShouldBe(car.Color);
        roundTripped.Condition.ShouldBe(car.Condition);
    }

    [Fact]
    public void ToDomain_NullOptionalFields_RemainNull()
    {
        var document = new CarDataDocument
        {
            Make = "Volkswagen",
            Model = "Golf",
            Year = 2018
        };

        var car = CarDataMapper.ToDomain(document);

        car.Make.ShouldBe("Volkswagen");
        car.Model.ShouldBe("Golf");
        car.Year.ShouldBe(2018);
        car.MileageKm.ShouldBeNull();
        car.Price.ShouldBeNull();
        car.Currency.ShouldBeNull();
        car.Vin.ShouldBeNull();
        car.FuelType.ShouldBeNull();
        car.Transmission.ShouldBeNull();
        car.EngineDisplacement.ShouldBeNull();
        car.Color.ShouldBeNull();
        car.Condition.ShouldBeNull();
    }
}
