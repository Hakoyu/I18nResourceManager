using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Globalization;

namespace I18nResourceManager.Models.Messages;

internal class EditCultureMessage
    : ValueChangedMessage<(string? OldCultureName, string? NewCultureName)>
{
    public EditCultureMessage(string? OldCultureName, string? NewCultureName)
        : base((OldCultureName, NewCultureName)) { }
}
