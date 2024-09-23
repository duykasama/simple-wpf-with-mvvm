using PIMTool.Client.Constants;
using PIMTool.Client.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class CreateProjectProjectStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ProjectStatus enumValue)
        {
            switch (enumValue)
            {
                case ProjectStatus.NEW:
                    return Application.Current.Resources[MultilingualKey.New];
                case ProjectStatus.PLA:
                    return Application.Current.Resources[MultilingualKey.Planned];
                case ProjectStatus.INP:
                    return Application.Current.Resources[MultilingualKey.InProgress];
                case ProjectStatus.FIN:
                    return Application.Current.Resources[MultilingualKey.Finished];
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            switch (stringValue)
            {
                case "New":
                case "NEW":
                case "Nouveau":
                    return ProjectStatus.NEW;
                case "Planned":
                case "PLANNED":
                case "Prévue":
                    return ProjectStatus.PLA;
                case "In Progress":
                case "IN PROGRESS":
                case "En cours":
                    return ProjectStatus.INP;
                case "Finished":
                case "FINISHED":
                case "Finie":
                    return ProjectStatus.FIN;
            }
        }

        return value;
    }
}
