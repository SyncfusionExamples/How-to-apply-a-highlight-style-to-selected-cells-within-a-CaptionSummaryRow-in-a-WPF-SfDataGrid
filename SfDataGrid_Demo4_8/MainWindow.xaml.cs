using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.ScrollAxis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SfDataGrid_Demo4_8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

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
    }

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
}
