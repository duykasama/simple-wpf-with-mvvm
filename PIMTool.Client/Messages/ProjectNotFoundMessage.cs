using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class ProjectNotFoundMessage : ValueChangedMessage<int>
{
    public ProjectNotFoundMessage(int value) : base(value)
    {
    }
}
