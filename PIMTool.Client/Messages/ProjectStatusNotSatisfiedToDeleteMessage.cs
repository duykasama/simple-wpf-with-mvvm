using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class ProjectStatusNotSatisfiedToDeleteMessage : ValueChangedMessage<string>
{
    public string ErrorMessage { get; set; }

    public ProjectStatusNotSatisfiedToDeleteMessage(string value) : base(value)
    {
        ErrorMessage = value;
    }
}
