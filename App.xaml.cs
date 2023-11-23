using CommunityToolkit.Mvvm.DependencyInjection;
using I18nResourceManager.Views;
using I18nResourceManager.Views.Main;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Media;

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
        SetWindowGlassBrush();
        //var window = Ioc.Default.GetService<StartWindow>();
        //window!.Show();
        var window = Ioc.Default.GetService<MainWindow>();
        window!.Show();
    }

    private static void SetWindowGlassBrush()
    {
        // 设置主题色
        var windowGlassBrush = SystemParameters.WindowGlassBrush;
        Current.Resources[nameof(SystemParameters.WindowGlassBrush)] = windowGlassBrush;
        // 根据主题色的明亮程度来设置字体颜色
        var color = (Color)ColorConverter.ConvertFromString(windowGlassBrush.ToString());
        if (IsLightColor(color))
        {
            Current.Resources["G_ForegroundOnWindowGlassBrush"] = Current.Resources["G_DarkColor"];
        }
        else
        {
            Current.Resources["G_ForegroundOnWindowGlassBrush"] = Current.Resources["G_LightColor"];
        }
    }

    /// <summary>
    /// 检测颜色是否为亮色调
    /// </summary>
    /// <param name="color">颜色</param>
    /// <returns>是为<see langword="true"/>,不是为<see langword="false"/></returns>
    private static bool IsLightColor(Color color)
    {
        return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255 > 0.5;
    }
}
