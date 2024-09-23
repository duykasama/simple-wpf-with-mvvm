using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class DeleteConfirmedMessage : ValueChangedMessage<int>
{
    public DeleteConfirmedMessage(int value) : base(value)
    {
    }
}
