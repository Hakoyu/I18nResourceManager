using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HKW.HKWUtils.Collections;
using I18nResourceManager.Models;

namespace I18nResourceManager.ViewModels.Main;

internal partial class SettingsPageVM : ObservableObject
{
    private readonly ILogger<SettingsPageVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<SettingsPageVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public Settings Settings => Settings.Instance;

    public SettingsPageVM() { }

    #region Property

    #endregion

    #region Command

    #endregion
}
