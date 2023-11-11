using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;

namespace I18nResourceManager.ViewModels.Main;

internal partial class LoadFilesWindowVM : ObservableObject
{
    private readonly ILogger<LoadFilesWindowVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<LoadFilesWindowVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public LoadFilesWindowVM() { }

    #region Property

    #endregion

    #region Command

    #endregion
}
