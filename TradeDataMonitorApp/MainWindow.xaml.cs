using System.Configuration;
using System.Windows;
using TradeDataMonitorApp.ViewModels;
using TradeDataMonitoring;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ViewModelBase viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
