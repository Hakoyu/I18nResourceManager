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

    public ObservableList<LoadFileInfo> LoadFileInfos { get; set; } = new();

    public ObservableList<I18nData> AllDatas { get; set; }

    public LoadFilesWindowVM() { }

    public IEnumerable<I18nData>? LoadFiles(IEnumerable<string> files)
    {
        try
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var fileInfo = new LoadFileInfo()
                {
                    FileName = fileName,
                    FilePath = file,
                    Culture = Utils.GetCultureInfo(fileName) is CultureInfo cultureInfo
                        ? cultureInfo.Name
                        : string.Empty
                };
                LoadFileInfos.Add(fileInfo);
                LoadFile(fileInfo);
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessageBox(this, $"载入文件失败\n{ex}");
            return null;
        }
        return null;
    }

    private void LoadFile(LoadFileInfo fileInfo) { }

    private void LoadTomlFile(LoadFileInfo fileInfo)
    {
        var toml = TOML.ParseFromFile(fileInfo.FilePath);
        var res = new I18nResource(fileInfo.FileName, fileInfo.Culture);
        foreach (var item in toml)
        {
            res.Datas.Add(new(item.Key) { Datas = new() });
        }
    }

    #region Property

    #endregion

    #region Command

    #endregion
}
