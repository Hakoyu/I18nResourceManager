using CommunityToolkit.Mvvm.DependencyInjection;
using HKW.HKWUtils.Collections;
using HKW.HKWUtils.Observable;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace I18nResourceManager.Views.Main;

/// <summary>
/// EditWindow.xaml 的交互逻辑
/// </summary>
internal partial class MainWindow : WindowX
{
    private readonly ObservableListX<PageInfo> _pages = new();

    public MainWindow()
    {
        InitializeComponent();
        _pages.AddRange(
            App.MainWindowPages.Select(
                p =>
                    new PageInfo()
                    {
                        Name = p.Value.Name,
                        Description = p.Value.FullName,
                        Content = Ioc.Default.GetService(p.Value)
                    }
            )
        );
        ListBox_Main.ItemsSource = _pages;
        ListBox_Main.SelectedIndex = 0;
    }

    private void Frame_Main_ContentRendered(object sender, EventArgs e)
    {
        if (sender is not Frame frame)
            return;
        // 清理过时页面
        while (frame.CanGoBack)
            frame.RemoveBackEntry();
        GC.Collect();
    }
}
