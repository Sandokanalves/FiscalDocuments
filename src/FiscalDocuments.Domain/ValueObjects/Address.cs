namespace FiscalDocuments.Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string District { get; private set; } = string.Empty;
    public string CityCode { get; private set; } = string.Empty;
    public string CityName { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;

    private Address() { }

    public Address(string street, string number, string district, string cityCode, string cityName, string state, string zipCode)
    {
        Street = street;
        Number = number;
        District = district;
        CityCode = cityCode;
        CityName = cityName;
        State = state;
        ZipCode = zipCode;
    }
}

