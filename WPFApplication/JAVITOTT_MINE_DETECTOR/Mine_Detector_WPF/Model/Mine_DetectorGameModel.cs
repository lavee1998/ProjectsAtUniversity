using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mine_Detector_WPF.Persistence;

namespace Mine_Detector_WPF.Model
{
    public class Mine_DetectorGameModel
    {
        #region Constructor

        public Mine_DetectorGameModel(IMine_DetectorDataAccess dataAccess) //IMine_DetectorDataAccess dataAcces
        {
            _dataAccess = dataAccess;
            _fieldsize = FieldSize.Medium;
            _tableSize = tableSizeMedium;
            _minecount = MineCountMedium;
            _table = new Mine_DetectorTable(tableSizeMedium);

            _nextplayer = Players.Player1;
            GenerateTable();
        }
        #endregion
        #region Size constans

        private const Int32 tableSizeSmall = 6;
        private const Int32 tableSizeMedium = 8;
        private const Int32 tableSizeLarge = 16;

        private const Int32 MineCountSmall = 5;
        private const Int32 MineCountMedium = 8;
        private const Int32 MineCountLarge = 12;

        #endregion


        #region Fields
        private IMine_DetectorDataAccess _dataAccess; // az adatok elérésére
        private FieldSize _fieldsize;
        private Int32 _tableSize;
        private Int32 _fieldsizetoint = 8;
        private Mine_DetectorTable _table;
        private Players _nextplayer;
        private Int32 _minecount;
        Players _winner = Players.Nobody;



        #endregion

        #region Properties

        public Players Actplayer { get => _nextplayer; set => _nextplayer = value; }
        public FieldSize Fieldsize { get => _fieldsize; set => _fieldsize = value; }
        public int Fieldsizetoint { get => _fieldsizetoint; set => _fieldsizetoint = value; }
        public Mine_DetectorTable Table { get { return _table; } }

        public int TableSize { get => _tableSize; set => _tableSize = value; }
        public int Minecount { get => _minecount; set => _minecount = value; }
        public Players Winner { get => _winner; set => _winner = value; }

        public Mine_DetectorEventArgs Mine_DetectorEventArgs
        {
            get => default(Mine_DetectorEventArgs);
            set
            {
            }
        }
        #endregion

        #region Public methods

        public event EventHandler<Mine_DetectorEventArgs> GameOver;




        public void Step(Int32 x, Int32 y)
        {
            _table.SetLocked(x, y);

            if (_table.GetValue(x, y) == 9)
            {
                _winner = _table.Actplayer;
                OnGameAdvanced(x, y, _table.Actplayer);

            }
            else
            {

                if (_table.GetValue(x, y) == 0)
                {
                    if (x > 0 && !_table.IsLocked(x - 1, y)) Step(x - 1, y);


                    if (y > 0 && !_table.IsLocked(x, y - 1)) Step(x, y - 1);

                    if (x > 0 && y > 0 && !_table.IsLocked(x - 1, y - 1)) Step(x - 1, y - 1);

                    if (x < _tableSize - 1 && !_table.IsLocked(x + 1, y)) Step(x + 1, y);

                    if (y < _tableSize - 1 && !_table.IsLocked(x, y + 1)) Step(x, y + 1);

                    if (x < _tableSize - 1 && y < _tableSize - 1 && !_table.IsLocked(x + 1, y + 1)) Step(x + 1, y + 1);

                    if (x < _tableSize - 1 && y > 0 && !_table.IsLocked(x + 1, y - 1)) Step(x + 1, y - 1);

                    if (x > 0 && y < _tableSize - 1 && !_table.IsLocked(x - 1, y + 1)) Step(x - 1, y + 1);


                }
            }
        }

        public void StepGame(Int32 x, Int32 y)
        {
            _table.Actplayer = (_table.Actplayer == Players.Player1) ? Players.Player2 : Players.Player1;
            //  _table.Actplayer = _nextplayer;


            Step(x, y);

            CheckDraw(x, y);

            Console.WriteLine((Winner == Players.Player1) ? "Player1" : "Player2 OR Nobody");
            Console.WriteLine((Winner == Players.Nobody) ? "Nobody" : "Player2 ");
        }

        public void CheckDraw(Int32 x, Int32 y)
        {
            if (_table.TableIsFull()) OnGameAdvanced(x, y, Players.Nobody);

        }

        public void OnGameAdvanced(Int32 x, Int32 y, Players player)
        {
            if (GameOver != null)
                GameOver(this, new Mine_DetectorEventArgs(x, y, player));
        }


        public void NewGame()
        {
            switch (_fieldsize)
            {
                case FieldSize.Small:
                    _tableSize = tableSizeSmall;
                    _minecount = MineCountSmall;

                    break;

                case FieldSize.Medium:
                    _tableSize = tableSizeMedium;
                    _minecount = MineCountMedium;
                    break;

                case FieldSize.Large:
                    _tableSize = tableSizeLarge;
                    _minecount = MineCountLarge;
                    break;

            }
            _table = new Mine_DetectorTable(_tableSize);
            _table.Actplayer = Players.Player1;
            _winner = Players.Nobody;

            GenerateTable();

        }

        #endregion

        #region Private methods
        public void GenerateTable()
        {
            Random _random = new Random();
            Int32 x;
            Int32 y;

            //bomba értékeinek beállítása
            for (int i = 0; i < _minecount; i++)
            {
                do
                {
                    x = _random.Next(_tableSize);
                    y = _random.Next(_tableSize);
                }

                while (!_table.IsEmpty(x, y));

                _table.SetValue(x, y, 9, false);



            }
            // _table.SetValue(0, 0, 9, false);
            //  _table.SetValue(_tableSize-1, _tableSize-1, 9, false);

            int count;
            //mezők értékeinek beállítása aszerint, hogy hány bomba van körülötte
            for (int i = 0; i < _tableSize; i++)
            {

                for (int j = 0; j < _tableSize; j++)
                {
                    count = 0;
                    if (_table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && _table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && _table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _tableSize - 1 && _table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < _tableSize - 1 && _table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && _table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _tableSize - 1 && j < _tableSize - 1 && _table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < _tableSize - 1 && _table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < _tableSize - 1 && _table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        _table.SetValue(i, j, count, false);
                    }


                }
            }
        }
        #endregion

        #region Save
        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _table);
        }
        #endregion

        #region Load

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            _table = await _dataAccess.LoadAsync(path);
            _tableSize = _table.FieldSize;

            if(_tableSize == 6)
            {
                _fieldsize = FieldSize.Small;
            }
            else if(_tableSize == 8)
            {
                _fieldsize = FieldSize.Medium;
            }
            else
            {
                _fieldsize = FieldSize.Large;
            }


        }

        #endregion
    }
}
