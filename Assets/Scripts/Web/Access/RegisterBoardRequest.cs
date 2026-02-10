using System;

public class RegisterBoardRequest
{
    public String FirstName1 { get; set; }
    public String FirstName2 { get; set; }
    public String LastName1 { get; set; }
    public String LastName2 { get; set; }
    public String Roles { get; set; } = "BD";
    public String Email { get; set; }
    public String Password { get; set; }
    public long PhoneCountryId { get; set; }
    public String Phone { get; set; }


    public RegisterBoardRequest()
    {
    }

    public RegisterBoardRequest(String firstName1, String firstName2, String lastName1, String lastName2,
                                String roles, String email, String password, long phoneCountryId, String phone,
                                int news, String referredCode)
    {
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        LastName1 = lastName1;
        LastName2 = lastName2;
        Roles = roles;
        Email = email;
        Password = password;
        PhoneCountryId = phoneCountryId;
        Phone = phone;
    }

    public RegisterBoardRequest(String firstName1, String firstName2, String lastName1, String lastName2,
                                String roles, String email, String password, long phoneCountryId, String phone)
    {
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        LastName1 = lastName1;
        LastName2 = lastName2;
        Roles = roles;
        Email = email;
        Password = password;
        PhoneCountryId = phoneCountryId;
        Phone = phone;
    }

    public RegisterBoardRequest(String firstName1, String firstName2, String lastName1, String lastName2,
                                String roles, String email, long phoneCountryId, String phone)
    {
        FirstName1 = firstName1;
        FirstName2 = firstName2;
        LastName1 = lastName1;
        LastName2 = lastName2;
        Roles = roles;
        Email = email;
        Password = null;
        PhoneCountryId = phoneCountryId;
        Phone = phone;
    }
}
