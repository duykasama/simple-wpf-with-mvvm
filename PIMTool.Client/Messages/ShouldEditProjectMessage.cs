using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class ShouldEditProjectMessage : ValueChangedMessage<int>
{
    public int ProjectId { get; set; }

    public ShouldEditProjectMessage(int value) : base(value)
    {
        ProjectId = value;
    }
}
