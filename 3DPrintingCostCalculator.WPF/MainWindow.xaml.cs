using _3DPrintingCostCalculator.Shared;
using System.Windows;

namespace _3DPrintingCostCalculator.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ViewModel_Main)this.DataContext;

            vm.CheckBoxABS = false;
            vm.CheckBoxPLA = false;
            vm.CheckBoxTPU = false;
            vm.FilamentLengthUsed = 0.0f;
            vm.PricePerGram = 0.0f;
            vm.PrintingTimeInMinutes = 0;
            vm.CostPerHour = 30f;
            vm.MarkupInPercent = 0.1f;
        }
    }
}
