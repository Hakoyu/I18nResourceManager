using CommunityToolkit.Mvvm.Messaging;
using I18nResourceManager.Models.Messages;
using I18nResourceManager.ViewModels.Main;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
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
/// LoadFilesWindow.xaml 的交互逻辑
/// </summary>
public partial class LoadFilesWindow : WindowX
{
    public LoadFilesWindow()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<EditCultureMessage, Guid>(
            this,
            LoadFilesWindowVM.MessageToken,
            MessageHandler
        );
    }

    private void MessageHandler(object sender, EditCultureMessage message)
    {
        if (message.Value.OldCultureInfo is null && message.Value.NewCultureInfo is null)
            throw new("???");
        var oldInfo = message.Value.OldCultureInfo;
        var newInfo = message.Value.NewCultureInfo;
        if (oldInfo is not null && newInfo is not null)
        {
            MainPage.ReplaceCulture(_dataGridI18nColumns, oldInfo.Value, newInfo.Value);
        }
        else if (newInfo is not null)
        {
            MainPage.AddCulture(DataGrid_Datas, _dataGridI18nColumns, newInfo.Value);
        }
        else if (oldInfo is not null)
        {
            MainPage.RemoveCulture(DataGrid_Datas, _dataGridI18nColumns, oldInfo.Value);
        }
    }

    private readonly Dictionary<string, DataGridI18nColumn> _dataGridI18nColumns = new();

    private void DataGrid_Datas_CurrentCellChanged(object sender, EventArgs e)
    {
        // 选中单元格切换时, 绑定到编辑框上, 以便输入换行
        if (sender is not DataGrid dataGrid)
            return;
        // 编辑框被选中时, CurrentCell会变为null, 无视
        if (TextBox_EditText.IsKeyboardFocused || GridSplitter_1.IsKeyboardFocused)
            return;
        // 为空或者选中到 Id 列时, 清空并禁用编辑框
        if (dataGrid.CurrentCell.Column is null || dataGrid.CurrentCell.Column.Header is "Id")
        {
            // 由于绑定到虚拟值会显示绑定失败, 则判断如果已经绑定到虚拟值就返回, 以此减少绑定失败的次数 (好像并没有什么用?)
            if (
                BindingOperations.GetBinding(TextBox_EditText, TextBox.TextProperty)
                    is Binding binding1
                && binding1.Path.Path == "?"
            )
                return;
            //BindingOperations.ClearBinding(TextBox_EditText, TextBlock.TextProperty);
            //if (BindingOperations.IsDataBound(TextBox_EditText, TextBox.TextProperty))
            // 清除绑定后没有立刻生效, 故绑定一个虚拟值 (注意:此举会提示绑定错误)
            TextBox_EditText.SetBinding(TextBox.TextProperty, "?");
            TextBox_EditText.Text = string.Empty;
            TextBox_EditText.IsEnabled = false;
            return;
        }
        // 将 Text 绑定与单元格相同的内容上
        var binding = new Binding()
        {
            Source = dataGrid.CurrentCell.Item,
            Path = new(dataGrid.CurrentCell.Column.SortMemberPath),
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        TextBox_EditText.SetBinding(TextBox.TextProperty, binding);
        TextBox_EditText.IsEnabled = true;
    }
}
