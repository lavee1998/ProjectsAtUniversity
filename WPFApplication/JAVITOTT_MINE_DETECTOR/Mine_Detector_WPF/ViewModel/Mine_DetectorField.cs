using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mine_Detector_WPF.ViewModel
{
    public class Mine_DetectorField : ViewModelBase
    {
        private Boolean _isLocked;

        private String _text;


        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sor lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Oszlop lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        public Int32 Number { get; set; }

        /// <summary>
        /// Színérték lekérdezése, vagy beállítása.
        /// </summary>
        Color color;
        public Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Mezőváltoztató parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand StepCommand { get; set; }

        public Mine_DetectorViewModel Mine_DetectorViewModel
        {
            get => default(Mine_DetectorViewModel);
            set
            {
            }
        }
    }
}
