namespace DAL.Common.DocClass
{
    public interface ISupportLocationNameChangesNested
    {
        bool ChangeOfficeName(string locationId, string newName, bool modified);
    }
}