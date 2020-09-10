using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aknakereso.Persistence;
using aknakereso.Model;

namespace aknakereso
{
    public enum Players { Player1, Player2, Nobody }
    public partial class GameForm : Form
    {

        #region Fields
        private IMine_DetectorDataAccess _dataAccess;
        private Mine_DetectorGameModel _model;
        private Button[,] _buttonGrid;
        #endregion


        public GameForm()
        {
            InitializeComponent();
          
        }

        private void GameFormLoad(object sender, EventArgs e)
        {
            _dataAccess = new Mine_DetectorFileDataAccess();
            _model = new Mine_DetectorGameModel(_dataAccess);
            _model.GameOver += new EventHandler<Mine_DetectorEventArgs>(View_GameOver);
            GenerateTable();
            SetupTable();
        }

        private void NewGame()
        {
            _model.NewGame();
            GenerateTable();
            SetupTable();
        }

        private void View_GameOver(Object sender , Mine_DetectorEventArgs e)
        {
            SetupTable();
            
            foreach (Button button in _buttonGrid) // kikapcsoljuk a gombokat
                button.Enabled = false;

            if (e.Winner == Players.Player1) // győzelemtől függő üzenet megjelenítése
            {
               // _buttonGrid[e.Locationofmine.Item1, e.Locationofmine.Item2].BackColor = Color.Red;
                MessageBox.Show("Gratulálok, az első játékos győzött!"  ,  "Aknakereső játék",
                              
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else if(e.Winner == Players.Player2)
            {
               // _buttonGrid[e.Locationofmine.Item1, e.Locationofmine.Item2].BackColor = Color.Red;
                MessageBox.Show("Gratulálok, a második játékos győzött!", "Aknakereső játék" ,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Gratulálok, Döntetlen", "Aknakereső játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }

            NewGame();
        }


        private void GenerateTable()
        {
           if(_buttonGrid != null)
            {
                foreach (Button button in _buttonGrid)
                 {
                       Controls.Remove(button);
                 }
            }
     
             _buttonGrid = new Button[_model.TableSize, _model.TableSize];
           
            this.Size = new Size(5 + _model.TableSize * 50 + _model.TableSize + 25, _model.TableSize * 50 + _model.TableSize + 25 + 40);
            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                   
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j + j, 25 + 50 * i + i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betűtípus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100+ i * _model.Table.Size + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].FlatAppearance.BorderColor = Color.Black;
                    _buttonGrid[i, j].FlatAppearance.BorderSize = 2;
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    Controls.Add(_buttonGrid[i, j]);
                  
                }
            }
        }

        private void ButtonGrid_MouseClick(Object sender , MouseEventArgs e)
        {
            Console.WriteLine(_model.Table.Size);

            Int32 x = ((sender as Button).TabIndex - 100) / _model.Table.Size;
            Int32 y = ((sender as Button).TabIndex - 100) % _model.Table.Size;

            //Console.WriteLine(x);
           // Console.WriteLine(y);

            _model.StepGame(x, y);
            SetupTable();
        }

        private void SetupTable()
        {
            for (Int32 i = 0; i < _buttonGrid.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _buttonGrid.GetLength(1); j++)
                {
                    if (!_model.Table.IsLocked(i, j)) // ha nincs zárolva a mező
                    {
                        _buttonGrid[i, j].Text = String.Empty;
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].BackColor = Color.Black;
                        
                    }
                    else // ha zárolva van
                    {
                        if (_model.Table[i, j] == 9)
                        {
                            _buttonGrid[i, j].BackColor = Color.Red;
                            _buttonGrid[i, j].Text = String.Empty;
                        }
                        else if(_model.Table[i,j]==0)
                        {
                            _buttonGrid[i, j].Text = String.Empty;
                            _buttonGrid[i, j].BackColor = Color.DarkBlue;
                            _buttonGrid[i, j].Enabled = false; // gomb kikapcsolása
                        }
                        else  
                        {
                            _buttonGrid[i, j].BackColor = Color.Azure;
                            _buttonGrid[i, j].Text = _model.Table[i, j].ToString();
                            _buttonGrid[i, j].Enabled = false; // gomb kikapcsolása
                        }                           
                    }
                }
            }
        }

        private void MenuGameSmall_Click(Object sender, EventArgs e)
        {
            _model.Fieldsize = FieldSize.Small;
            NewGame();

        }

        private void MenuGameMedium_Click(Object sender, EventArgs e)
        {
            _model.Fieldsize = FieldSize.Medium;
            NewGame();
        }

        private void MenuGameLarge_Click(Object sender, EventArgs e)
        {
            _model.Fieldsize = FieldSize.Large;
            NewGame();
        }

        private void MenuNewGame_Click(Object sender, EventArgs e)
        {
            NewGame();
          
        }

        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {
           
          

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                }
                catch (Mine_DetectorDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                     await _model.LoadGameAsync(openFileDialog1.FileName);
                    GenerateTable();
                    SetupTable();
                    

                }
                catch (Mine_DetectorDataException)
                {

                     MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }

        
    }
}



