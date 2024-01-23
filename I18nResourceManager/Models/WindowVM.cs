using CommunityToolkit.Mvvm.ComponentModel;
using HanumanInstitute.MvvmDialogs;
using System.ComponentModel;

namespace I18nResourceManager.Models;

/// <summary>
/// 窗口视图模型
/// <para>
/// 包含窗口关闭事件
/// </para>
/// </summary>
/// <typeparam name="T">类型</typeparam>
public class WindowVM<T> : ObservableObject, IViewClosing, IViewClosed
    where T : WindowVM<T>
{
    public void OnClosing(CancelEventArgs e)
    {
        Closing?.Invoke((T)this, e);
        if (e.Cancel)
            return;
    }

    public async Task OnClosingAsync(CancelEventArgs e)
    {
        if (AsyncClosing is null)
            return;
        foreach (
            var asyncEvent in AsyncClosing.GetInvocationList().Cast<ClosingAsyncEventHandler<T>>()
        )
            await asyncEvent((T)this, e);
        if (e.Cancel)
            return;
    }

    public void OnClosed()
    {
        Closed?.Invoke((T)this, new());
    }

    public event ClosedEventHandler<T>? Closed;
    public event ClosingEventHandler<T>? Closing;
    public event ClosingAsyncEventHandler<T>? AsyncClosing;
}

public delegate void ClosedEventHandler<T>(T shender, EventArgs e);
public delegate void ClosingEventHandler<T>(T shender, CancelEventArgs e);
public delegate Task ClosingAsyncEventHandler<T>(T shender, CancelEventArgs e);
