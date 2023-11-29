using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Globalization;

namespace I18nResourceManager.Models.Messages;

internal class EditCultureMessage
    : ValueChangedMessage<(SimpleCultureInfo? OldCultureInfo, SimpleCultureInfo? NewCultureInfo)>
{
    public EditCultureMessage(SimpleCultureInfo? OldCultureInfo, SimpleCultureInfo? NewCultureInfo)
        : base((OldCultureInfo, NewCultureInfo)) { }
}
