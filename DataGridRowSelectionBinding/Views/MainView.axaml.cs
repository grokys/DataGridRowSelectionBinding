using Avalonia.Controls;
using DataGridRowSelectionBinding.ViewModels;

namespace DataGridRowSelectionBinding.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
