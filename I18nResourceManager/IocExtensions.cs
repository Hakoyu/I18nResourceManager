using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace I18nResourceManager;

public static class AvaloniaExtensions
{
    public static TElement SetDataContext<TElement, TViewModel>(this TElement element)
        where TElement : StyledElement
        where TViewModel : ObservableObject
    {
        element.DataContext = Ioc.Default.GetService<TViewModel>();
        return element;
    }

    public static TElement SetDataContext<TElement, TViewModel>(
        this TElement element,
        TViewModel viewModel
    )
        where TElement : StyledElement
        where TViewModel : ObservableObject
    {
        element.DataContext = viewModel;
        return element;
    }
}

/// <summary>
/// IOC拓展
/// </summary>
public static class IocExtensions
{
    #region AddService
    #region Window
    /// <summary>
    /// 添加窗口
    /// </summary>
    /// <typeparam name="TWindow">窗口类型</typeparam>
    /// <typeparam name="TWindowVM">窗口视图模型类型</typeparam>
    /// <param name="services"></param>
    /// <param name="setDataContext">是否在窗口创建时添加数据上下文</param>
    public static void AddWindow<TWindow, TWindowVM>(
        this IServiceCollection services,
        bool setDataContext = true
    )
        where TWindow : Window, new()
        where TWindowVM : ObservableObject, new()
    {
        services.AddTransient<TWindowVM>();
        services.AddTransient<TWindow>(sp =>
        {
            var window = new TWindow();
            if (setDataContext)
                window.DataContext = sp.GetService<TWindowVM>();
            RegisterWindowShutdown(window);
            return window;
        });
    }

    /// <summary>
    /// 添加窗口
    /// </summary>
    /// <typeparam name="TWindow">窗口类型</typeparam>
    /// <typeparam name="TWindowVM">窗口视图模型类型</typeparam>
    /// <param name="services"></param>
    /// <param name="window">窗口</param>
    /// <param name="windowVM">窗口视图模型</param>
    /// <param name="setDataContext">是否在窗口创建时添加数据上下文</param>
    public static void AddWindow<TWindow, TWindowVM>(
        this IServiceCollection services,
        Func<IServiceProvider, TWindow> window,
        Func<IServiceProvider, TWindowVM> windowVM,
        bool setDataContext = true
    )
        where TWindow : Window
        where TWindowVM : ObservableObject
    {
        services.AddTransient<TWindowVM>(sp => windowVM(sp));
        services.AddTransient<TWindow>(sp =>
        {
            var result = window(sp);
            if (setDataContext)
                result.DataContext = sp.GetService<TWindowVM>();
            RegisterWindowShutdown(result);
            return result;
        });
    }
    #endregion

    #region UserControl
    /// <summary>
    /// 添加用户控件
    /// </summary>
    /// <typeparam name="TUserControl">用户控件类型</typeparam>
    /// <typeparam name="TUserControlVM">用户控件视图模型类型</typeparam>
    /// <param name="services"></param>
    /// <param name="setDataContext">是否在用户控件创建时添加数据上下文</param>
    public static void AddUserControl<TUserControl, TUserControlVM>(
        this IServiceCollection services,
        bool setDataContext = true
    )
        where TUserControl : UserControl, new()
        where TUserControlVM : ObservableObject, new()
    {
        services.AddTransient<TUserControlVM>();
        services.AddTransient<TUserControl>(sp =>
        {
            var uc = new TUserControl();
            if (setDataContext)
                uc.DataContext = sp.GetService<TUserControlVM>();
            return uc;
        });
    }

    /// <summary>
    /// 添加用户控件
    /// </summary>
    /// <typeparam name="TUserControl">用户控件类型</typeparam>
    /// <typeparam name="TUserControlVM">用户控件视图模型类型</typeparam>
    /// <param name="services"></param>
    /// <param name="userControl">用户控件</param>
    /// <param name="userControlVM">用户控件视图模型</param>
    /// <param name="setDataContext">是否在窗口创建时添加数据上下文</param>
    public static void AddUserControl<TUserControl, TUserControlVM>(
        this IServiceCollection services,
        Func<IServiceProvider, TUserControl> userControl,
        Func<IServiceProvider, TUserControlVM> userControlVM,
        bool setDataContext = true
    )
        where TUserControl : UserControl
        where TUserControlVM : ObservableObject
    {
        services.AddTransient<TUserControlVM>(sp => userControlVM(sp));
        services.AddTransient<TUserControl>(sp =>
        {
            var result = userControl(sp);
            if (setDataContext)
                result.DataContext = sp.GetService<TUserControlVM>();
            return result;
        });
    }
    #endregion
    #endregion
    /// <summary>
    /// 注册窗口关闭事件
    /// </summary>
    /// <typeparam name="T">窗口类型</typeparam>
    /// <param name="window">窗口</param>
    public static void RegisterWindowShutdown<T>(T window)
        where T : Window
    {
        window.Closed += Window_Closed;
    }

    /// <summary>
    /// 窗口关闭事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void Window_Closed(object? sender, EventArgs e)
    {
        ClearDataContext(sender);
    }

    /// <summary>
    /// 开始关闭事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void Dispatcher_ShutdownStarted(object? sender, EventArgs e)
    {
        ClearDataContext(sender);
    }

    /// <summary>
    /// 清除DataContext
    /// </summary>
    /// <param name="obj"></param>
    private static void ClearDataContext(object? obj)
    {
        if (obj is StyledElement se)
        {
            try
            {
                se.DataContext = null;
            }
            catch { }
        }
    }
}
