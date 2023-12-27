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
using I18nResourceManager.ViewModels.Main;
using HKW.HKWTOML.Deserializer;
using HKW.HKWUtils.Observable;

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
    private ObservableListX<HistoricData> _historicDatas = new();
    #endregion

    #region Command
    [RelayCommand]
    private void CreateProject()
    {
        var window = Ioc.Default.GetService<MainWindowVM>()!;
        //window.Closed += (v) =>
        //{
        //    _dialogService.Show(null, this);
        //};
        _dialogService.Show(null, window);
    }

    [RelayCommand]
    private void LoadProject(string path)
    {
        var info = TOMLDeserializer.Deserialize<I18nResInfo>(TOML.ParseFromFile(path));
    }

    [RelayCommand]
    private void LoadProjectFromDirectory()
    {
        var result = _dialogService.ShowOpenFileDialog(this, Utils.I18nResInfoSettings);
        if (result is null)
            return;
        var path = result.LocalPath;
        if (path.EndsWith(Utils.I18nResInfoFile) is false)
        {
            _dialogService.ShowMessageBox(this, $"文件必须为 {Utils.I18nResInfoFile}");
            return;
        }
        LoadProject(result.LocalPath);
    }

    [RelayCommand]
    private void Test()
    {
        HistoricDatas.AddRange(HistoricData.GetCustomers(1000));
    }
    #endregion
}
