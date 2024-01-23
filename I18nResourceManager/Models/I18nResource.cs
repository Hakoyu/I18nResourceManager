using CommunityToolkit.Mvvm.ComponentModel;
using HKW.HKWUtils.Collections;
using HKW.HKWUtils.Observable;
using I18nResourceManager.Models.DataFile;
using I18nResourceManager.Models.ProgrammingLanguage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models;

/// <summary>
/// I18n资源
/// </summary>
public partial class I18nResource : ObservableObject
{
    /// <summary>
    /// 资源名称
    /// </summary>
    [ObservableProperty]
    private string _name = string.Empty;

    /// <summary>
    /// 数据
    /// </summary>
    [ObservableProperty]
    private ObservableList<I18nData> _datas = new();

    /// <summary>
    /// 显示的数据
    /// </summary>
    [ObservableProperty]
    private ObservableList<I18nData> _showDatas = new();

    /// <summary>
    /// 搜索的Id
    /// </summary>
    [ObservableProperty]
    private string _searchId = string.Empty;

    public ObservableList<string> CultureNames { get; }

    public I18nResource(string name, ObservableList<string> cultureNames)
    {
        Name = name;
        CultureNames = cultureNames;
        foreach (var cultureName in CultureNames)
        {
            foreach (var data in Datas)
                data.Texts.Add(cultureName, new(cultureName));
        }
        CultureNames.ListChanged += CultureNames_ListChanged;
        Datas.ListChanged += Datas_ListChanged;
    }

    private void Datas_ListChanged(
        IObservableList<I18nData> sender,
        NotifyListChangedEventArgs<I18nData> args
    )
    {
        if (args.Action is ListChangeAction.Add)
        {
            var newData = args.NewItems![0];
            foreach (var cultureName in CultureNames)
                newData.Texts.TryAdd(cultureName, new(cultureName));
        }
    }

    /// <summary>
    /// 当文化列表改变时 通知前台和资源做出响应
    /// </summary>
    /// <param name="args"></param>
    private void CultureNames_ListChanged(
        IObservableList<string> sender,
        NotifyListChangedEventArgs<string> args
    )
    {
        if (args.Action is ListChangeAction.Add)
        {
            foreach (var cultureName in args.NewItems!)
            {
                foreach (var data in Datas)
                    data.Texts.Add(cultureName, new(cultureName));
            }
        }
        else if (args.Action is ListChangeAction.Remove)
        {
            foreach (var cultureName in args.OldItems!)
            {
                foreach (var data in Datas)
                    data.Texts.Remove(cultureName);
            }
        }
        else if (args.Action is ListChangeAction.Replace)
        {
            var oldCulture = args.OldItems![0];
            var newCulture = args.NewItems![0];
            foreach (var data in Datas)
            {
                data.Texts[newCulture] = data.Texts[oldCulture];
                data.Texts.Remove(oldCulture);
            }
        }
    }

    public void SaveTo(string path)
    {
        Settings.Instance.CurrentLanguage.SaveToFile(path, this);
        Settings.Instance.CurrentDataFileFormat.SaveToFile(path, this);
    }
}
