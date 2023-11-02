using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Globalization;

namespace I18nResourceManager.Models.Messages;

internal class EditCultureMessage
    : ValueChangedMessage<(string? oldCultureName, string? newCultureName)>
{
    public EditCultureMessage(string? oldCultureName, string? newCultureName)
        : base((oldCultureName, newCultureName)) { }
}
