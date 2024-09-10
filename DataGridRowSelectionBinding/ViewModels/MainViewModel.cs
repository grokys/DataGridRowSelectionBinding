using System.Collections.ObjectModel;
using System.Linq;

namespace DataGridRowSelectionBinding.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        Items = new(Enumerable.Range(1, 1000)
            .Select(i => new ItemViewModel { Id = i, Name = $"Item {i}" }));
    }

    public ObservableCollection<ItemViewModel> Items { get; }
}
