using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Reactive;

namespace DataGridRowSelectionBinding.Behaviors;

internal static class DataGridExtensions
{
    public static readonly AttachedProperty<bool> SynchronizeSelectionProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, bool>("SynchronizeSelection", typeof(DataGridExtensions));

    private static readonly ConditionalWeakTable<DataGrid, IDisposable> _dispose = [];

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
            if (e.GetNewValue<bool>())
                Subscribe(dataGrid);
            else
                Unsubscribe(dataGrid);
        }
    }

    private static void Subscribe(DataGrid dataGrid)
    {
        dataGrid.PropertyChanged += OnDataGridPropertyChanged;
        dataGrid.SelectionChanged += OnDataGridSelectionChanged;

        if (dataGrid.ItemsSource is IList list)
        {
            // Set the initial selection state.
            foreach (var item in list)
            {
                if (item is ISelectable i)
                    i.IsSelected = dataGrid.SelectedItems.Contains(item);
            }

            // Listen for property changes on the collection.
            if (list is IAvaloniaReadOnlyList<ISelectable> avaloniaList)
            {
                var disposable = avaloniaList.TrackItemPropertyChanged(x => OnItemPropertyChanged(dataGrid, x.Item1, x.Item2));
                _dispose.Add(dataGrid, disposable);
            }
        }
    }

    private static void Unsubscribe(DataGrid dataGrid)
    {
        dataGrid.PropertyChanged -= OnDataGridPropertyChanged;
        dataGrid.SelectionChanged -= OnDataGridSelectionChanged;

        if (_dispose.TryGetValue(dataGrid, out var disposable))
        {
            disposable.Dispose();
            _dispose.Remove(dataGrid);
        }
    }

    private static void OnDataGridPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == DataGrid.ItemsSourceProperty && sender is DataGrid dataGrid)
        {
            // When the ItemsSource changes, reset and re-subscribe.
            Unsubscribe(dataGrid);
            Subscribe(dataGrid);
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

    private static void OnItemPropertyChanged(DataGrid dataGrid, object? item, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ISelectable.IsSelected) && item is ISelectable i)
        {
            if (i.IsSelected)
                dataGrid.SelectedItems.Add(item);
            else
                dataGrid.SelectedItems.Remove(item);
        }
    }
}
