using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using I18nResourceManager.Models;
using HKW.HKWUtils.Collections;
using HKW.HKWTOML;
using System.IO;
using System.Globalization;

namespace I18nResourceManager.ViewModels.Main;

internal partial class LoadFilesWindowVM : ObservableObject
{
    public static Guid MessageToken { get; } = Guid.NewGuid();

    private readonly ILogger<LoadFilesWindowVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<LoadFilesWindowVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    [ObservableProperty]
    public ObservableList<DataFileInfo> _loadFileInfos = new();

    [ObservableProperty]
    public ObservableList<I18nData> _allDatas = new();

    public LoadFilesWindowVM() { }

    public void LoadFiles(IEnumerable<string> files)
    {
        List<string> errorFiles = new();
        foreach (var file in files)
        {
            try
            {
                var fileInfo = GetFileInfo(file);
                LoadFileInfos.Add(fileInfo);
                LoadFile(fileInfo);
            }
            catch
            {
                errorFiles.Add(file);
                // TODO: Add log
            }
        }
        if (errorFiles.Count > 0)
        {
            _dialogService.ShowMessageBox(
                this,
                $"以下文件载入失败\n{string.Join(Environment.NewLine, errorFiles)}"
            );
        }
    }

    public static DataFileInfo GetFileInfo(string file)
    {
        var fileName = Path.GetFileNameWithoutExtension(file);
        return new()
        {
            FileName = fileName,
            FilePath = file,
            Culture = Utils.GetCultureInfo(fileName) is CultureInfo cultureInfo
                ? cultureInfo.Name
                : string.Empty
        };
    }

    private void LoadFile(DataFileInfo fileInfo)
    {
        if (fileInfo.FilePath.EndsWith(TOML.TOMLExtension))
            LoadTomlFile(fileInfo);
    }

    private void LoadTomlFile(DataFileInfo fileInfo)
    {
        var toml = TOML.ParseFromFile(fileInfo.FilePath);
        foreach (var item in toml)
            fileInfo.Datas.Add(new(item.Key) { Datas = new() });
    }

    #region Property

    #endregion

    #region Command

    #endregion
}
