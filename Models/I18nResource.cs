using CommunityToolkit.Mvvm.ComponentModel;
using HKW.HKWUtils.Collections;
using I18nResourceManager.Models.DataFile;
using I18nResourceManager.Models.ProgrammingLanguage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models;

public partial class I18nResource : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private ObservableList<I18nData> _datas = new();

    public ObservableList<string> CultureNames { get; }

    public I18nResource(string name, ObservableList<string> cultureNames)
    {
        Name = name;
        CultureNames = cultureNames;
        foreach (var cultureName in CultureNames)
        {
            foreach (var data in Datas)
                data.Datas.Add(cultureName, new());
        }
        CultureNames.ListChanged += CultureNames_ListChanged;
        Datas.ListChanged += Datas_ListChanged;
    }

    private void Datas_ListChanged(NotifyListChangedEventArgs<I18nData> args)
    {
        if (args.Action is ListChangeAction.Add)
        {
            var newData = args.NewItems![0];
            foreach (var cultureName in CultureNames)
                newData.Datas.TryAdd(cultureName, new());
        }
    }

    /// <summary>
    /// 当文化列表改变时 通知前台和资源做出响应
    /// </summary>
    /// <param name="args"></param>
    private void CultureNames_ListChanged(NotifyListChangedEventArgs<string> args)
    {
        if (args.Action is ListChangeAction.Add)
        {
            foreach (var cultureName in args.NewItems!)
            {
                foreach (var data in Datas)
                    data.Datas.Add(cultureName, new());
            }
        }
        else if (args.Action is ListChangeAction.Remove)
        {
            foreach (var cultureName in args.OldItems!)
            {
                foreach (var data in Datas)
                    data.Datas.Remove(cultureName);
            }
        }
        else if (args.Action is ListChangeAction.Replace)
        {
            var oldCulture = args.OldItems![0];
            var newCulture = args.NewItems![0];
            foreach (var data in Datas)
            {
                var tempData = data.Datas[oldCulture];
                data.Datas.Remove(oldCulture);
                data.Datas.Add(newCulture, tempData);
            }
        }
    }

    public void SaveTo(string path)
    {
        Settings.Instance.CurrentLanguage.SaveToFile(path, this);
        Settings.Instance.CurrentDataFileFormat.SaveToFile(path, this);
    }
}
