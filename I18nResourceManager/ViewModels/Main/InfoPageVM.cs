using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using I18nResourceManager.Models;
using Microsoft.Extensions.Logging;

namespace I18nResourceManager.ViewModels.Main;

internal partial class InfoPageVM : ObservableObject
{
    private readonly ILogger<InfoPageVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<InfoPageVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public InfoPageVM() { }

    #region Property


    #endregion

    #region Command

    #endregion
}
