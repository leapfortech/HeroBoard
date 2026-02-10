using System;

public class PhoneCodeRequest
{
    public long PhoneCountryId { get; set; }
    public String PhoneNumber { get; set; }
    public int Code { get; set; }

    public PhoneCodeRequest()
    {

    }

    public PhoneCodeRequest(long phoneCountryId, String phoneNumber, int code)
    {
        PhoneCountryId = phoneCountryId;
        PhoneNumber = phoneNumber;
        Code = code;
    }
}
