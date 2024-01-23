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

internal partial class StartVM : ObservableObject
{
    private readonly ILogger<StartVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<StartVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public StartVM() { }

    #region Property
    [ObservableProperty]
    private ObservableList<HistoricData> _historicDatas = new();
    #endregion

    #region Command
    [RelayCommand]
    private void CreateProject()
    {
        //var window = Ioc.Default.GetService<MainWindowVM>()!;
        //window.Closed += (v) =>
        //{
        //    _dialogService.Show(null, this);
        //};
        //_dialogService.Show(null, window);
    }

    [RelayCommand]
    private async Task LoadProject(HistoricData data)
    {
        if (File.Exists(data.ProjectPath) is false)
        {
            if (
                await _dialogService.ShowMessageBoxAsync(
                    this,
                    $"项目 \"{data.Name}\" 不存在, 是否从最近使用列表中移除它",
                    "",
                    HanumanInstitute.MvvmDialogs.FrameworkDialogs.MessageBoxButton.YesNo
                )
                is true
            )
                HistoricDatas.Remove(data);
            return;
        }
        LoadProject(data.ProjectPath);
    }

    [RelayCommand]
    private async Task LoadProjectFromDirectory()
    {
        var result = await _dialogService.ShowOpenFileDialogAsync(this, Utils.I18nResInfoSettings);
        if (result is null)
            return;
        var path = result.LocalPath;
        if (path.EndsWith(Utils.I18nResInfoFile) is false)
        {
            await _dialogService.ShowMessageBoxAsync(this, $"文件必须为 {Utils.I18nResInfoFile}");
            return;
        }
        LoadProject(result.LocalPath);
    }

    [RelayCommand]
    private void Test()
    {
        foreach (var item in HistoricData.GetCustomers(1000))
            HistoricDatas.Add(item);
    }
    #endregion

    private void LoadProject(string path)
    {
        var info = TOMLDeserializer.Deserialize<I18nResInfo>(TOML.ParseFromFile(path));
    }
}
