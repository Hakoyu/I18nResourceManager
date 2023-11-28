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

namespace I18nResourceManager.Views;

/// <summary>
/// EditCultureWindow.xaml 的交互逻辑
/// </summary>
internal partial class EditTextWindow : WindowX
{
    public EditTextWindow()
    {
        InitializeComponent();
        TextBox_Main.Focus();
    }
}
