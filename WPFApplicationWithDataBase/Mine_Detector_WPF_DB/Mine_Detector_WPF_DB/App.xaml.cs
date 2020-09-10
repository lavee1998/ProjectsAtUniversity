using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Mine_Detector_WPF_DB.Model;
using Mine_Detector_WPF_DB.ViewModel;
using Mine_Detector_WPF_DB.Persistence;
using Mine_Detector_WPF_DB.View;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Threading;

namespace Mine_Detector_WPF_DB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private Mine_DetectorGameModel _model;
        private Mine_DetectorViewModel _viewModel;
        private MainWindow _view;
        
        private LoadWindow _loadWindow;
        private SaveWindow _saveWindow;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // modell létrehozása
            //   _model = new SudokuGameModel(new SudokuFileDataAccess());
            IMine_DetectorDataAccess dataAccess;
            dataAccess = new Mine_DetectorDbDataAccess("name = GameModel");
            _model = new Mine_DetectorGameModel(dataAccess);

           

            _model.GameOver += new EventHandler<Mine_DetectorEventArgs>(ViewModel_GameOver);

            _model.NewGame();


            // nézemodell létrehozása
            _viewModel = new Mine_DetectorViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            // _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGameOpen += new EventHandler(ViewModel_LoadGameOpen);
            _viewModel.LoadGameClose += new EventHandler<String>(ViewModel_LoadGameClose);
            _viewModel.SaveGameOpen += new EventHandler(ViewModel_SaveGameOpen);
            _viewModel.SaveGameClose += new EventHandler<String>(ViewModel_SaveGameClose);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            _view.Show();


        }



        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>


        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            _model.NewGame();
            _viewModel.TableSize = _model.Table.FieldSize;
            _viewModel.OnPropertyChanged("TableSize");
            _viewModel.GenerateTable();
            _viewModel.RefreshTable();


        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object sender, System.EventArgs e)
        {




            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Aknakereső tábla betöltése";
                openFileDialog.Filter = "Aknakereső tábla|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);

                    _viewModel.TableSize = _model.Table.FieldSize;
                    _viewModel.OnPropertyChanged("TableSize");
                    _viewModel.GenerateTable();
                    _viewModel.RefreshTable();




                }
            }
            catch (Mine_DetectorDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Aknakereső", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {


            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Aknakereső tábla betöltése";
                saveFileDialog.Filter = "Aknakereső tábla|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (Mine_DetectorDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Sudoku", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }


        private void ViewModel_GameOver(Object sender, Mine_DetectorEventArgs e)
        {
            _viewModel.RefreshTable();
            _viewModel.Model_GameOver(this, new Mine_DetectorEventArgs(e.Locationofmine.Item1, e.Locationofmine.Item2, e.Winner));


            if (e.Winner == Players.Player1) // győzelemtől függő üzenet megjelenítése
            {
                // _buttonGrid[e.Locationofmine.Item1, e.Locationofmine.Item2].BackColor = Color.Red;
                MessageBox.Show("Gratulálok, az első játékos győzött!", "Aknakereső játék",

                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else if (e.Winner == Players.Player2)
            {
                // _buttonGrid[e.Locationofmine.Item1, e.Locationofmine.Item2].BackColor = Color.Red;
                MessageBox.Show("Gratulálok, a második játékos győzött!", "Aknakereső játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Gratulálok, Döntetlen", "Aknakereső játék",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }

            ViewModel_NewGame(this, null);

        }

        private void ViewModel_LoadGameOpen(object sender, System.EventArgs e)
        {
            
            _viewModel.SelectedGame = null; // kezdetben nincsen kiválasztott elem

            _loadWindow = new LoadWindow(); // létrehozzuk a játék állapot betöltő ablakot
            _loadWindow.DataContext = _viewModel;
            _loadWindow.ShowDialog(); // megjelenítjük dialógusként

        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGameClose(object sender, String name)
        {
            if (name != null)
            {
                try
                {
                    await _model.LoadGameAsync(name);
                    _viewModel.TableSize = _model.Table.FieldSize;
                    _viewModel.OnPropertyChanged("TableSize");
                    _viewModel.GenerateTable();
                    _viewModel.RefreshTable();
                }
                catch
                {
                    MessageBox.Show("Játék betöltése sikertelen!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _loadWindow.Close(); // játékállapot betöltőtő ablak bezárása
        }

        /// <summary>
        /// Játék mentés választó eseménykezelője.
        /// </summary>
        private void ViewModel_SaveGameOpen(object sender, EventArgs e)
        {
        

           

            _viewModel.SelectedGame = null; // kezdetben nincsen kiválasztott elem
            _viewModel.NewName = String.Empty;

            _saveWindow = new SaveWindow(); // létrehozzuk a játék állapot mentő ablakot
            _saveWindow.DataContext = _viewModel;
            _saveWindow.ShowDialog(); // megjelenítjük dialógusként

      
        }

      
        private async void ViewModel_SaveGameClose(object sender, String name)
        {
            if (name != null)
            {
                try
                {
                    // felülírás ellenőrzése
                    var games = await _model.ListGamesAsync();
                    if (games.All(g => g.Name != name) ||
                        MessageBox.Show("Biztos, hogy felülírja a meglévő mentést?", "Sudoku",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _model.SaveGameAsync(name);
                    }
                }
                catch
                {
                    MessageBox.Show("Játék mentése sikertelen!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _saveWindow.Close(); // játékállapot mentő ablak bezárása
        }

        #endregion
    }
}
