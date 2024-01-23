using Avalonia.Controls;
using Avalonia.Interactivity;

namespace I18nResourceManager.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class StartView : UserControl
{
    public StartView()
    {
        InitializeComponent();
    }

    private void Button_CreateNewProject_Click(object sender, RoutedEventArgs e)
    {
        //Visibility = Visibility.Hidden;
        //var editWindow = Ioc.Default.GetService<MainWindow>();
        //editWindow!.Closed += EditWindow_Closed;
        //editWindow.Show();
    }

    private void EditWindow_Closed(object? sender, EventArgs e)
    {
        //Visibility = Visibility.Visible;
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
