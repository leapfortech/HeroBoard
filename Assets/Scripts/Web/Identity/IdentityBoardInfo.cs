
public class IdentityBoardInfo
{
    public Identity Identity { get; set; }
    public DpiBoardPhoto DpiBoardPhoto { get; set; }

    public IdentityBoardInfo()
    {
    }

    public IdentityBoardInfo(Identity identity, DpiBoardPhoto dDpiBoardPhoto)
    {
        Identity = identity;
        DpiBoardPhoto = dDpiBoardPhoto;
    }
}
