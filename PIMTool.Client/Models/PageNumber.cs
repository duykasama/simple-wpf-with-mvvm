using CommunityToolkit.Mvvm.ComponentModel;

namespace PIMTool.Client.Models;

public class PageNumber : ObservableObject
{
    private int _pageNumber;
    public int Number
    {
        get => _pageNumber;
        set => SetProperty(ref _pageNumber, value);
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}
