using CommunityToolkit.Mvvm.ComponentModel;

namespace I18nResourceManager;

/// <summary>
/// 页面信息
/// </summary>
public partial class PageInfo : ObservableObject
{
    /// <summary>
    /// ID
    /// </summary>
    [ObservableProperty]
    private string? _id;

    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// 简介
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    private object? _content;

    partial void OnContentChanged(object? value)
    {
        if (Content is null)
            Id = string.Empty;
        else
            Id = Content.GetType().FullName;
    }
}
