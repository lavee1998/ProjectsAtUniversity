using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mine_Detector_WPF.Persistence;

namespace Mine_Detector_WPF.Model
{
    public class Mine_DetectorEventArgs:EventArgs
    {

        #region Fields

        private Tuple<Int32, Int32> locationofmine;
        private Players winner;

        public Mine_DetectorEventArgs(Int32 x, Int32 y, Players _winner)
        {
            Locationofmine = new Tuple<Int32, Int32>(x, y);
            Winner = _winner;
        }

        public Players Winner { get => winner; set => winner = value; }
        public Tuple<int, int> Locationofmine { get => locationofmine; set => locationofmine = value; }

        #endregion
    }
}
