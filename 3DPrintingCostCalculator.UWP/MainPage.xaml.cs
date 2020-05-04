using _3DPrintingCostCalculator.Shared;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _3DPrintingCostCalculator.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ViewModel_Main)this.DataContext;

            vm.FilamentLengthUsed = 0.0f;
            vm.PricePerGram = 0.0f;
            vm.PrintingTimeInMinutes = 0;
            vm.CostPerHour = 30f;
            vm.MarkupInPercent = 0.1f;
        }
    }
}
