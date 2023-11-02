using CommunityToolkit.Mvvm.Messaging;
using HKW.HKWUtils.Collections;
using I18nResourceManager.Models;
using I18nResourceManager.Models.Messages;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace I18nResourceManager.Views.Main;

/// <summary>
/// MainPage.xaml 的交互逻辑
/// </summary>
internal partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<EditCultureMessage>(
            this,
            (s, e) =>
            {
                if (
                    string.IsNullOrEmpty(e.Value.oldCultureName)
                    && string.IsNullOrEmpty(e.Value.newCultureName)
                )
                    throw new("???");
                if (
                    string.IsNullOrEmpty(e.Value.oldCultureName) is false
                    && string.IsNullOrEmpty(e.Value.newCultureName) is false
                )
                {
                    var newCulture = CultureInfo.GetCultureInfo(e.Value.newCultureName);
                    var oldCulture = CultureInfo.GetCultureInfo(e.Value.oldCultureName);
                    ReplaceCulture(oldCulture, newCulture);
                }
                else if (string.IsNullOrEmpty(e.Value.newCultureName) is false)
                {
                    var newCulture = CultureInfo.GetCultureInfo(e.Value.newCultureName);
                    AddCulture(newCulture);
                }
                else if (string.IsNullOrEmpty(e.Value.oldCultureName) is false)
                {
                    var oldCulture = CultureInfo.GetCultureInfo(e.Value.oldCultureName);
                    RemoveCulture(oldCulture);
                }
            }
        );
    }

    private readonly Dictionary<string, DataGridI18nColumn> _dataGridI18nColumns = new();

    // TODO: 国际化标头
    private const string ValueBindingFormat = "Datas[{0}].Value";
    private const string CommentBindingFormat = "Datas[{0}].Comment";
    private const string CommentHeaderFormat = "{0} - Comment";

    /// <summary>
    /// 添加文化列
    /// </summary>
    /// <param name="culture"></param>
    public void AddCulture(CultureInfo culture)
    {
        // 文化数据列
        var valueColumn = new DataGridTextColumn()
        {
            Header = culture.GetFullInfo(),
            Binding = new Binding(string.Format(ValueBindingFormat, culture.Name))
            {
                Mode = BindingMode.TwoWay
            }
        };
        // 文化数据注释列
        var commentColumn = new DataGridTextColumn()
        {
            Header = string.Format(CommentHeaderFormat, culture.GetFullInfo()),
            Binding = new Binding(string.Format(CommentBindingFormat, culture.Name))
            {
                Mode = BindingMode.TwoWay
            }
        };
        DataGrid_Datas.Columns.Add(valueColumn);
        DataGrid_Datas.Columns.Add(commentColumn);
        _dataGridI18nColumns.Add(culture.Name, new(valueColumn, commentColumn));
    }

    /// <summary>
    /// 删除文化列
    /// </summary>
    /// <param name="culture"></param>
    public void RemoveCulture(CultureInfo culture)
    {
        DataGrid_Datas.Columns.Remove(_dataGridI18nColumns[culture.Name].Value);
        DataGrid_Datas.Columns.Remove(_dataGridI18nColumns[culture.Name].Comment);
        _dataGridI18nColumns.Remove(culture.Name);
    }

    /// <summary>
    /// 替换文化列
    /// </summary>
    /// <param name="oldCulture"></param>
    /// <param name="newCulture"></param>
    public void ReplaceCulture(CultureInfo oldCulture, CultureInfo newCulture)
    {
        //if (_dataGridI18nColumns.ContainsKey(newCultureName))
        //    throw new();
        var i18nColumn = _dataGridI18nColumns[oldCulture.Name];
        i18nColumn.Value.Header = newCulture.GetFullInfo();
        i18nColumn.Value.Binding = new Binding(string.Format(ValueBindingFormat, newCulture.Name));
        i18nColumn.Comment.Header = string.Format(CommentHeaderFormat, newCulture.GetFullInfo());
        i18nColumn.Comment.Binding = new Binding(
            string.Format(CommentBindingFormat, newCulture.Name)
        );
        _dataGridI18nColumns.Remove(oldCulture.Name);
        _dataGridI18nColumns.Add(newCulture.Name, i18nColumn);
    }

    private void DataGrid_Datas_CurrentCellChanged(object sender, EventArgs e)
    {
        // 选中单元格切换时, 绑定到编辑框上, 以便输入换行
        if (sender is not DataGrid dataGrid)
            return;
        // 编辑框被选中时, CurrentCell会变为null, 无视
        if (TextBox_EditText.IsKeyboardFocused)
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

public class DataGridI18nColumn
{
    public DataGridTextColumn Value { get; set; } = null!;
    public DataGridTextColumn Comment { get; set; } = null!;

    public DataGridI18nColumn() { }

    public DataGridI18nColumn(DataGridTextColumn value, DataGridTextColumn comment)
    {
        Value = value;
        Comment = comment;
    }
}
