using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aknakereso.Persistence
{
    //public enum Players { Player1, Player2, Nobody }


    public class Mine_DetectorTable
    {
        
        #region Fields
        private Int32 _fieldSize;
        private Int32[,] _fieldValues;
        private Boolean[,] _fieldLocks;
        private Players _actplayer;
     
       
        #endregion

        #region Construktor
        public Mine_DetectorTable(Int32 fieldsize)
        {
            _fieldSize = fieldsize;
            _actplayer = Players.Player1;
            _fieldValues = new Int32[_fieldSize, _fieldSize];
            _fieldLocks = new Boolean[_fieldSize, _fieldSize];
           
            for (int i = 0; i < _fieldSize; i++)
            {
                for (int j = 0; j < _fieldSize; j++)
                {
                    _fieldValues[i, j] = 0;
                    _fieldLocks[i, j] = false;
                 
                }
            }

        }

        public Int32 this[Int32 x, Int32 y] { get { return GetValue(x, y); } }

        public Int32 GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y];
        }

        public Boolean IsEmpty(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldValues[x, y] == 0;
        }

        public Boolean IsLocked(Int32 x, Int32 y)
        {
            if (x < 0 || x >=_fieldLocks.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldLocks.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _fieldLocks[x, y];
        }
        public void SetLocked(Int32 x, Int32 y)
        {
        if (x< 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y< 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

        _fieldLocks[x, y] = true;
        }

        public Boolean TableIsFull()
        {
            for(int i = 0;i<_fieldSize;i++)
            {
                for (int j = 0; j < _fieldSize; j++)
                {
                    if (_fieldLocks[i, j] == false && _fieldValues[i, j] != 9)
                        return false;
                }
            }
            return true;
        }
         

        public void SetValue(Int32 x, Int32 y, Int32 value, Boolean islocked)
        {
            
            if (x < 0 || x >= _fieldValues.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _fieldValues.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");
            if (value < 0 || value > 9)
                throw new ArgumentOutOfRangeException("value", "The value is out of range.");

            _fieldValues[x, y] = value;
            _fieldLocks[x, y] = islocked;

        }


        #endregion


        #region Properties

        public Int32 Size { get { return _fieldValues.GetLength(0); } }

        public Players Actplayer { get => _actplayer; set => _actplayer = value; }


        #endregion
    }
}
