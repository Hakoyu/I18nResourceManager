using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using System;

namespace I18nResourceManager;

/// <summary>
/// Maps view models to views in Avalonia.
/// </summary>
internal class ViewLocator : ViewLocatorBase
{
    /// <inheritdoc />
    protected override string GetViewName(object viewModel) =>
        viewModel.GetType().FullName!.Replace("ViewModels", "Views").Replace("VM", "Window");
}
