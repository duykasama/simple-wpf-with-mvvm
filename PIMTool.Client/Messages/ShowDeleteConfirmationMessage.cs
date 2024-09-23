using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class ShowDeleteConfirmationMessage : ValueChangedMessage<int?>
{
    public ShowDeleteConfirmationMessage(int? value) : base(value)
    {
    }
}
