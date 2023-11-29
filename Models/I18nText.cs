using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;

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

    public I18nText() { }

    public I18nText(string value, string comment)
    {
        Value = value;
        Comment = comment;
    }
}
