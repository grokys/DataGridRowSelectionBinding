using Avalonia;
using Avalonia.Controls;
using Avalonia.Reactive;

namespace DataGridRowSelectionBinding.Behaviors;

internal static class DataGridExtensions
{
    public static readonly AttachedProperty<bool> SynchronizeSelectionProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, bool>("SynchronizeSelection", typeof(DataGridExtensions));

    static DataGridExtensions()
    {
        SynchronizeSelectionProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<bool>>(OnSynchronizeSelectionChanged));
    }

    public static bool GetSynchronizeSelection(DataGrid dataGrid)
    {
        return dataGrid.GetValue(SynchronizeSelectionProperty);
    }

    public static void SetSynchronizeSelection(DataGrid dataGrid, bool value)
    {
        dataGrid.SetValue(SynchronizeSelectionProperty, value);
    }

    private static void OnSynchronizeSelectionChanged(AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.Sender is DataGrid dataGrid)
        {
            // TODO: Synchronize initial state.
            if (e.GetNewValue<bool>())
                dataGrid.SelectionChanged += OnDataGridSelectionChanged;
            else
                dataGrid.SelectionChanged -= OnDataGridSelectionChanged;
        }
    }

    private static void OnDataGridSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        foreach (var item in e.AddedItems)
        {
            if (item is ISelectable i)
                i.IsSelected = true;
        }

        foreach (var item in e.RemovedItems)
        {
            if (item is ISelectable i)
                i.IsSelected = false;
        }
    }
}
