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
using HKW.HKWUtils.Observable;
using HKW.HKWUtils;

namespace I18nResourceManager.ViewModels.Main;

internal partial class LoadFilesWindowVM : DialogWindowVM<LoadFilesWindowVM>
{
    public static Guid MessageToken { get; } = Guid.NewGuid();

    private readonly ILogger<LoadFilesWindowVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<LoadFilesWindowVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public LoadFilesWindowVM()
    {
        CheckGroup.CheckAllFalse += CheckGroup_CheckAllFalse;
#if DEBUG
        foreach (var info in DataFileInfo.GetFakeDatas(""))
        {
            info.ValueChanged += FileInfo_ValueChanged;
            LoadFileInfos.Add(info);
        }
#endif
    }

    private void CheckGroup_CheckAllFalse(CheckGroup sender, EventArgs e)
    {
        _dialogService.ShowMessageBoxAsync(this, "全选失败\n有无法选中的项目");
    }

    #region Property
    [ObservableProperty]
    private CheckGroup _checkGroup = new();

    [ObservableProperty]
    private ObservableList<DataFileInfo> _loadFileInfos = new();

    [ObservableProperty]
    private DataFileInfo? _cultureLoadFileInfo = null;

    [ObservableProperty]
    private ObservableList<I18nData> _datas = new();

    partial void OnCultureLoadFileInfoChanged(DataFileInfo? oldValue, DataFileInfo? newValue)
    {
        //FileInfo_ValueChanged(
        //    newValue,
        //    new(nameof(DataFileInfo.Culture), oldValue!.Culture, newValue.Culture)
        //);
        WeakReferenceMessenger.Default.Send<EditCultureMessage, Guid>(
            new(oldValue is null ? null : new(oldValue?.Culture!), new(newValue?.Culture!)),
            MessageToken
        );
    }

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
            _dialogService.ShowMessageBoxAsync(
                this,
                $"以下文件载入失败\n{string.Join(Environment.NewLine, errorFiles)}"
            );
        }
    }

    private void FileInfo_ValueChanged(DataFileInfo sender, PropertyValueChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DataFileInfo.Culture))
        {
            var (oldValue, newValue) = e.GetValue<string>();
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _dialogService.ShowMessageBoxAsync(this, "文化不能为空");
                e.Cancel = true;
                return;
            }
            if (CultureUtils.TryGetCultureInfo(newValue, out var cultureInfo) is false)
            {
                _dialogService.ShowMessageBoxAsync(this, $"无法识别文化 {newValue}");
                e.Cancel = true;
                return;
            }
            WeakReferenceMessenger.Default.Send<EditCultureMessage, Guid>(
                new(new(oldValue), new(cultureInfo)),
                MessageToken
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
        LoadFileInfos.Add(fileInfo);
        fileInfo.ValueChanged += FileInfo_ValueChanged;
        if (fileInfo.FilePath.EndsWith(TOML.ExtensionName))
            LoadTomlFile(fileInfo);
    }

    private void LoadTomlFile(DataFileInfo fileInfo)
    {
        var toml = TOML.ParseFromFile(fileInfo.FilePath);
        foreach (var item in toml)
            fileInfo.Datas.Add(new(item.Key));
    }
}
