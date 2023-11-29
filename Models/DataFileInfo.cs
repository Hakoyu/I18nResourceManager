using CommunityToolkit.Mvvm.ComponentModel;
using HKW.HKWUtils.Collections;
using HKW.HKWViewModels;

namespace I18nResourceManager.Models;

public partial class DataFileInfo : ViewModelBase<DataFileInfo>
{
    [ObservableProperty]
    private string _fileName = string.Empty;

    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSelect))]
    private string _culture = string.Empty;

    [ObservableProperty]
    private bool _isSelected = false;

    [ObservableProperty]
    private ObservableList<I18nData> _datas = new();

    public bool CanSelect => string.IsNullOrEmpty(Culture) is false;
}
