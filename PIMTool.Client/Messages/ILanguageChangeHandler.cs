namespace PIMTool.Client.Messages;

public interface ILanguageChangeHandler
{
    void HandleLanguageChange(object parameter, LanguageChangedMessage message);
}
