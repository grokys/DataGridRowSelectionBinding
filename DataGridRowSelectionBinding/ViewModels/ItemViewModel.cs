using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DataGridRowSelectionBinding.ViewModels;

public partial class ItemViewModel : ObservableObject, ISelectable
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private bool _isSelected;
}
