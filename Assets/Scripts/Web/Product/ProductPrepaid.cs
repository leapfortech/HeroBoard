using System;
using System.Collections.Generic;

public class ProductPrepaid
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int CpiMin { get; set; }
    public int CpiMax { get; set; }
    public int CpiDefault { get; set; }
    public double ReserveRate { get; set; }
    public int ProductPrepaidStatusId { get; set; }


    public ProductPrepaid()
    {
    }

    public ProductPrepaid(int id, int projectId, int cpiMin, int cpiMax, int cpiDefault, double reserveRate, int productPrepaidStatusId)
    {
        Id = id;
        ProjectId = projectId;
        CpiMin = cpiMin;
        CpiMax = cpiMax;
        CpiDefault = cpiDefault;
        ReserveRate = reserveRate;
        ProductPrepaidStatusId = productPrepaidStatusId;
    }
}