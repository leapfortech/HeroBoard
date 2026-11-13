using System;

public class LocalityFull
{
    public long LocalityTypeId { get; set; } = -1L;
    public long CountryId { get; set; } = -1L;
    public long StateId { get; set; } = -1L;
    public long CityId { get; set; } = -1L;


    public LocalityFull()
    {
    }

    public LocalityFull(long localityTypeId, long countryId, long stateId, long cityId)
    {
        LocalityTypeId = localityTypeId;
        CountryId = countryId;
        StateId = stateId;
        CityId = cityId;
    }
}
