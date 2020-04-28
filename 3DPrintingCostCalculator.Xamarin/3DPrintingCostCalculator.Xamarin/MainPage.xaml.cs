using _3DPrintingCostCalculator.Shared;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace _3DPrintingCostCalculator.Xamarin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnReset_Clicked(object sender, EventArgs e)
        {
            var vm = (ViewModel_Main)this.BindingContext;

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
