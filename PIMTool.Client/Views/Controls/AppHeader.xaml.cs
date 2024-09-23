using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Messages;
using System.Windows.Controls;

namespace PIMTool.Client.Views.Controls;
/// <summary>
/// Interaction logic for Header.xaml
/// </summary>
public partial class AppHeader : UserControl
{
    public AppHeader()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, HandleLanguageChange);
    }

    private void HandleLanguageChange(object _, LanguageChangedMessage __)
    {
        enLanguage.GetBindingExpression(TextBlock.ForegroundProperty)?.UpdateTarget();
        frLanguage.GetBindingExpression(TextBlock.ForegroundProperty)?.UpdateTarget();
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<LanguageChangedMessage>(this);
    }
}
