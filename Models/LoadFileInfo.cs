using CommunityToolkit.Mvvm.ComponentModel;
using HKW.HKWUtils.Collections;

namespace I18nResourceManager.Models;

public partial class DataFileInfo : ObservableObject
{
    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private string _culture = string.Empty;

    [ObservableProperty]
    private bool _isSelected = false;

    [ObservableProperty]
    private ObservableList<I18nData> _datas = null!;
}
