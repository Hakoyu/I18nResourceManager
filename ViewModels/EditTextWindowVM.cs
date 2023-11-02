using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using System.ComponentModel;
using I18nResourceManager.Models;
using System.Globalization;

namespace I18nResourceManager.ViewModels;

internal partial class EditTextWindowVM : WindowVM<EditTextWindowVM>, IModalDialogViewModel
{
    private readonly ILogger<EditTextWindowVM> _logger = Ioc.Default
        .GetService<ILoggerFactory>()!
        .CreateLogger<EditTextWindowVM>();

    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    public EditTextWindowVM() { }

    public bool? DialogResult { get; private set; } = false;

    #region Property
    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _watermark;

    [ObservableProperty]
    private string? _text;

    #endregion

    #region Command
    [RelayCommand]
    private void Cancel()
    {
        _dialogService.Close(this);
    }

    [RelayCommand]
    private void OK()
    {
        DialogResult = true;
        _dialogService.Close(this);
    }
    #endregion
}
