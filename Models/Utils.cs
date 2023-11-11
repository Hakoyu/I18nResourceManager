using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models;

public static class Utils
{
    public const string I18nResInfoFile = "I18nResInfo.toml";
    public static OpenFileDialogSettings I18nResInfoSettings { get; } =
        new() { Filters = new() { new("I18nResInfo File", "toml") } };
}
