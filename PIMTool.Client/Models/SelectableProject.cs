using System.ComponentModel;

namespace PIMTool.Client.Models;

public class SelectableProject : Project, INotifyPropertyChanged
{
    private bool _selected = false;
    public bool IsSelected
    {
        get => _selected;
        set
        {
            if (_selected != value)
            {
                _selected = value;
            }
            PropertyChanged?.Invoke(this, new IsSelectedChangedEventArgs(nameof(IsSelected), IsSelected, Id));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public class IsSelectedChangedEventArgs : PropertyChangedEventArgs
{
    public bool IsSelected { get; set; }

    public int Id { get; set; }

    public IsSelectedChangedEventArgs(string? propertyName) : base(propertyName)
    {
    }

    public IsSelectedChangedEventArgs(string? propertyName, bool isSelected, int id) : base(propertyName)
    {
        IsSelected = isSelected;
        Id = id;
    }
}
