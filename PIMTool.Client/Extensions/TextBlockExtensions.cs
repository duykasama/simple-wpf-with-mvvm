using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PIMTool.Client.Extensions;

internal static class TextBlockExtensions
{
    #region Command

    public static readonly DependencyProperty CommandProperty =
                DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(TextBlockExtensions),
                    new PropertyMetadata(null, OnCommandChanged));
    public static ICommand GetCommand(TextBlock textBlock)
    {
        return (ICommand)textBlock.GetValue(CommandProperty);
    }

    public static void SetCommand(TextBlock textBlock, ICommand value)
    {
        textBlock.SetValue(CommandProperty, value);
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBlock textBlock)
        {
            textBlock.MouseLeftButtonUp -= TextBlock_MouseLeftButtonUp;
            if (e.NewValue != null)
            {
                textBlock.MouseLeftButtonUp += TextBlock_MouseLeftButtonUp;
            }
        }
    }

    private static void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        var textBlock = sender as TextBlock;
        var command = GetCommand(textBlock);
        var commandParameter = GetCommandParameter(textBlock);

        if (command != null && command.CanExecute(commandParameter))
        {
            command.Execute(commandParameter);
        }
    }

    #endregion

    #region CommandParameter

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(TextBlockExtensions), null);

    public static object GetCommandParameter(TextBlock textBlock)
    {
        return textBlock.GetValue(CommandParameterProperty);
    }

    public static void SetCommandParameter(TextBlock textBlock, object value)
    {
        textBlock.SetValue(CommandParameterProperty, value);
    }

    #endregion

    #region IsActive

    public static readonly DependencyProperty IsActiveProperty =
                DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(TextBlockExtensions),
                    new PropertyMetadata(false));

    public static bool GetIsActive(TextBlock textBlock)
    {
        return (bool)textBlock.GetValue(IsActiveProperty);
    }

    public static void SetIsActive(TextBlock textBlock, bool value)
    {
        textBlock.SetValue(IsActiveProperty, value);
    }

    #endregion
}
