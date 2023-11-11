using HanumanInstitute.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models;

public static class Expansions
{
    public static string GetFullInfo(this CultureInfo cultureInfo)
    {
        return $"{cultureInfo.DisplayName} [{cultureInfo.Name}]";
    }

    //public static void SetVisible(this IDialogService dialogService,bool isVisible)
    //{
    //    var window = dialogService.DialogManager.FindViewByViewModel
    //}
}
