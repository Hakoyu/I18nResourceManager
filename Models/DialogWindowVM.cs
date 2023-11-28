using HanumanInstitute.MvvmDialogs;

namespace I18nResourceManager.Models;

/// <summary>
/// 对话框窗口视图模型
/// <para>
/// 包含窗口关闭事件
/// </para>
/// </summary>
/// <typeparam name="T">类型</typeparam>
public class DialogWindowVM<T> : WindowVM<T>, IModalDialogViewModel
    where T : DialogWindowVM<T>
{
    public bool? DialogResult => false;
}
