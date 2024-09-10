using System.Linq;
using Avalonia.Collections;

namespace DataGridRowSelectionBinding.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        Items = new(Enumerable.Range(1, 1000)
            .Select(i => new ItemViewModel { Id = i, Name = $"Item {i}" }));
    }

    public AvaloniaList<ItemViewModel> Items { get; }
}
