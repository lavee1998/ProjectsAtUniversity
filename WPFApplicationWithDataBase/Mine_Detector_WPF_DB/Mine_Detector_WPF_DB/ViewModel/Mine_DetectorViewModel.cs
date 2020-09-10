using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mine_Detector_WPF_DB.Model;
using Mine_Detector_WPF_DB.Persistence;
using Mine_Detector_WPF_DB;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Mine_Detector_WPF_DB.ViewModel
{
    public class Mine_DetectorViewModel : ViewModelBase
    {
        #region Fields

        private Mine_DetectorGameModel _model; // modell

        private Int32 _tableSize;

        public ObservableCollection<SaveEntry> Games { get; set; }

        private SaveEntry _selectedGame;

        private String _newName = String.Empty;

        public Int32 TableSize
        {
            get { return _tableSize; }
            set
            {
                if (_tableSize != value)
                {
                    _tableSize = value;
                    OnPropertyChanged("TableSize");
                }
            }
        }

        #endregion

        #region Properties

       
        public DelegateCommand NewGameCommand { get; private set; }

      
        public DelegateCommand LoadGameCommand { get; private set; }

      
        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand LoadGameOpenCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCloseCommand { get; private set; }

        /// <summary>
        /// Játék mentés választó parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameOpenCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCloseCommand { get; private set; }




        public ObservableCollection<Mine_DetectorField> Fields { get; set; }

        public SaveEntry SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                if (_selectedGame != null)
                    NewName = String.Copy(_selectedGame.Name);

                OnPropertyChanged();
                LoadGameCloseCommand.RaiseCanExecuteChanged();
                SaveGameCloseCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Új játék mentés nevének lekérdezése.
        /// </summary>
        public String NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;

                OnPropertyChanged();
                SaveGameCloseCommand.RaiseCanExecuteChanged();
            }
        }



        public Boolean IsGameSmall
        {
            get { return _model.Fieldsize == FieldSize.Small; }
            set
            {
                Console.WriteLine("hello");
                _model.Fieldsize = FieldSize.Small;
                NewGame(this, null);
                _tableSize = _model.TableSize;
                OnPropertyChanged("TableSize");
                OnPropertyChanged("IsGameSmall");
                OnPropertyChanged("IsGameMedium");
                OnPropertyChanged("IsGameLarge");
            }
        }

       
        public Boolean IsGameMedium
        {
            get { return _model.Fieldsize == FieldSize.Medium; }
            set
            {
              

                _model.Fieldsize = FieldSize.Medium;
                NewGame(this, null);
                _tableSize = _model.TableSize;

                OnPropertyChanged("TableSize");

                OnPropertyChanged("IsGameSmall");
                OnPropertyChanged("IsGameMedium");
                OnPropertyChanged("IsGameLarge");
            }
        }

      
        public Boolean IsGameLarge
        {
            get { return _model.Fieldsize == FieldSize.Large; }
            set
            {
                

                _model.Fieldsize = FieldSize.Large;
                
                NewGame(this, null);
                _tableSize = _model.TableSize;
                OnPropertyChanged("TableSize");
                OnPropertyChanged("IsGameSmall");
                OnPropertyChanged("IsGameMedium");
                OnPropertyChanged("IsGameLarge");
            }
        }

        #endregion

        #region Events

       
        public event EventHandler NewGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;

        public event EventHandler LoadGameOpen;

        public event EventHandler<String> LoadGameClose; 
        public event EventHandler SaveGameOpen;
        public event EventHandler<String> SaveGameClose;

        #endregion

        #region Constructors

        public Mine_DetectorViewModel(Mine_DetectorGameModel model)
        {
            _model = model;
            _tableSize = _model.TableSize;
            _model.GameOver += new EventHandler<Mine_DetectorEventArgs>(Model_GameOver);
           
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());

            LoadGameOpenCommand = new DelegateCommand(async param =>
            {
                Games = new ObservableCollection<SaveEntry>(await _model.ListGamesAsync());
                OnLoadGameOpen();
            });
            LoadGameCloseCommand = new DelegateCommand(
                param => SelectedGame != null, // parancs végrehajthatóságának feltétele
                param => { OnLoadGameClose(SelectedGame.Name); });
            SaveGameOpenCommand = new DelegateCommand(async param =>
            {
                Games = new ObservableCollection<SaveEntry>(await _model.ListGamesAsync());
                OnSaveGameOpen();
            });
            SaveGameCloseCommand = new DelegateCommand(
                param => NewName.Length > 0, // parancs végrehajthatóságának feltétele
                param => { OnSaveGameClose(NewName); });

            Fields = new ObservableCollection<Mine_DetectorField>();

            GenerateTable();
            RefreshTable();
        }

        #endregion

        #region Private methods

     
        
       public void GenerateTable()
       {
            Fields.Clear();
            
            for (Int32 i = 0; i < _model.Table.FieldSize; i++) // inicializáljuk a mezőket
            {
               
                for (Int32 j = 0; j < _model.Table.FieldSize; j++)
                {
                    Fields.Add(new Mine_DetectorField
                    {
                        IsLocked = true,
                        Color = Colors.Yellow,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))

                        
                    });
                    Console.WriteLine(i + " " + j);
                }
            }
        }

       public void RefreshTable()
       {
            foreach (Mine_DetectorField field in Fields) // inicializálni kell a mezőket is
            {
                
                
                if (!_model.Table.IsLocked(field.X, field.Y)) // ha nincs zárolva a mező
                {
                    field.Color = Colors.Black; //fekete
                    field.IsLocked = true;
                    field.Text = String.Empty;

    

                }
                else // ha zárolva van
                {
                    field.IsLocked = false;

                    if (_model.Table[field.X, field.Y] == 9)
                    {
                        field.Color = Colors.Red; //piros
                        field.Text = String.Empty;
                    }
                    else if (_model.Table[field.X, field.Y] == 0)
                    {
                        field.Color = Colors.DarkBlue; //DarkBlue
                        field.Text = String.Empty;
                        
                    }
                    else
                    {
                        field.Color = Colors.LightGreen; //Azure
                        field.Text = _model.Table[field.X, field.Y].ToString();
                      
              
                    }
                }
               
      
                Console.WriteLine(field.Text);
                Console.WriteLine(field.Color);
                Console.WriteLine(field.IsLocked);
            }
            OnPropertyChanged("Fields");



        }

   
        private void StepGame(Int32 index)
        {
            Mine_DetectorField field = Fields[index];

            _model.StepGame(field.X, field.Y);

            RefreshTable();

           
        }

        #endregion

        #region Game event handlers

       public void Model_GameOver(object sender, Mine_DetectorEventArgs e)
        {
            foreach (Mine_DetectorField field in Fields)
            {
                field.IsLocked = true; // minden mezőt lezárunk
            }
        }

      
        private void Model_GameAdvanced(object sender, Mine_DetectorEventArgs e)
        {
            OnPropertyChanged("GameTime");
        }

        private void Model_GameCreated(object sender, Mine_DetectorEventArgs e)
        {
            RefreshTable();
        }

        #endregion

        #region Event methods

        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        private void OnLoadGameOpen()
        {
            if (LoadGameOpen != null)
                LoadGameOpen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltésének eseménykiváltása.
        /// </summary>
        private void OnLoadGameClose(String name)
        {
            if (LoadGameClose != null)
                LoadGameClose(this, name);
        }

        /// <summary>
        /// Játék mentés választásának eseménykiváltása.
        /// </summary>
        private void OnSaveGameOpen()
        {
            if (SaveGameOpen != null)
                SaveGameOpen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentésének eseménykiváltása.
        /// </summary>
        private void OnSaveGameClose(String name)
        {
            if (SaveGameClose != null)
                SaveGameClose(this, name);
        }


        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
            
            Console.WriteLine("Betöltöttem!");
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }


      

        #endregion
    }
}
