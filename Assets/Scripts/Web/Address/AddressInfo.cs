using System;

public class AddressInfo
{
    public Address Address { get; set; }
    public String[] HouseholdBills { get; set; }

    public AddressInfo()
    {
    }

    public AddressInfo(Address address, String[] householdBills = null)
    {
        Address = address;
        HouseholdBills = householdBills;
    }
}