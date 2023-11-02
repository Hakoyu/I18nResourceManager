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

    public static IEnumerable<I18nText> GetCustomers(int count, int seed = 114514)
    {
        var faker = new Faker<I18nText>()
            .UseSeed(seed)
            .RuleFor(p => p.Value, f => f.Name.FirstName())
            .RuleFor(p => p.Comment, f => f.Name.FullName());
        return faker.Generate(count);
        //Randomizer.Seed = new Random(seed);
        //var faker = new Faker<TestData>()
        //    .RuleFor(p => p.Id, Guid.NewGuid().ToString())
        //    .RuleFor(p => p.Name, f => f.Name.FirstName());
        //return faker.Generate(count);
    }
}
