using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using I18nResourceManager.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.ViewModels.Main;

internal partial class MainWindowVM : WindowVM<MainWindowVM>
{
    private readonly ILogger<InfoPageVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<InfoPageVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public static MainWindowVM Instance { get; private set; } = null!;

    public string ProjectPath { get; set; }

    public MainWindowVM()
    {
        Instance = this;
    }
    #region Property

    #endregion

    #region Command

    #endregion
}
