# Synchronizing DataGrid Row Selection with the ViewModel

This repository contains an example using a XAML behavior to synchronize the selection of rows in a DataGrid with a ViewModel in WPF.

It defines a [`DataGridExtensions.SynchronizeSelection`](DataGridRowSelectionBinding/Behaviors/DataGridExtensions.axaml) attached property that can be set on a `DataGrid`:

```xml
<DataGrid ItemsSource="{Binding Items}"
          b:DataGridExtensions.SynchronizeSelection="{Binding SelectedItem}"/>
```

(see [MainView.axaml](DataGridRowSelectionBinding/Views/MainView.axaml))

When set, the attached property will synchronize the selection of rows in the `DataGrid` with each bound row model. The behavior assumes that the model implements `IsSelectable` and hence has a boolean `IsSelected` property.