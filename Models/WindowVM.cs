using CommunityToolkit.Mvvm.ComponentModel;
using HanumanInstitute.MvvmDialogs;
using System.ComponentModel;
using HKW.HKWUtils.Events;

namespace I18nResourceManager.Models;

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
            var asyncEvent in AsyncClosing
                .GetInvocationList()
                .Cast<XAsyncEventHandler<T, CancelEventArgs>>()
        )
        {
            if (asyncEvent is not null)
                await asyncEvent((T)this, e);
        }
        if (e.Cancel)
            return;
    }

    public void OnClosed()
    {
        Closed?.Invoke((T)this);
    }

    public event XEventHandler<T>? Closed;
    public event XEventHandler<T, CancelEventArgs>? Closing;
    public event XAsyncEventHandler<T, CancelEventArgs>? AsyncClosing;
}
