using HKW.HKWUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models;

/// <summary>
/// 简易文化信息
/// </summary>
public struct SimpleCultureInfo
{
    /// <summary>
    /// 未知文化标识符
    /// </summary>
    public const string UnknownCulture = nameof(UnknownCulture);

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 全名
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// 是未知文化
    /// </summary>
    public readonly bool IsUnknownCulture => Name == UnknownCulture;

    public SimpleCultureInfo()
    {
        FullName = Name = UnknownCulture;
    }

    public SimpleCultureInfo(string cultureName)
    {
        if (CultureUtils.TryGetCultureInfo(cultureName, out var cultureInfo) is false)
        {
            FullName = Name = UnknownCulture;
            return;
        }
        Name = cultureInfo.Name;
        FullName = cultureInfo.GetFullName();
    }

    public SimpleCultureInfo(CultureInfo cultureInfo)
    {
        Name = cultureInfo.Name;
        FullName = cultureInfo.GetFullName();
    }

    public override readonly string ToString()
    {
        return FullName;
    }
}
