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
using CommunityToolkit.Mvvm.Messaging;
using I18nResourceManager.Models.Messages;
using HKW.HKWViewModels;

namespace I18nResourceManager.ViewModels.Main;

internal partial class LoadFilesWindowVM : DialogWindowVM<LoadFilesWindowVM>
{
    public static Guid MessageToken { get; } = Guid.NewGuid();

    private readonly ILogger<LoadFilesWindowVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<LoadFilesWindowVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public LoadFilesWindowVM() { }

    #region Property

    [ObservableProperty]
    private ObservableList<DataFileInfo> _loadFileInfos = new();

    [ObservableProperty]
    private ObservableList<I18nData> _allDatas = new();
    #endregion

    #region Command
    [RelayCommand]
    private void Cancel()
    {
        _dialogService.Close(this);
    }
    #endregion

    public void LoadFiles(IEnumerable<string> files)
    {
        List<string> errorFiles = new();
        foreach (var file in files)
        {
            try
            {
                var fileInfo = GetFileInfo(file);
                LoadFileInfos.Add(fileInfo);
                fileInfo.ValueChanged += FileInfo_ValueChanged;
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

    private void FileInfo_ValueChanged(DataFileInfo sender, ValueChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DataFileInfo.Culture))
        {
            var (oldValue, newValue) = e.GetValue<string>();
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _dialogService.ShowMessageBox(this, "文化不能为空");
                if (string.IsNullOrWhiteSpace(oldValue) is false)
                    sender.Culture = oldValue;
                return;
            }
            if (Utils.GetCultureInfo(newValue) is null)
            {
                _dialogService.ShowMessageBox(this, $"无法识别文化 {newValue}");
                return;
            }
            WeakReferenceMessenger.Default.Send<EditCultureMessage, Guid>(
                new(oldValue, newValue),
                MessageToken
            );
        }
        else if (e.PropertyName == nameof(DataFileInfo.IsSelected))
        {
            var (oldValue, newValue) = e.GetValue<bool>();
            if (string.IsNullOrWhiteSpace(sender.Culture) is false)
                return;
            if (newValue is false)
                return;
            _dialogService.ShowMessageBox(this, $"文化为空, 无法选中");
            sender.IsSelected = false;
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
}
