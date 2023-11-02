using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using I18nResourceManager.ViewModels;
using I18nResourceManager.Views;
using I18nResourceManager.Views.Main;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace I18nResourceManager.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
internal partial class StartWindow : WindowX
{
    public StartWindow()
    {
        InitializeComponent();
    }

    public StartWindowVM ViewModel => (StartWindowVM)DataContext;
    private readonly IDialogService _dialogService = Ioc.Default.GetService<IDialogService>()!;

    private void Button_CreateNewProject_Click(object sender, RoutedEventArgs e)
    {
        Visibility = Visibility.Hidden;
        var editWindow = Ioc.Default.GetService<MainWindow>();
        editWindow!.Closed += EditWindow_Closed;
        editWindow.Show();
    }

    private void EditWindow_Closed(object? sender, EventArgs e)
    {
        Visibility = Visibility.Visible;
    }

    private void Button_LoadProjectFromFile_Click(object sender, RoutedEventArgs e)
    {
        //_dialogService.ShowOpenFolderDialog(this);
        //OpenFileDialog openFileDialog = new() { Title = "文件", Filter = $"文件|*.*" };
        //if (openFileDialog.ShowDialog() is true)
        //{
        //    ViewModel.LoadProjectFromDirectoryCommand.Execute(openFileDialog.FileName);
        //}
    }

    //private void Button_Click(object sender, RoutedEventArgs e)
    //{
    //    //DataGrid_Main.Columns.Add(
    //    //    new DataGridTextColumn()
    //    //    {
    //    //        Header = DataGrid_Main.Columns.Count,
    //    //        Binding = new Binding($"Pairs[{DataGrid_Main.Columns.Count}]")
    //    //    }
    //    //);
    //}
}
