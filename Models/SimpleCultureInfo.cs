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
    public const string Unknown = nameof(Unknown);

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 全名
    /// </summary>
    public string FullName { get; set; }

    public SimpleCultureInfo()
    {
        FullName = Name = Unknown;
    }

    public SimpleCultureInfo(string cultureName)
    {
        var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
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
