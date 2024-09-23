using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Constants;
using PIMTool.Client.Messages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PIMTool.Client.Views.Pages;
/// <summary>
/// Interaction logic for ProjectListPage.xaml
/// </summary>
public partial class ProjectListPage : Page
{
    public ProjectListPage()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, HandleLanguageChange);
        WeakReferenceMessenger.Default.Register<ProjectStatusNotSatisfiedToDeleteMessage>(this, ShowErrorDialogue);
        WeakReferenceMessenger.Default.Register<ShowDeleteConfirmationMessage>(this, ShowConfirmationMessageBox);
    }

    private void ShowErrorDialogue(object _, ProjectStatusNotSatisfiedToDeleteMessage message)
    {
        var caption = Application.Current.Resources[MultilingualKey.Error]?.ToString() ?? "Error";
        MessageBox.Show(message.ErrorMessage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void ShowConfirmationMessageBox(object _, ShowDeleteConfirmationMessage message)
    {
        var projectId = message.Value;
        var messageKey = projectId == null ? MultilingualKey.DeleteMultipleConfirmation : MultilingualKey.DeleteConfirmation;
        var messageContent = Application.Current.Resources[messageKey].ToString() ?? "Are you sure to delete project?";
        var caption = Application.Current.Resources[MultilingualKey.Warning].ToString() ?? "Warning";
        var result = MessageBox.Show(messageContent, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            if (projectId == null)
            {
                WeakReferenceMessenger.Default.Send(new DeleteMultipleConfirmedMessage(true));
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new DeleteConfirmedMessage(projectId.Value));
            }
        }
    }

    private void HandleLanguageChange(object _, LanguageChangedMessage message)
    {
        foreach (var projectItem in projectListView.Items)
        {
            var container = projectListView.ItemContainerGenerator.ContainerFromItem(projectItem) as ListViewItem;

            if (container != null)
            {
                var textBlocks = FindVisualChildren<TextBlock>(container);
                foreach (var textBlock in textBlocks)
                {
                    textBlock?.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                }

            }
        }
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
        WeakReferenceMessenger.Default.Unregister<ProjectStatusNotSatisfiedToDeleteMessage>(this);
        WeakReferenceMessenger.Default.Unregister<ShowDeleteConfirmationMessage>(this);
        WeakReferenceMessenger.Default.Send(new ShouldUnregisterMessage(true));
    }
}
