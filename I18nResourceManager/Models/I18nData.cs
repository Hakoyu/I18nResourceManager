using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using HKW.HKWUtils.Collections;
using HKW.HKWUtils.Observable;
using I18nResourceManager.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace I18nResourceManager.Models;

/// <summary>
/// I18n数据
/// </summary>
public partial class I18nData : ObservableObject
{
    /// <summary>
    /// ID
    /// </summary>
    [ObservableProperty]
    private string _id;

    /// <summary>
    /// 文本
    /// <para>
    /// (CultureName, I18nText)
    /// </para>
    /// </summary>
    [ObservableProperty]
    ObservableDictionary<string, I18nText> _texts = new();

    /// <inheritdoc/>
    /// <param name="id">ID</param>
    public I18nData(string id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    /// <param name="id">ID</param>
    /// <param name="cultureNames">文化名称</param>
    public I18nData(string id, IEnumerable<string> cultureNames)
        : this(id)
    {
        foreach (string cultureName in cultureNames)
            Texts.Add(cultureName, new(cultureName));
    }

    public static IEnumerable<I18nData> GetFakeDatas(int count = 10, params string[] cultureNames)
    {
        var faker = new Faker();
        for (int i = 0; i < count; i++)
        {
            var data = new I18nData(faker.Name.FirstName());
            data.Texts = new(
                cultureNames
                    .Zip(cultureNames.Select(c => I18nText.GetFakeDatas(data.Id, c).First()))
                    .ToDictionary(v => v.First, v => v.Second)
            );
            yield return data;
        }
    }
}
