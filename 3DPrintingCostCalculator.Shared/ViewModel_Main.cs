using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace _3DPrintingCostCalculator.Shared
{
    public class ViewModel_Main : INotifyPropertyChanged
    {
        public enum Enum_Materials
        {
            PLA,
            TPU,
            ABS,
            PETG,
            PETT,
            HIPS
        }

        #region properties
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Enum_Materials> Materials { get; set; } = new ObservableCollection<Enum_Materials>();

        public Enum_Materials _SelectedMaterials = Enum_Materials.PLA;
        public Enum_Materials SelectedMaterials
        {
            get { return _SelectedMaterials; }
            set
            {
                Set(nameof(SelectedMaterials), ref _SelectedMaterials, value);

                ComputeAll();
            }
        }

        private double _FilamentCost = 0.0d;
        public double FilamentCost
        {
            get { return _FilamentCost; }
            set { Set(nameof(FilamentCost), ref _FilamentCost, value); ComputeAll(); }
        }

        private double _FilamentGrams = 0.0d;
        public double FilamentGrams
        {
            get { return _FilamentGrams; }
            set { Set(nameof(FilamentGrams), ref _FilamentGrams, value); ComputeAll(); }
        }

        private float _FilamentLengthUsed = 0.0f;
        public float FilamentLengthUsed
        {
            get { return _FilamentLengthUsed; }
            set { Set(nameof(FilamentLengthUsed), ref _FilamentLengthUsed, value); ComputeAll(); }
        }

        private float _PricePerGram = 0.0f;
        public float PricePerGram
        {
            get { return _PricePerGram; }
            set
            {
                Set(nameof(PricePerGram), ref _PricePerGram, value); 
                //ComputeAll();
            }
        }

        private double _TotalMaterialCost = 0.0d;
        public double TotalMaterialCost
        {
            get { return _TotalMaterialCost; }
            set { Set(nameof(TotalMaterialCost), ref _TotalMaterialCost, value); }
        }

        private int _PrintingTimeInMinutes = 0;
        public int PrintingTimeInMinutes
        {
            get { return _PrintingTimeInMinutes; }
            set { Set(nameof(PrintingTimeInMinutes), ref _PrintingTimeInMinutes, value); ComputeAll(); }
        }

        private float _CostPerHour = 30.0f;
        public float CostPerHour
        {
            get { return _CostPerHour; }
            set { Set(nameof(CostPerHour), ref _CostPerHour, value); ComputeAll(); }
        }

        private double _LaborCost = 0.0d;
        public double LaborCost
        {
            get { return _LaborCost; }
            set { Set(nameof(LaborCost), ref _LaborCost, value); }
        }

        private float _MarkupInPercent = 0.10f;
        public float MarkupInPercent
        {
            get { return _MarkupInPercent; }
            set { Set(nameof(MarkupInPercent), ref _MarkupInPercent, value); ComputeAll(); }
        }

        private double _FinalPrice = 0.0d;
        public double FinalPrice
        {
            get { return _FinalPrice; }
            set { Set(nameof(FinalPrice), ref _FinalPrice, value); }
        }

        private TimeSpan _PrintingTime = new TimeSpan();
        public TimeSpan PrintingTime
        {
            get { return _PrintingTime; }
            set { Set(nameof(PrintingTime), ref _PrintingTime, value); ComputeAll(); }
        }

        #endregion

        public ViewModel_Main()
        {
            this.Materials.Add(Enum_Materials.PLA);
            this.Materials.Add(Enum_Materials.TPU);
            this.Materials.Add(Enum_Materials.PETG);
            this.Materials.Add(Enum_Materials.ABS);
            this.Materials.Add(Enum_Materials.PETT);
            this.Materials.Add(Enum_Materials.HIPS);

            this.SelectedMaterials = this.Materials[0];
        }

        // computations are based in
        // https://www.omnicalculator.com/other/3d-printing
        void ComputeAll()
        {
            // density
            float p = 0.0f;

            // g/cm3 is based from Cura Materials
            switch(this.SelectedMaterials)
            {
                case Enum_Materials.ABS:
                    p = 1.10f;
                    break;
                case Enum_Materials.HIPS:
                    p = 1.04f;
                    break;
                case Enum_Materials.PETG:
                    p = 1.38f;
                    break;
                case Enum_Materials.PETT:
                    p = 1.45f;
                    break;
                case Enum_Materials.PLA:
                    p = 1.24f;
                    break;
                case Enum_Materials.TPU:
                    p = 1.22f;
                    break;
            }

            this.PricePerGram = (float)(this.FilamentCost / this.FilamentGrams);

            // filament diameter
            double d = 1.75d;

            double materialcost = p * 3.14159265359f * Math.Pow((d / 2d), 2) * this.FilamentLengthUsed * this.PricePerGram;

            this.TotalMaterialCost = Math.Round((float)materialcost, 2);

#if __WPF__
            double laborcost = (this.PrintingTimeInMinutes / 60.00d) * this.CostPerHour;
#else
            double laborcost = this.PrintingTime.TotalHours * this.CostPerHour;
#endif

            this.LaborCost = Math.Round((float)laborcost, 2);

            double materialpluslabor = (materialcost + laborcost);
            double markup = materialpluslabor * this.MarkupInPercent;
            this.FinalPrice = Math.Round((materialpluslabor + markup), 2);
        }

        void Set<T>(string propertyname, ref T prop, T value)
        {
            prop = value;
            NotifyPropertyChanged(propertyname);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
