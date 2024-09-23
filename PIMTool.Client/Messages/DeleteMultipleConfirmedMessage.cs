using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class DeleteMultipleConfirmedMessage : ValueChangedMessage<bool>
{
    public DeleteMultipleConfirmedMessage(bool value) : base(value)
    {
    }
}
