using System;
using System.Collections.Generic;
using System.Linq;
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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using PropertyChanged;
using ViewModel;

namespace EzHRMApp.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class DashboardView : UserControl
    {
        public SeriesCollection ChartCollections { get; set; }

        public DashboardView()
        {
            InitializeComponent();
        }

        public void UpdatePieChart(object sender, RoutedEventArgs e)
        {
            checkInPieChart.Series[0].Values = new ChartValues<int>(new int[] { DashboardViewModel.Instance.BeingLateSum });
            checkInPieChart.Series[1].Values = new ChartValues<int>(new int[] { DashboardViewModel.Instance.BeingOnTimeSum });
            checkInPieChart.Series[2].Values = new ChartValues<int>(new int[] { DashboardViewModel.Instance.BeingEarlySum });

            checkOutPieChart.Series[0].Values = new ChartValues<int>(new int[] { DashboardViewModel.Instance.CheckOutEarly });
            checkOutPieChart.Series[1].Values = new ChartValues<int>(new int[] { DashboardViewModel.Instance.CheckOutOnTime });
            checkOutPieChart.Series[2].Values = new ChartValues<int>(new int[] { DashboardViewModel.Instance.WorkOverTimeSum });
        }
    }
}
