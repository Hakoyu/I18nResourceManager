using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using HKW.HKWUtils.Collections;
using I18nResourceManager.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace I18nResourceManager.Models;

/// <summary>
/// I18n数据
/// </summary>
public partial class I18nData : ObservableObject
{
    private static readonly ILogger<I18nData> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<I18nData>();

    private static readonly IDialogService _dialogService =
        Ioc.Default.GetService<IDialogService>()!;

    [ObservableProperty]
    private string _id = string.Empty;

    partial void OnIdChanged(string? oldValue, string newValue)
    {
        if (string.IsNullOrWhiteSpace(newValue) is false)
            return;
        _dialogService.ShowMessageBox(
            MainWindowVM.Instance,
            new() { Content = "Id不可为空", Icon = MessageBoxImage.Warning }
        );
        if (string.IsNullOrWhiteSpace(oldValue) is false)
            Id = oldValue;
    }

    /// <summary>
    /// 值
    /// <para>
    /// (CultureName, I18nText)
    /// </para>
    /// </summary>
    [ObservableProperty]
    ObservableDictionary<string, I18nText> _datas = new();

    public I18nData(string id)
    {
        Id = id;
    }

    public I18nData(string id, IEnumerable<string> cultureNames)
    {
        Id = id;
        foreach (string cultureName in cultureNames)
        {
            Datas.Add(cultureName, new());
        }
    }
}
