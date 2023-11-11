using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Wpf;
using HKW.WPF.Extensions;
using I18nResourceManager.ViewModels;
using I18nResourceManager.ViewModels.Main;
using I18nResourceManager.Views;
using I18nResourceManager.Views.Main;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager;

public partial class App
{
    private static ILoggerFactory _loggerFactory = null!;
    public static Dictionary<string, Type> MainWindowPages { get; } = new();

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        AddLogger(services);
        AddMVVMDialogs(services);
        AddStartWindow(services);
        AddMainWindow(services);
        return services.BuildServiceProvider();
    }

    private static void AddMainWindow(IServiceCollection services)
    {
        MainWindowPages.Add(nameof(MainPage), typeof(MainPage));
        services.AddPage<MainPage, MainPageVM>();
        MainWindowPages.Add(nameof(SettingsPage), typeof(SettingsPage));
        services.AddPage<SettingsPage, SettingsPageVM>();
        MainWindowPages.Add(nameof(InfoPage), typeof(InfoPage));
        services.AddPage<InfoPage, InfoPageVM>();

        services.AddWindow<MainWindow, MainWindowVM>();
        services.AddWindow<EditTextWindow, EditTextWindowVM>();
        services.AddWindow<LoadFilesWindow, LoadFilesWindowVM>();
    }

    private static void AddStartWindow(IServiceCollection services)
    {
        services.AddWindow<StartWindow, StartWindowVM>();
    }

    private static void AddMVVMDialogs(IServiceCollection services)
    {
        services.AddSingleton<IDialogService>(
            new DialogService(
                new DialogManager(
                    viewLocator: new ViewLocator(),
                    logger: _loggerFactory.CreateLogger<DialogManager>()
                ),
                viewModelFactory: x => Ioc.Default.GetService(x)
            )
        );
    }

    private static void AddLogger(IServiceCollection services)
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.AddNLog());
        services.AddSingleton<ILoggerFactory>(sp => _loggerFactory);
    }
}
