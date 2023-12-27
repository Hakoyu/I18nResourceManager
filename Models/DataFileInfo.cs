using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using HKW.HKWUtils;
using HKW.HKWUtils.Collections;
using HKW.HKWUtils.Observable;
using HKW.HKWViewModels;
using System.ComponentModel;
using System.IO;

namespace I18nResourceManager.Models;

public partial class DataFileInfo : ObservableObject<DataFileInfo>
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

    [ObservableProperty]
    private string _searchId = string.Empty;

    [ObservableProperty]
    private ObservableList<I18nData> _showDatas;

    [ObservableProperty]
    private ObservableDictionary<string, I18nText> _searchTexts = new();

    /// <summary>
    /// 能被选中
    /// </summary>
    public bool CanSelect => string.IsNullOrWhiteSpace(Culture) is false;

    /// <summary>
    /// 上一个可用文化
    /// </summary>
    private string _lastCulture = string.Empty;

    public DataFileInfo()
    {
        ShowDatas = Datas;
        Datas.ListChanged += Datas_ListChanged;
        SearchTexts.DictionaryChanged += SearchTexts_DictionaryChanged;
    }

    private void SearchTexts_DictionaryChanged(
        IObservableDictionary<string, I18nText> sender,
        NotifyDictionaryChangedEventArgs<string, I18nText> e
    )
    {
        if (e.Action is DictionaryChangeAction.Add)
        {
            foreach (var item in e.NewItems!)
                item.Value.PropertyChanged += Text_PropertyChanged;
        }
        else if (e.Action is DictionaryChangeAction.Remove)
        {
            foreach (var item in e.OldItems!)
                item.Value.PropertyChanged -= Text_PropertyChanged;
        }
    }

    private void Text_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        RefreshShowDatas();
    }

    private void Datas_ListChanged(
        IObservableList<I18nData> sender,
        NotifyListChangedEventArgs<I18nData> e
    )
    {
        RefreshShowDatas();
    }

    partial void OnDatasChanged(ObservableList<I18nData> value)
    {
        RefreshShowDatas();
    }

    partial void OnSearchIdChanged(string value)
    {
        RefreshShowDatas();
    }

    private void RefreshShowDatas()
    {
        var temp = string.IsNullOrWhiteSpace(SearchId)
            ? Datas
            : Datas.Where(d => d.Id.Contains(SearchId, StringComparison.OrdinalIgnoreCase));
        foreach (var text in SearchTexts)
        {
            if (string.IsNullOrWhiteSpace(text.Value.Value) is false)
                temp = temp.Where(
                    d =>
                        d.Texts
                            .First()
                            .Value.Value.Contains(
                                text.Value.Value,
                                StringComparison.OrdinalIgnoreCase
                            )
                );
            if (string.IsNullOrWhiteSpace(text.Value.Comment) is false)
                temp = temp.Where(
                    d =>
                        d.Texts
                            .First()
                            .Value.Comment.Contains(
                                text.Value.Comment,
                                StringComparison.OrdinalIgnoreCase
                            )
                );
        }
        if (temp is not ObservableList<I18nData> result)
            result = new ObservableList<I18nData>(temp);
        ShowDatas = result;
    }

    partial void OnCultureChanged(string? oldValue, string newValue)
    {
        if (
            string.IsNullOrWhiteSpace(newValue)
            || CultureUtils.Exists(newValue) is false
            || _lastCulture == newValue
        )
            return;
        _lastCulture = newValue;
        foreach (var data in Datas)
        {
            data.Texts[newValue] = data.Texts[oldValue!];
            data.Texts.Remove(oldValue!);
        }
        SearchTexts.Remove(oldValue!);
        SearchTexts[newValue] = new(newValue);
    }

    public static IEnumerable<DataFileInfo> GetFakeDatas(string cultureName, int count = 10)
    {
        var generator = new Faker<DataFileInfo>()
            .RuleFor(v => v.Culture, cultureName)
            .RuleFor(v => v.FilePath, f => f.System.FilePath())
            .RuleFor(v => v.FileName, (f, v) => Path.GetFileNameWithoutExtension(v.FilePath))
            .RuleFor(v => v.Datas, (f, v) => new(I18nData.GetFakeDatas(count, cultureName)));
        return generator.Generate(count);
    }
}
