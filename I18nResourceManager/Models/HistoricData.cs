using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models;

/// <summary>
/// 历史数据
/// </summary>
public partial class HistoricData : ObservableObject
{
    /// <summary>
    /// 名称
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// 项目路径
    /// </summary>
    [ObservableProperty]
    private string? _projectPath;

    /// <summary>
    /// 最后编辑时间
    /// </summary>
    [ObservableProperty]
    private DateTime? _lastEditTime;

    public static IEnumerable<HistoricData> GetCustomers(int count, int seed = 114514)
    {
        var faker = new Faker<HistoricData>()
            .UseSeed(seed)
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.ProjectPath, f => f.System.DirectoryPath())
            .RuleFor(p => p.LastEditTime, f => f.Date.Past());
        return faker.Generate(count);
    }
}
