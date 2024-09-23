using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Constants;
using PIMTool.Client.Messages;
using PIMTool.Client.ValidationRules;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace PIMTool.Client.Views.Pages;
/// <summary>
/// Interaction logic for CreateProjectPage.xaml
/// </summary>
public partial class CreateProjectPage : Page
{
    public CreateProjectPage()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, HandleLanguageChange);
        WeakReferenceMessenger.Default.Register<ProjectNotFoundMessage>(this, HandleProjectNotFoundEvent);
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!IsTextAllowed(e.Text))
        {
            e.Handled = true;
        }
    }

    private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            string text = (string)e.DataObject.GetData(typeof(string));
            if (!IsTextAllowed(text))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }

    private static bool IsTextAllowed(string text)
    {
        return ProjectNumberRegex().IsMatch(text);
    }

    [GeneratedRegex("^[0-9]+$")]
    private static partial Regex ProjectNumberRegex();

    private void MemberErrorContainer_TextChange(object sender, TextChangedEventArgs e)
    {
        var errorText = memberErrorContainer.Text;

        if (string.IsNullOrEmpty(errorText))
        {
            Validation.ClearInvalid(membersTextBox.GetBindingExpression(TextBox.TextProperty));
            return;
        }

        var validationError = new ValidationError(new MembersValidationRule(), membersTextBox.GetBindingExpression(TextBox.TextProperty))
        {
            ErrorContent = memberErrorContainer.Text
        };
        Validation.MarkInvalid(membersTextBox.GetBindingExpression(TextBox.TextProperty), validationError);
    }

    private void HandleLanguageChange(object _, LanguageChangedMessage message)
    {
        titleCreateOrEdit.GetBindingExpression(Label.ContentProperty).UpdateTarget();
        buttonCreateOrEdit.GetBindingExpression(Button.ContentProperty).UpdateTarget();

        BindingOperations.GetBindingExpression(projectStatuses, ComboBox.TextProperty)?.UpdateTarget();
        foreach (var status in projectStatuses.Items)
        {
            if (projectStatuses.ItemContainerGenerator.ContainerFromItem(status) is ComboBoxItem container)
            {
                var textBlocks = FindVisualChildren<TextBlock>(container);
                foreach (var textBlock in textBlocks)
                {
                    textBlock?.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                }
            }
        }
    }

    private void HandleProjectNotFoundEvent(object _, ProjectNotFoundMessage message)
    {
        var messageTemplate = Application.Current.Resources[MultilingualKey.ProjectNotFound]?.ToString() ?? string.Empty;
        var caption = Application.Current.Resources[MultilingualKey.Error]?.ToString() ?? "Error";
        MessageBox.Show(string.Format(messageTemplate, message.Value), caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private List<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
    {
        List<T> children = new List<T>();

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T)
            {
                children.Add((T)child);
            }

            children.AddRange(FindVisualChildren<T>(child));
        }

        return children;
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<LanguageChangedMessage>(this);
        WeakReferenceMessenger.Default.Unregister<ProjectNotFoundMessage>(this);
    }
}
