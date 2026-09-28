using WestCoastFitness.Domain.Common;

namespace WestCoastFitness.Domain.Locations;

public sealed class ClubLocation : Entity
{
    public string Name { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; } = "AZ";
    public string PostalCode { get; private set; }
    public string TimeZone { get; private set; } = "America/Phoenix";
    public decimal CityPrivilegeTaxRate { get; private set; }
    public int StudioCapacity { get; private set; }
    public bool OutdoorPatio { get; private set; }

    private ClubLocation()
    {
        Name = City = PostalCode = string.Empty;
    }

    public ClubLocation(
        string name,
        string city,
        string postalCode,
        decimal cityPrivilegeTaxRate,
        int studioCapacity,
        bool outdoorPatio)
    {
        Name = Guard.Required(name, nameof(name));
        City = Guard.Required(city, nameof(city));
        PostalCode = Guard.Required(postalCode, nameof(postalCode));
        if (cityPrivilegeTaxRate is < 0 or > 0.05m)
        {
            throw new ArgumentOutOfRangeException(nameof(cityPrivilegeTaxRate));
        }

        if (studioCapacity < 8)
        {
            throw new ArgumentOutOfRangeException(nameof(studioCapacity));
        }

        CityPrivilegeTaxRate = cityPrivilegeTaxRate;
        StudioCapacity = studioCapacity;
        OutdoorPatio = outdoorPatio;
    }

    public decimal CombinedTaxRate(decimal stateTransactionPrivilegeRate) =>
        stateTransactionPrivilegeRate + CityPrivilegeTaxRate;
}
