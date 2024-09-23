using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PIMTool.Client.Messages;

public class LanguageChangedMessage : ValueChangedMessage<string>
{
    public LanguageChangedMessage(string language) : base(language)
    {
    }
}
