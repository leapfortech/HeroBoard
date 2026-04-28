using System.Collections.Generic;

public class ReferredFullAllRsp
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public List<ReferredFull> ReferredFulls { get; set; }

    public ReferredFullAllRsp()
    {
    }

    public ReferredFullAllRsp(int page, int totalPages, List<ReferredFull> referredFulls)
    {
        Page = page;
        TotalPages = totalPages;
        ReferredFulls = referredFulls;
    }
}