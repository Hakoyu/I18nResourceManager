using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HKW.HKWUtils.Collections;
using I18nResourceManager.Models.ProgrammingLanguage;
using I18nResourceManager.Models.DataFile;
using HKW.HKWUtils.Observable;

namespace I18nResourceManager.Models;

internal partial class Settings : ObservableObject
{
    public static Settings Instance { get; set; } = new();

    [ObservableProperty]
    private string _programmingLanguagePath = "";

    [ObservableProperty]
    private ProgrammingLanguageTypes _currentLanguage = ProgrammingLanguageTypes.CSharp;

    [ObservableProperty]
    private ObservableList<ProgrammingLanguageTypes> _languages =
        new(Enum.GetValues<ProgrammingLanguageTypes>());

    [ObservableProperty]
    private DataFileTypes _currentDataFileFormat;

    [ObservableProperty]
    private ObservableList<DataFileTypes> _dataFileFormats = new(Enum.GetValues<DataFileTypes>());

    public Settings() { }
}
