using System;

public class ProjectInformation
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int ProjectInformationTypeId { get; set; }
    public String Information { get; set; }
    public int Status { get; set; }


    public ProjectInformation()
    {
    }

    public ProjectInformation(int id, int projectId, int projectInformationTypeId, String information, int status)
    {
        Id = id;
        ProjectId = projectId;
        ProjectInformationTypeId = projectInformationTypeId;
        Information = information;
        Status = status;
    }
}