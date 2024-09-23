using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Messages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PIMTool.Client.Views.Controls;
/// <summary>
/// Interaction logic for AppSearchBar.xaml
/// </summary>
public partial class AppSearchBar : UserControl
{
    public AppSearchBar()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, HandleLanguageChange);
    }

    private void HandleLanguageChange(object _, LanguageChangedMessage message)
    {
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

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<LanguageChangedMessage>(this);
    }
}
