using CommunityToolkit.Mvvm.ComponentModel;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models.ProgrammingLanguage;

public partial class SaveAsCSharp : ObservableObject, IProgrammingLanguageSave
{
    //    [ObservableProperty]
    //    private string _usingNameSpace = "HKW.HKWViewModels";

    //    [ObservableProperty]
    //    private string _usingNameSpaceFormat = "using {UsingNameSpace};";

    //    [ObservableProperty]
    //    private string _nameSpace = "I18nResource";

    //    [ObservableProperty]
    //    private string _nameSpaceFormat = "namespace {NameSpace};\n{ClassTitle}";

    //    [ObservableProperty]
    //    private string _classTitle = string.Empty;

    //    [ObservableProperty]
    //    private string _className = "{ResourceName}";

    //    [ObservableProperty]
    //    private string _classData = string.Empty;

    //    [ObservableProperty]
    //    private string _classTitleFormat =
    //        @"public class {ClassName}\{
    //{ClassData}
    //\}";

    //    [ObservableProperty]
    //    private string _propertyFormat = "public {PropertyName} {PropertyValue}";

    public string ResourceName { get; private set; } = string.Empty;

    public void SaveToFile(string filePath, I18nResource resource)
    {
        ResourceName = resource.Name;
        var sb = new StringBuilder();
        sb.AppendLine("using HKW.HKWViewModels;");
        sb.AppendLine();
        sb.AppendLine("namespace I18nResource;");
        sb.AppendLine();
        sb.AppendLine($"public class {ResourceName}");
        sb.AppendLine("{");
        sb.AppendLine("    private static I18nRes I18nRes { get; } = new(MainWindowVM.I18nCore);");
        foreach (var data in resource.Datas)
        {
            sb.AppendLine(
                $"    public static string {data.Id} => I18nRes.GetCultureData(nameof({data.Id}));"
            );
        }
        sb.AppendLine("}");
        File.WriteAllText(Path.Combine(filePath, $"{resource.Name}.cs"), sb.ToString());
    }

    //public void SaveToFile(string filePath, I18nResource resource)
    //{
    //    ResourceName = resource.Name;
    //    ClassName = Smart.Format(ClassName, this);
    //    ClassTitle = Smart.Format(ClassTitleFormat, this);

    //    var sb = new StringBuilder();
    //    sb.AppendLine(Smart.Format(UsingNameSpaceFormat, this));
    //    sb.AppendLine(Smart.Format(NameSpaceFormat, this));
    //    File.WriteAllText(Path.Combine(filePath, $"{resource.Name}.cs"), sb.ToString());
    //}
}
