namespace I18nResourceManager.Models.DataFile;

public enum DataFileTypes
{
    TOML,
}

public static class DataFileFormatExpansions
{
    private static readonly Dictionary<DataFileTypes, IDataFileSave> _dataFileSaves =
        new() { [DataFileTypes.TOML] = new SaveAsTOML() };

    public static void SaveToFile(this DataFileTypes format, string filePath, I18nResource resource)
    {
        _dataFileSaves[format].SaveToFile(filePath, resource);
    }
}
