using CommunityToolkit.Mvvm.DependencyInjection;
using I18nResourceManager.Views;
using I18nResourceManager.Views.Main;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace I18nResourceManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Ioc.Default.ConfigureServices(ConfigureServices());
        Startup += App_Startup;
    }

    private void App_Startup(object sender, StartupEventArgs e)
    {
        var window = Ioc.Default.GetService<StartWindow>();
        window!.Show();
    }
}
