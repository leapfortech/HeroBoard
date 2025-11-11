using System;

public class PhoneCodeRequest
{
    public int PhoneCountryId { get; set; }
    public String PhoneNumber { get; set; }
    public int Code { get; set; }

    public PhoneCodeRequest()
    {

    }

    public PhoneCodeRequest(int phoneCountryId, String phoneNumber, int code)
    {
        PhoneCountryId = phoneCountryId;
        PhoneNumber = phoneNumber;
        Code = code;
    }
}
