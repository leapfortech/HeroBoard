using System;

public class Alert
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int TypeId { get; set; }
    public int AlertFrequencyId { get; set; }
    public String Alias { get; set; }
    public float Amount { get; set; }
    public String DueName { get; set; }
    public DateTime DateTime { get; set; }
    public int WeekDayId { get; set; }
    public int Status { get; set; }


    public Alert()
    {
    }

    public Alert(int id, int appUserId, int typeId, int alertFrequencyId, String alias, float amount, String dueName, DateTime dateTime, int weekDayId, int status)
    {
        Id = id;
        AppUserId = appUserId;
        TypeId = typeId;
        AlertFrequencyId = alertFrequencyId;
        Alias = alias;
        Amount = amount;
        DueName = dueName;
        DateTime = dateTime;
        WeekDayId = weekDayId;
        Status = status;
    }
}
