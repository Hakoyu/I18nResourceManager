using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Wpf;
using System;

namespace I18nResourceManager;

/// <summary>
/// Maps view models to views in Avalonia.
/// </summary>
public class ViewLocator : ViewLocatorBase
{
    /// <inheritdoc />
    protected override string GetViewName(object viewModel) =>
        viewModel.GetType().FullName!.Replace("ViewModels", "Views").Replace("VM", "");
}
