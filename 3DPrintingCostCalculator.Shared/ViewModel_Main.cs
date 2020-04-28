using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _3DPrintingCostCalculator.Shared
{
    public class ViewModel_Main : INotifyPropertyChanged
    {
        public enum Enum_Materials
        {
            PLA,
            TPU,
            ABS,
            PETG
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

                CheckBoxABS = value == Enum_Materials.ABS ? true : false;
                CheckBoxTPU = value == Enum_Materials.TPU ? true : false;
                CheckBoxPETG = value == Enum_Materials.PETG ? true : false;
                CheckBoxPLA = value == Enum_Materials.PLA ? true : false;

                ComputeAll();
            }
        }

        public bool CheckBoxPLA { get; set; } = false;

        public bool CheckBoxABS { get; set; } = false;

        public bool CheckBoxTPU { get; set; } = false;

        private bool _CheckBoxPETG = false;
        public bool CheckBoxPETG { get; set; } = false;

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
        #endregion

        public ViewModel_Main()
        {
            this.Materials.Add(Enum_Materials.PLA);
            this.Materials.Add(Enum_Materials.TPU);
            this.Materials.Add(Enum_Materials.PETG);
            this.Materials.Add(Enum_Materials.ABS);

            this.SelectedMaterials = this.Materials[0];
        }

        void ComputeAll()
        {
            // density
            float p = 0.0f;
            // g/cm3 is based from Cura Materials
            if (CheckBoxABS) p = 1.10f;
            else if (CheckBoxPLA) p = 1.24f;
            else if (CheckBoxTPU) p = 1.22f;
            else if (CheckBoxPETG) p = 1.38f;

            this.PricePerGram = (float)(this.FilamentCost / this.FilamentGrams);

            // filament diameter
            double d = 1.75d;

            double materialcost = p * 3.14159265359f * Math.Pow((d / 2d), 2) * this.FilamentLengthUsed * this.PricePerGram;
            this.TotalMaterialCost = Math.Round((float)materialcost, 2);

            double laborcost = (this.PrintingTimeInMinutes / 60.00d) * this.CostPerHour;
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
