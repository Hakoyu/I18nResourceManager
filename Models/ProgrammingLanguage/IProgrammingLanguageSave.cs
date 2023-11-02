namespace I18nResourceManager.Models.ProgrammingLanguage;

public interface IProgrammingLanguageSave
{
    public void SaveToFile(string filePath, I18nResource resource);
}
