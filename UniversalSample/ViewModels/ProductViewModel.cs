using System;
using System.ComponentModel;

namespace UniversalSample.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private string modelNumber;
        public string ModelNumber
        {
            get { return modelNumber; }
            set
            {
                Valid.Required(value);
                modelNumber = value;
                C("ModelNumber");
            }
        }

        private string modelName;
        public string ModelName
        {
            get { return modelName; }
            set
            {
                Valid.Required(value);
                modelName = value;
                C("ModelName");
            }
        }

        private double unitCost;
        public double UnitCost
        {
            get { return unitCost; }
            set
            {
                if (value < 0) throw new ArgumentException("Can't be less than 0.");
                unitCost = value;
                C("UnitCost");
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                Valid.Required(value);
                description = value;
                C("Description");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void C(string s)
        {
            var e = new PropertyChangedEventArgs(s);
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }


    }
}
