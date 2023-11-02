namespace I18nResourceManager.Models.DataFile;

public interface IDataFileSave
{
    public void SaveToFile(string filePath, I18nResource resource);
}
