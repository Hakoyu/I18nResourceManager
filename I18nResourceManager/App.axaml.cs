using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using I18nResourceManager.ViewModels;
using I18nResourceManager.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace I18nResourceManager;

public partial class App : Application
{
    private static ILoggerFactory _loggerFactory = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Ioc.Default.ConfigureServices(ConfigureServices());
    }

    /// <summary>
    /// 设置Ioc服务
    /// </summary>
    /// <returns></returns>
    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        AddLogger(services);
        AddMVVMDialogs(services);

        AddStartView(services);

        return services.BuildServiceProvider();
    }

    private static void AddStartView(ServiceCollection services)
    {
        services.AddTransient<StartWindow>();
        services.AddTransient<StartView>();
        services.AddTransient<StartVM>();
    }

    /// <summary>
    /// 添加视图模型弹窗
    /// </summary>
    /// <param name="services"></param>
    private static void AddMVVMDialogs(IServiceCollection services)
    {
        services.AddSingleton<IDialogService>(
            new DialogService(
                new DialogManager(
                    viewLocator: new ViewLocator(),
                    dialogFactory: new DialogFactory().AddMessageBox(),
                    logger: _loggerFactory.CreateLogger<DialogManager>()
                ),
                viewModelFactory: x => Ioc.Default.GetService(x)
            )
        );
    }

    /// <summary>
    /// 添加记录器
    /// </summary>
    /// <param name="services"></param>
    private static void AddLogger(IServiceCollection services)
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.AddNLog());
        services.AddSingleton<ILoggerFactory>(sp => _loggerFactory);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 用于桌面
            desktop.MainWindow = Ioc.Default
                .GetService<StartWindow>()!
                .SetDataContext<StartWindow, StartVM>();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            // 用于单页面视图(例如网页和手机)
            singleViewPlatform.MainView = Ioc.Default
                .GetService<StartView>()!
                .SetDataContext<StartView, StartVM>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
