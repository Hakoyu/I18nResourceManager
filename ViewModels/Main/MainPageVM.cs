using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using HKW.HKWUtils.Collections;
using I18nResourceManager.Models;
using I18nResourceManager.Models.Messages;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;

namespace I18nResourceManager.ViewModels.Main;

internal partial class MainPageVM : ObservableObject
{
    public static Guid MessageToken { get; } = Guid.NewGuid();

    private static readonly ILogger<InfoPageVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<InfoPageVM>();

    private static readonly IDialogService _dialogService =
        Ioc.Default.GetService<IDialogService>()!;

    public MainPageVM()
    {
        CultureNames.ListChanged += CultureNames_ListChanged;

        I18nResources.Add(new("Test"));
        //CultureNames.Add("zh-CN");
        //I18nResources.Add(new("Text1", CultureNames));
        //CurrentI18nResource = I18nResources.First();
        //CurrentI18nResource.Datas.Add(
        //    new("bbb")
        //    {
        //        Datas = new()
        //        {
        //            ["zh-CN"] = new() { Value = "bbbText", Comment = "bbbComment" }
        //        }
        //    }
        //);
        //CurrentI18nResource.Datas.Add(
        //    new("ccc")
        //    {
        //        Datas = new()
        //        {
        //            ["zh-CN"] = new() { Value = "cccText", Comment = "cccComment" }
        //        }
        //    }
        //);
    }

    /// <summary>
    /// 当文化列表改变时 通知前台和资源做出响应
    /// </summary>
    /// <param name="args"></param>
    private void CultureNames_ListChanged(NotifyListChangedEventArgs<string> args)
    {
        if (args.Action is ListChangeAction.Add)
        {
            foreach (var cultureName in args.NewItems!)
            {
                WeakReferenceMessenger.Default.Send<EditCultureMessage, Guid>(
                    new(null, cultureName),
                    MessageToken
                );
            }
        }
        else if (args.Action is ListChangeAction.Remove)
        {
            foreach (var cultureName in args.OldItems!)
            {
                WeakReferenceMessenger.Default.Send<EditCultureMessage, Guid>(
                    new(cultureName, null),
                    MessageToken
                );
            }
        }
        else if (args.Action is ListChangeAction.Replace)
        {
            var newCultureName = args.NewItems!.First();
            var oldCultureName = args.OldItems!.First();
            WeakReferenceMessenger.Default.Send<EditCultureMessage, Guid>(
                new(oldCultureName, newCultureName),
                MessageToken
            );
        }
    }

    #region Property
    [ObservableProperty]
    private I18nResource _currentI18nResource = new();

    [ObservableProperty]
    private ObservableCollection<I18nResource> _i18nResources = new();

    [ObservableProperty]
    private IList<I18nData> _selectedI18nDatas = new ObservableCollection<I18nData>();

    [ObservableProperty]
    private ObservableList<string> _cultureNames = new();
    #endregion

    #region Command

    #region I18nData
    [RelayCommand]
    private void AddI18nData()
    {
        var vm = new EditTextWindowVM();
        vm.Closing += (s, e) =>
        {
            var i18nDataId = s.Text;
            if (s.DialogResult is false)
                return;
            if (string.IsNullOrWhiteSpace(i18nDataId))
            {
                _dialogService.ShowMessageBox(MainWindowVM.Instance, "Id不可为空");
                e.Cancel = true;
                return;
            }

            if (CurrentI18nResource.Datas.Any(i => i.Id == i18nDataId))
            {
                _dialogService.ShowMessageBox(MainWindowVM.Instance, "数据已存在");
                e.Cancel = true;
                return;
            }
            else
            {
                CurrentI18nResource.Datas.Add(new(i18nDataId));
            }
        };
        _dialogService.ShowDialog(MainWindowVM.Instance, vm);
    }

    [RelayCommand]
    private void RemoveI18nData()
    {
        var result = _dialogService.ShowMessageBox(
            MainWindowVM.Instance,
            new()
            {
                Content = $"确定删除选中的 {SelectedI18nDatas.Count} 项吗?",
                Button = MessageBoxButton.YesNo
            }
        );
        if (result is true)
        {
            for (var i = 0; i < SelectedI18nDatas.Count; )
            {
                CurrentI18nResource.Datas.Remove(SelectedI18nDatas[i]);
            }
        }
    }
    #endregion

    #region Culture
    [RelayCommand]
    private void EditCulture(string oldCultureInfo)
    {
        var oldCultureName = GetCultureName(oldCultureInfo);
        var vm = new EditTextWindowVM() { Text = oldCultureName };
        vm.Closing += (s, e) =>
        {
            var newCultureName = s.Text;
            if (s.DialogResult is false)
                return;
            if (string.IsNullOrWhiteSpace(newCultureName))
            {
                _dialogService.ShowMessageBox(MainWindowVM.Instance, "文化名称不可为空");
                e.Cancel = true;
                return;
            }

            if (CultureNames.Contains(newCultureName) is true)
            {
                _dialogService.ShowMessageBox(MainWindowVM.Instance, "文化已存在");
                e.Cancel = false;
                return;
            }
            else
            {
                try
                {
                    var culture = CultureInfo.GetCultureInfo(newCultureName);
                }
                catch
                {
                    _dialogService.ShowMessageBox(
                        MainWindowVM.Instance,
                        new() { Content = "未知的文化" }
                    );
                    e.Cancel = false;
                    return;
                }
                CultureNames.Add(newCultureName);
            }
        };
        _dialogService.ShowDialog(MainWindowVM.Instance, vm);
    }

    [RelayCommand]
    private void RemoveCulture(string cultureInfo)
    {
        var result = _dialogService.ShowMessageBox(
            MainWindowVM.Instance,
            new() { Content = "确定删除吗?", Button = MessageBoxButton.YesNo }
        );
        if (result is true)
        {
            var cultureName = GetCultureName(cultureInfo);
            CultureNames.Remove(cultureName);
        }
    }

    private static readonly Regex _getCultureNameRagex =
        new(@"(?<=\[).*(?=\])", RegexOptions.Compiled);

    private static string GetCultureName(string cultureInfo)
    {
        if (cultureInfo is null)
            return string.Empty;
        return _getCultureNameRagex.Match(cultureInfo).Groups[0].Value;
    }
    #endregion

    #region Resource
    [RelayCommand]
    private void EditResource(I18nResource resource)
    {
        var vm = new EditTextWindowVM() { Text = resource?.Name };
        vm.Closing += (s, e) =>
        {
            var newResourceName = s.Text;
            if (s.DialogResult is false)
                return;
            if (string.IsNullOrWhiteSpace(newResourceName))
            {
                _dialogService.ShowMessageBox(MainWindowVM.Instance, "资源名称不可为空");
                e.Cancel = true;
                return;
            }

            if (I18nResources.Any(i => i.Name == newResourceName))
            {
                _dialogService.ShowMessageBox(MainWindowVM.Instance, "资源名已存在");
                return;
            }
            else
            {
                if (resource is null)
                {
                    I18nResources.Add(new(newResourceName, CultureNames));
                }
                else
                {
                    resource.Name = newResourceName;
                }
            }
        };
        _dialogService.ShowDialog(MainWindowVM.Instance, vm);
    }

    [RelayCommand]
    private void RemoveResource(I18nResource resource)
    {
        var result = _dialogService.ShowMessageBox(
            MainWindowVM.Instance,
            new()
            {
                Title = "删除资源",
                Content = $"确定删除资源 {resource.Name} 吗?",
                Button = MessageBoxButton.YesNo,
                Icon = MessageBoxImage.Information,
            }
        );
        if (result is true)
        {
            I18nResources.Remove(resource);
        }
    }
    #endregion

    #region Save
    [RelayCommand]
    private void Save() { }

    [RelayCommand]
    private void SaveTo()
    {
        var result = _dialogService.ShowOpenFolderDialog(
            MainWindowVM.Instance,
            new() { Title = "保存位置" }
        );
        if (result is null)
            return;
        try
        {
            foreach (var resource in I18nResources)
            {
                resource.SaveTo(result.LocalPath);
            }
            _dialogService.ShowMessageBox(MainWindowVM.Instance, "保存成功");
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessageBox(MainWindowVM.Instance, $"保存失败\n{ex}");
        }
    }
    #endregion

    #region LoadFiles
    [RelayCommand]
    private void LoadFiles()
    {
        var result = _dialogService.ShowOpenFilesDialog(
            MainWindowVM.Instance,
            new()
            {
                Title = "载入文件",
                Filters = new() { new("TOML文件", "toml"), new("所有文件", "*") }
            }
        );
        if (result is null)
            return;
        var window = Ioc.Default.GetService<LoadFilesWindowVM>()!;
        window.LoadFiles(result.Select(i => i.LocalPath));
        _dialogService.ShowDialog(MainWindowVM.Instance, window);
    }
    #endregion
    #endregion
}
