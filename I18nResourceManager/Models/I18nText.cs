using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using HKW.HKWViewModels;

namespace I18nResourceManager.Models;

/// <summary>
/// I18n文本
/// </summary>
public partial class I18nText : ObservableObject
{
    /// <summary>
    /// 值
    /// </summary>
    [ObservableProperty]
    private string _value = string.Empty;

    /// <summary>
    /// 注释
    /// </summary>
    [ObservableProperty]
    private string _comment = string.Empty;

    /// <summary>
    /// 文化名称
    /// </summary>
    [ObservableProperty]
    private string _cultureName = string.Empty;

    public I18nText(string cultureName)
    {
        CultureName = cultureName;
    }

    public I18nText(string cultureName, string value, string comment)
        : this(cultureName)
    {
        Value = value;
        Comment = comment;
    }

    public static IEnumerable<I18nText> GetFakeDatas(string id, string cultureName, int count = 10)
    {
        for (var i = 0; i < count; i++)
        {
            yield return new(
                cultureName,
                $"{id}_{cultureName}",
                $"{id}_{cultureName}_{nameof(Comment)}"
            );
        }
    }
}
