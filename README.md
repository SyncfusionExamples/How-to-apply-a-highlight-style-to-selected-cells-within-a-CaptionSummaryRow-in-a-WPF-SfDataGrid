# How-to-apply-a-highlight-style-to-selected-cells-within-a-CaptionSummaryRow-in-a-WPF-SfDataGrid

In [WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid), highlighting the selected cells in the CaptionSummaryRow can be achieved using a [StyleSelector](https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.styleselector?view=netframework-4.8). Within the selector, you can retrieve the pressed [GridCellInfo](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GridCellInfo.html) by using the GetGridCellInfo method by passing the PressedRowColumnIndex, then compare its MappingName with the [CaptionSummaryCell](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GridCaptionSummaryCell.html) to apply the appropriate style to the selected cell.

**C#**
```
public class CaptionSummaryCellStyleSelector : StyleSelector
{
    SfDataGrid dataGrid = (Application.Current.MainWindow as MainWindow).dataGrid;
    GridCellInfo cell = null;

    public override Style SelectStyle(object item, DependencyObject container)
    {
        if ((dataGrid.SelectionController as GridCellSelectionController) != null)
        {
            var pressedRowColumnIndex = (dataGrid.SelectionController as GridCellSelectionController).GetType().GetProperty("PressedRowColumnIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((dataGrid.SelectionController as GridCellSelectionController));

            if (pressedRowColumnIndex != null)
                cell = dataGrid.GetGridCellInfo((RowColumnIndex)pressedRowColumnIndex);
            var nodeEntry = cell.GetType().GetProperty("NodeEntry", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cell);

            if (nodeEntry != null && item == nodeEntry && (cell as GridCellInfo).Column.MappingName == (container as GridCaptionSummaryCell).ColumnBase.GridColumn.MappingName)
                return Application.Current.Resources["SelectedCaptionSummaryCellStyle"] as Style;
        }
        return base.SelectStyle(item, container);
    }
}
```

To update the style at runtime, use the UpdateDataRow method to refresh the CaptionSummaryRow in the [SelectionChanged](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html#Syncfusion_UI_Xaml_Grid_SfDataGrid_SelectionChanged) event when the cell is selected.

**C#**
```
 private void DataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
 {
     if (e.AddedItems != null)
     {
         foreach (var item in e.AddedItems)
         {
             GridCellInfo gridCellInfo = item as GridCellInfo;
             if (gridCellInfo == null)
                 continue;
             var nodeEntry = gridCellInfo.GetType().GetProperty("NodeEntry", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(gridCellInfo);
             if (nodeEntry != null && nodeEntry is Group group)
             {
                 var index = dataGrid.ResolveStartIndexOfGroup(group);
                 this.dataGrid.UpdateDataRow(index);
             }
         }
     }

     if (e.RemovedItems != null)
     {
         foreach (var item in e.RemovedItems)
         {
             GridCellInfo gridCellInfo = item as GridCellInfo;
             if (gridCellInfo == null)
                 continue;
             var nodeEntry = gridCellInfo.GetType().GetProperty("NodeEntry", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(gridCellInfo);
             if (nodeEntry != null && nodeEntry is Group group)
             {
                 var index = dataGrid.ResolveStartIndexOfGroup(group);
                 this.dataGrid.UpdateDataRow(index);
             }
         }
     }
 }
```

**Note**: Multiple cell selection within the same CaptionSummaryRow is not feasible because the CaptionSummaryRow is treated internally as a single [GridCellInfo](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GridCellInfo.html). Therefore, when multiple selection is enabled, selecting one [CaptionSummaryCell](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GridCaptionSummaryCell.html) and then clicking another within the same row will clear the selection. So, it cannot be achieved.

![Highlight Style For CaptionSummaryCell](Highlight%20Style%20For%20CaptionSummaryCell.gif)

Take a moment to peruse the [WPF DataGrid - Summaries](https://help.syncfusion.com/wpf/datagrid/summaries) documentation, to learn more about summaries with examples.