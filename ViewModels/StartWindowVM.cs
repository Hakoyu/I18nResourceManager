using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HKW.HKWUtils.Collections;
using I18nResourceManager.Models;
using System.IO;
using HKW.HKWTOML;
using HKW.HKWTOML.Serializer;

namespace I18nResourceManager.ViewModels;

internal partial class StartWindowVM : ObservableObject
{
    private readonly ILogger<StartWindowVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<StartWindowVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public StartWindowVM() { }

    #region Property
    [ObservableProperty]
    private ObservableList<HistoricData> _historicDatas = new();
    #endregion

    #region Command
    [RelayCommand]
    private void CreateProject() { }

    [RelayCommand]
    private void LoadProject(string path) { }

    [RelayCommand]
    private void LoadProjectFromDirectory()
    {
        //Directory
    }

    [RelayCommand]
    private void Test()
    {
        HistoricDatas.AddRange(HistoricData.GetCustomers(1000));
    }
    #endregion
}
