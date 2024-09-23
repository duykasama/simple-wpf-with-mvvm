﻿using PIMTool.Client.Constants;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class CreateOrUpdateProjectButtonTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isEditing)
        {
            var key = isEditing ? MultilingualKey.EditProjectButton : MultilingualKey.CreateProject;
            return Application.Current.Resources[key];
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
