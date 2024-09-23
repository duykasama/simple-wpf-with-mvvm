using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class ShouldUnregisterMessage : ValueChangedMessage<bool>
{
    public ShouldUnregisterMessage(bool value) : base(value)
    {
    }
}
