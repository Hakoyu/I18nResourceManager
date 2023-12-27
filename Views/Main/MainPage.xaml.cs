using CommunityToolkit.Mvvm.Messaging;
using HKW.HKWUtils.Collections;
using HKW.WPF.Converters;
using I18nResourceManager.Models;
using I18nResourceManager.Models.Messages;
using I18nResourceManager.ViewModels.Main;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace I18nResourceManager.Views.Main;

/// <summary>
/// MainPage.xaml 的交互逻辑
/// </summary>
internal partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        // 注册消息
        WeakReferenceMessenger.Default.Register<EditCultureMessage, Guid>(
            this,
            MainPageVM.MessageToken,
            MessageHandler
        );
        // 注册关闭事件
        Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
    }

    private void Dispatcher_ShutdownStarted(object? sender, EventArgs e)
    {
        // 关闭后解除消息的注册
        WeakReferenceMessenger.Default.Unregister<EditCultureMessage, Guid>(
            this,
            MainPageVM.MessageToken
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
            ReplaceCulture(_dataGridI18nColumns, oldInfo.Value, newInfo.Value);
        }
        else if (newInfo is not null)
        {
            AddCulture(DataGrid_Datas, _dataGridI18nColumns, newInfo.Value);
        }
        else if (oldInfo is not null)
        {
            RemoveCulture(DataGrid_Datas, _dataGridI18nColumns, oldInfo.Value);
        }
    }

    private readonly Dictionary<string, DataGridI18nColumn> _dataGridI18nColumns = new();

    public const string ValueBindingFormat = "Texts[{0}].Value";
    public const string CommentBindingFormat = "Texts[{0}].Comment";
    public const string SearchValueBindingFormat = "SearchTexts[{0}].Value";
    public const string SearchCommentBindingFormat = "SearchTexts[{0}].Comment";
    public const string CommentHeaderFormat = "{0}.Comment";

    public static object CreateHeader(Binding binding, string name, string searchBindingPath)
    {
        var panel = new DockPanel();
        panel.SetBinding(DataContextProperty, binding);
        var nameTextBlock = new TextBlock()
        {
            VerticalAlignment = VerticalAlignment.Center,
            Text = name
        };
        panel.Children.Add(nameTextBlock);

        var searchTextBox = new TextBox()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Padding = new(0, 5, 0, 5),
            Margin = new(5, 0, 5, 0)
        };
        DockPanel.SetDock(searchTextBox, Dock.Right);
        searchTextBox.SetBinding(
            TextBox.TextProperty,
            new Binding(searchBindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
        TextBoxHelper.SetWatermark(searchTextBox, "🔍");
        WatermarkHelper.SetMargin(searchTextBox, new(0));
        panel.Children.Add(searchTextBox);
        return panel;
    }

    public static void UpdateHeader(object header, string name, string searchBindingPath)
    {
        if (header is not StackPanel sp)
            throw new Exception("???");
        var nameTextBlock = (TextBlock)sp.Children[0];
        var searchTextBox = (TextBox)sp.Children[1];
        nameTextBlock.Text = name;
        searchTextBox.SetBinding(
            TextBox.TextProperty,
            new Binding(searchBindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
    }

    /// <summary>
    /// 添加文化列
    /// </summary>
    /// <param name="culture"></param>
    public static void AddCulture(
        DataGrid dataGrid,
        Dictionary<string, DataGridI18nColumn> i18nColumns,
        SimpleCultureInfo culture
    )
    {
        // 文化数据列
        var valueColumn = new DataGridTextColumn()
        {
            Header = CreateHeader(
                new Binding(nameof(LoadFilesWindowVM.CultureLoadFileInfo))
                {
                    Source = dataGrid.DataContext
                },
                culture.FullName,
                string.Format(SearchValueBindingFormat, culture.Name)
            ),
            Binding = new Binding(string.Format(ValueBindingFormat, culture.Name))
        };
        // 文化数据注释列
        var commentColumn = new DataGridTextColumn()
        {
            Header = CreateHeader(
                new Binding(nameof(LoadFilesWindowVM.CultureLoadFileInfo))
                {
                    Source = dataGrid.DataContext
                },
                string.Format(CommentHeaderFormat, culture.FullName),
                string.Format(SearchCommentBindingFormat, culture.Name)
            ),
            Binding = new Binding(string.Format(CommentBindingFormat, culture.Name))
        };
        dataGrid.Columns.Add(valueColumn);
        dataGrid.Columns.Add(commentColumn);
        i18nColumns.Add(culture.Name, new(valueColumn, commentColumn));
    }

    /// <summary>
    /// 删除文化列
    /// </summary>
    /// <param name="culture"></param>
    public static void RemoveCulture(
        DataGrid dataGrid,
        Dictionary<string, DataGridI18nColumn> i18nColumns,
        SimpleCultureInfo culture
    )
    {
        if (i18nColumns.Remove(culture.Name, out var column) is false)
            return;
        dataGrid.Columns.Remove(column.Value);
        dataGrid.Columns.Remove(column.Comment);
    }

    /// <summary>
    /// 替换文化列
    /// </summary>
    /// <param name="oldCulture"></param>
    /// <param name="newCulture"></param>
    public static void ReplaceCulture(
        Dictionary<string, DataGridI18nColumn> i18nColumns,
        SimpleCultureInfo oldCulture,
        SimpleCultureInfo newCulture
    )
    {
        if (oldCulture.Name == newCulture.Name)
            return;
        if (i18nColumns.Remove(oldCulture.Name, out var column) is false)
            return;
        UpdateHeader(
            column.Value.Header,
            newCulture.FullName,
            string.Format(SearchValueBindingFormat, newCulture.Name)
        );
        column.Value.Binding = new Binding(string.Format(ValueBindingFormat, newCulture.Name));

        UpdateHeader(
            column.Comment.Header,
            string.Format(CommentHeaderFormat, newCulture.FullName),
            string.Format(SearchCommentBindingFormat, newCulture.Name)
        );
        column.Comment.Binding = new Binding(string.Format(CommentBindingFormat, newCulture.Name));

        i18nColumns.Add(newCulture.Name, column);
    }

    private void DataGrid_Datas_CurrentCellChanged(object sender, EventArgs e)
    {
        // 选中单元格切换时, 绑定到编辑框上, 以便输入换行
        if (sender is not DataGrid dataGrid)
            return;
        // 编辑框被选中时, CurrentCell会变为null, 无视
        if (TextBox_EditText.IsKeyboardFocused || GridSplitter_1.IsKeyboardFocused)
            return;
        // 为空或者选中到 Id 列时, 清空并禁用编辑框
        if (
            dataGrid.CurrentCell.Column is null
            || dataGrid.CurrentCell.Column.Header is nameof(I18nData.Id)
        )
        {
            // 由于绑定到虚拟值会显示绑定失败, 则判断如果已经绑定到虚拟值就返回, 以此减少绑定失败的次数 (好像并没有什么用?)
            if (
                BindingOperations.GetBinding(TextBox_EditText, TextBox.TextProperty)
                    is Binding oldBinding
                && oldBinding.Source is null
            )
                return;
            // 清除绑定后没有立刻生效, 故绑定一个虚拟值 (注意:此举会提示绑定错误)
            TextBox_EditText.SetBinding(TextBox.TextProperty, nameof(string.Empty));
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

public class DataGridI18nColumn
{
    public DataGridTextColumn Value { get; }
    public DataGridTextColumn Comment { get; }

    public DataGridI18nColumn(DataGridTextColumn value, DataGridTextColumn comment)
    {
        Value = value;
        Comment = comment;
    }
}
