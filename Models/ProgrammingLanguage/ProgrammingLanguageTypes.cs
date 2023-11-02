using I18nResourceManager.Models;

namespace I18nResourceManager.Models.ProgrammingLanguage;

public enum ProgrammingLanguageTypes
{
    None,
    CSharp
}

public static class ProgrammingLanguageExpansions
{
    private static readonly Dictionary<
        ProgrammingLanguageTypes,
        IProgrammingLanguageSave
    > _dataFileFormatSave = new() { [ProgrammingLanguageTypes.CSharp] = new SaveAsCSharp() };

    public static void SaveToFile(
        this ProgrammingLanguageTypes language,
        string filePath,
        I18nResource resource
    )
    {
        if (language is ProgrammingLanguageTypes.None)
            return;
        _dataFileFormatSave[language].SaveToFile(filePath, resource);
    }
}
