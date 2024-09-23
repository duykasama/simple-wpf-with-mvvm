using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Constants;
using PIMTool.Client.Messages;
using PIMTool.Client.Models;
using System.Globalization;
using System.Windows;

namespace PIMTool.Client.ViewModels;

public class AppHeaderViewModel : ViewModelBase
{
    private CustomCommand _changeLanguageCommand;
    public CustomCommand ChangeLanguageCommand
    {
        get => _changeLanguageCommand;
        set => SetProperty(ref _changeLanguageCommand, value);
    }

    public AppHeaderViewModel()
    {
        _changeLanguageCommand = new CustomCommand(HandleChangeLanguage);
    }

    private void HandleChangeLanguage(object parameter)
    {
        var languageCode = parameter as string;
        var currentResources = Application.Current.Resources.MergedDictionaries;

        var languageDictionaries = currentResources
            .Where(d => d.Source != null && d.Source.OriginalString.Contains("/Resources/Languages/"))
            .ToList();

        foreach (var dictionary in languageDictionaries)
        {
            currentResources.Remove(dictionary);
        }

        string newLanguageDictionaryPath = $"/Resources/Languages/{languageCode}.xaml";
        var newLanguageDictionary = new ResourceDictionary { Source = new Uri(newLanguageDictionaryPath, UriKind.Relative) };

        currentResources.Add(newLanguageDictionary);

        OnPropertyChanged();
        var culture = languageCode switch
        {
            AppCultures.English => new CultureInfo(AppCultures.English),
            AppCultures.French => new CultureInfo(AppCultures.French),
            _ => throw new NotImplementedException(),
        };
        Thread.CurrentThread.CurrentCulture = culture;
        WeakReferenceMessenger.Default.Send(new LanguageChangedMessage(languageCode ?? string.Empty));
    }
}
