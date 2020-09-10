using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using aknakereso.Model;

namespace aknakereso.Test
{
    [TestClass]
    public class Mine_DetectorGameModelTest
    {
        private Mine_DetectorGameModel _model;

        [TestInitialize]
        public void Initialize()
        {
            _model = new Mine_DetectorGameModel(null);
            // perzisztencia nélküli modellt hozunk létre
            _model.GameOver += new EventHandler<Mine_DetectorEventArgs>(Model_GameOver);
        }

      


        [TestMethod]
        public void Mine_DetectorSmallNewGameTest()
        {
            _model.Fieldsize = FieldSize.Small;
            _model.NewGame();
            Int32 minecount = 0;

            Assert.AreEqual(Players.Player1, _model.Actplayer);
            Assert.AreEqual(_model.TableSize, 6);

            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    if (9 == _model.Table[i, j]) minecount++;


                    Assert.AreEqual(false, _model.Table.IsLocked(i, j));
                }

            }

            Assert.AreEqual(_model.Minecount, minecount);
            



        }

        [TestMethod]
        public void Mine_DetectorMediumNewGameTest()
        {
            _model.Fieldsize = FieldSize.Medium;
            _model.NewGame();

            Int32 minecount = 0;

            Assert.AreEqual(Players.Player1, _model.Actplayer);
            Assert.AreEqual(_model.TableSize, 8);

            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    if (9 == _model.Table[i, j]) minecount++;


                    Assert.AreEqual(false, _model.Table.IsLocked(i, j));
                }

            }

            Assert.AreEqual(_model.Minecount, minecount);



        }

        [TestMethod]
        public void Mine_DetectorLargeNewGameTest()
        {
            _model.Fieldsize = FieldSize.Large;
            _model.NewGame();

            Int32 minecount = 0;

            Assert.AreEqual(Players.Player1, _model.Actplayer);
            Assert.AreEqual(_model.TableSize, 16);

            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    if (9 == _model.Table[i, j]) minecount++;


                    Assert.AreEqual(false, _model.Table.IsLocked(i, j));
                }

            }
            

            Assert.AreEqual(_model.Minecount, minecount);

        }


        [TestMethod]
        public void Mine_DetectorStepGameTestPlayer2Win()
        {
            _model.Fieldsize = FieldSize.Medium;
            _model.NewGame();


            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    _model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, _model.Table[i, j]);

                }

            }

            _model.Table.SetValue(0, 0, 9, false);
            _model.Table.SetValue(_model.TableSize - 1, _model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < _model.TableSize; i++)
            {

                for (int j = 0; j < _model.TableSize; j++)
                {
                    count = 0;
                    if (_model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && _model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && _model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < _model.TableSize - 1 && _model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && _model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && j < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < _model.TableSize - 1 && _model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        _model.Table.SetValue(i, j, count, false);
                    }
                }
            }

            _model.StepGame(0, 0);

            Assert.AreEqual(Players.Player2, _model.Winner);

        }

        [TestMethod]
        public void Mine_DetectorStepGameTestPlayer1Win()
        {
            _model.Fieldsize = FieldSize.Medium;
            _model.NewGame();


            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                     _model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, _model.Table[i, j]);

                }

            }

            _model.Table.SetValue(0, 0, 9, false);
            _model.Table.SetValue(_model.TableSize - 1, _model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < _model.TableSize; i++)
            {

                for (int j = 0; j < _model.TableSize; j++)
                {
                    count = 0;
                    if (_model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && _model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && _model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < _model.TableSize - 1 && _model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && _model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && j < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < _model.TableSize - 1 && _model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        _model.Table.SetValue(i, j, count, false);
                    }
                }
            }

            _model.StepGame(1, 1);
            _model.StepGame(0, 0);

            Assert.AreEqual(Players.Player1, _model.Winner);

        }

        [TestMethod]
        public void Mine_DetectorStepGameTestDraw()
        {
            _model.Fieldsize = FieldSize.Medium;
            _model.NewGame();
           

            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    _model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, _model.Table[i, j]);

                }

            }

            _model.Table.SetValue(0, 0, 9, false);
            _model.Table.SetValue(_model.TableSize-1, _model.TableSize-1, 9, false);

            Int32 count;
            for (int i = 0; i < _model.TableSize; i++)
            {

                for (int j = 0; j < _model.TableSize; j++)
                {
                    count = 0;
                    if (_model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && _model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && _model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < _model.TableSize - 1 && _model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && _model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && j < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < _model.TableSize - 1 && _model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        _model.Table.SetValue(i, j, count, false);
                    }
                }
            }
            Assert.AreNotEqual(9, _model.Table[4, 4]);
         
            _model.StepGame(4, 4);
           

            Assert.AreEqual(Players.Nobody, _model.Winner);

            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    if ((i == 0 && j == 0) || (j == _model.TableSize - 1 && i == _model.TableSize - 1))
                    {
                        Assert.AreEqual(false, _model.Table.IsLocked(i, j));
                    }
                    else
                    {
                        Assert.AreEqual(true, _model.Table.IsLocked(i, j));
                    }

                }

            }

            Assert.AreEqual(1, _model.Table[0, 1]);
            Assert.AreEqual(1, _model.Table[1, 1]);
            Assert.AreEqual(1, _model.Table[1, 0]);
            Assert.AreEqual(1, _model.Table[_model.TableSize - 2, _model.TableSize - 2]);
            Assert.AreEqual(1, _model.Table[_model.TableSize - 2, _model.TableSize - 1]);
            Assert.AreEqual(1, _model.Table[_model.TableSize - 1, _model.TableSize - 2]);
        }

        [TestMethod]
        private void Model_GameOverTest1()
        {
            _model.Fieldsize = FieldSize.Medium;
            _model.NewGame();


            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    _model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, _model.Table[i, j]);

                }

            }

            _model.Table.SetValue(0, 0, 9, false);
            _model.Table.SetValue(_model.TableSize - 1, _model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < _model.TableSize; i++)
            {

                for (int j = 0; j < _model.TableSize; j++)
                {
                    count = 0;
                    if (_model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && _model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && _model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < _model.TableSize - 1 && _model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && _model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < _model.TableSize - 1 && j < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < _model.TableSize - 1 && _model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < _model.TableSize - 1 && _model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        _model.Table.SetValue(i, j, count, false);
                    }
                }
            }

            _model.StepGame(4, 4);

        }

        private void Model_GameOver(Object sender, Mine_DetectorEventArgs e)
        {
            if(_model.Table.TableIsFull())
            {
                Console.WriteLine(_model.Table.TableIsFull());
                Assert.AreEqual(Players.Nobody, e.Winner);
                Assert.AreNotEqual(9, _model.Table[e.Locationofmine.Item1, e.Locationofmine.Item2]);
            }
            else
            {
                Assert.AreEqual(_model.Winner, e.Winner);
                Assert.AreEqual(9, _model.Table[e.Locationofmine.Item1, e.Locationofmine.Item2]);
            }
            
        }

        


    }
}
