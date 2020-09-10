using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mine_Detector_WPF_DB.Model;
using Mine_Detector_WPF_DB.Persistence;
using System.Threading.Tasks;

namespace Mine_Detector_WPF_Test
{
    [TestClass]
    public class UnitTest1
    {
        private Mine_DetectorGameModel model;
        private Mock<IMine_DetectorDataAccess> mock;
        Mine_DetectorTable table;

        [TestInitialize]
        public void Initialize()
        {
            mock = new Mock<IMine_DetectorDataAccess>();
            table = new Mine_DetectorTable(8);

            

           

            mock.Setup(mock => mock.LoadAsync(It.IsAny<String>())).Returns(() => Task.FromResult<Mine_DetectorTable>(table));
            mock.Setup(mock => mock.SaveAsync(It.IsAny<String>(), null)).Returns(() => Task.FromResult<object>(null));
           
        model = new Mine_DetectorGameModel(mock.Object);
           // wrappedModel = new PrivateObject(model);

            // perzisztencia nélküli modellt hozunk létre
            // model.GameOver += new EventHandler<Mine_DetectorEventArgs>(Model_GameOver);
        }

        [TestMethod]
        public void TestLoadGame()
        {
            model.LoadGameAsync(String.Empty).Wait();
            mock.Verify(mock => mock.LoadAsync(It.IsAny<String>()), Times.Once());
        }
        /*
        [TestMethod]
        public void TestSaveGame()
        {
            model.SaveGameAsync(String.Empty).Wait();
            mock.Verify(mock => mock.SaveAsync(It.IsAny<String>(), null), Times.Once());
        }*/

        [TestMethod]
        public void Mine_DetectorSmallNewGameTest()
        {
            model.Fieldsize = FieldSize.Small;
            model.NewGame();
            Int32 minecount = 0;

            Assert.AreEqual(Players.Player1, model.Actplayer);
            Assert.AreEqual(model.TableSize, 6);

            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    if (9 == model.Table[i, j]) minecount++;


                    Assert.AreEqual(false, model.Table.IsLocked(i, j));
                }

            }

            Assert.AreEqual(model.Minecount, minecount);




        }

        [TestMethod]
        public void Mine_DetectorMediumNewGameTest()
        {
            model.Fieldsize = FieldSize.Medium;
            model.NewGame();

            Int32 minecount = 0;

            Assert.AreEqual(Players.Player1, model.Actplayer);
            Assert.AreEqual(model.TableSize, 8);

            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    if (9 == model.Table[i, j]) minecount++;


                    Assert.AreEqual(false, model.Table.IsLocked(i, j));
                }

            }

            Assert.AreEqual(model.Minecount, minecount);



        }

        [TestMethod]
        public void Mine_DetectorLargeNewGameTest()
        {
            model.Fieldsize = FieldSize.Large;
            model.NewGame();

            Int32 minecount = 0;

            Assert.AreEqual(Players.Player1, model.Actplayer);
            Assert.AreEqual(model.TableSize, 16);

            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    if (9 == model.Table[i, j]) minecount++;


                    Assert.AreEqual(false, model.Table.IsLocked(i, j));
                }

            }


            Assert.AreEqual(model.Minecount, minecount);

        }


        [TestMethod]
        public void Mine_DetectorStepGameTestPlayer2Win()
        {
            model.Fieldsize = FieldSize.Medium;
            model.NewGame();


            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, model.Table[i, j]);

                }

            }

            model.Table.SetValue(0, 0, 9, false);
            model.Table.SetValue(model.TableSize - 1, model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < model.TableSize; i++)
            {

                for (int j = 0; j < model.TableSize; j++)
                {
                    count = 0;
                    if (model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < model.TableSize - 1 && model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && j < model.TableSize - 1 && model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < model.TableSize - 1 && model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < model.TableSize - 1 && model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        model.Table.SetValue(i, j, count, false);
                    }
                }
            }

            model.StepGame(0, 0);

            Assert.AreEqual(Players.Player2, model.Winner);

        }

        [TestMethod]
        public void Mine_DetectorStepGameTestPlayer1Win()
        {
            model.Fieldsize = FieldSize.Medium;
            model.NewGame();


            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, model.Table[i, j]);

                }

            }

            model.Table.SetValue(0, 0, 9, false);
            model.Table.SetValue(model.TableSize - 1, model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < model.TableSize; i++)
            {

                for (int j = 0; j < model.TableSize; j++)
                {
                    count = 0;
                    if (model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < model.TableSize - 1 && model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && j < model.TableSize - 1 && model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < model.TableSize - 1 && model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < model.TableSize - 1 && model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        model.Table.SetValue(i, j, count, false);
                    }
                }
            }

            model.StepGame(1, 1);
            model.StepGame(0, 0);

            Assert.AreEqual(Players.Player1, model.Winner);

        }

        [TestMethod]
        public void Mine_DetectorStepGameTestDraw()
        {
            model.Fieldsize = FieldSize.Medium;
            model.NewGame();


            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, model.Table[i, j]);

                }

            }

            model.Table.SetValue(0, 0, 9, false);
            model.Table.SetValue(model.TableSize - 1, model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < model.TableSize; i++)
            {

                for (int j = 0; j < model.TableSize; j++)
                {
                    count = 0;
                    if (model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < model.TableSize - 1 && model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && j < model.TableSize - 1 && model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < model.TableSize - 1 && model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < model.TableSize - 1 && model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        model.Table.SetValue(i, j, count, false);
                    }
                }
            }
            Assert.AreNotEqual(9, model.Table[4, 4]);

            model.StepGame(4, 4);


            Assert.AreEqual(Players.Nobody, model.Winner);

            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    if ((i == 0 && j == 0) || (j == model.TableSize - 1 && i == model.TableSize - 1))
                    {
                        Assert.AreEqual(false, model.Table.IsLocked(i, j));
                    }
                    else
                    {
                        Assert.AreEqual(true, model.Table.IsLocked(i, j));
                    }

                }

            }

            Assert.AreEqual(1, model.Table[0, 1]);
            Assert.AreEqual(1, model.Table[1, 1]);
            Assert.AreEqual(1, model.Table[1, 0]);
            Assert.AreEqual(1, model.Table[model.TableSize - 2, model.TableSize - 2]);
            Assert.AreEqual(1, model.Table[model.TableSize - 2, model.TableSize - 1]);
            Assert.AreEqual(1, model.Table[model.TableSize - 1, model.TableSize - 2]);
        }

        [TestMethod]
        private void Model_GameOverTest1()
        {
            model.Fieldsize = FieldSize.Medium;
            model.NewGame();


            for (int i = 0; i < model.TableSize; i++)
            {
                for (int j = 0; j < model.TableSize; j++)
                {
                    model.Table.SetValue(i, j, 0, false);
                    Assert.AreEqual(0, model.Table[i, j]);

                }

            }

            model.Table.SetValue(0, 0, 9, false);
            model.Table.SetValue(model.TableSize - 1, model.TableSize - 1, 9, false);

            Int32 count;
            for (int i = 0; i < model.TableSize; i++)
            {

                for (int j = 0; j < model.TableSize; j++)
                {
                    count = 0;
                    if (model.Table.GetValue(i, j) != 9)
                    {
                        if (i > 0 && model.Table.GetValue(i - 1, j) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && model.Table.GetValue(i, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && model.Table.GetValue(i + 1, j) == 9)
                        {
                            count++;
                        }
                        if (j < model.TableSize - 1 && model.Table.GetValue(i, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j > 0 && model.Table.GetValue(i - 1, j - 1) == 9)
                        {
                            count++;
                        }
                        if (i < model.TableSize - 1 && j < model.TableSize - 1 && model.Table.GetValue(i + 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (i > 0 && j < model.TableSize - 1 && model.Table.GetValue(i - 1, j + 1) == 9)
                        {
                            count++;
                        }
                        if (j > 0 && i < model.TableSize - 1 && model.Table.GetValue(i + 1, j - 1) == 9)
                        {
                            count++;
                        }
                        model.Table.SetValue(i, j, count, false);
                    }
                }
            }

            model.StepGame(4, 4);

        }

        private void Model_GameOver(Object sender, Mine_DetectorEventArgs e)
        {
            if (model.Table.TableIsFull())
            {
                Console.WriteLine(model.Table.TableIsFull());
                Assert.AreEqual(Players.Nobody, e.Winner);
                Assert.AreNotEqual(9, model.Table[e.Locationofmine.Item1, e.Locationofmine.Item2]);
            }
            else
            {
                Assert.AreEqual(model.Winner, e.Winner);
                Assert.AreEqual(9, model.Table[e.Locationofmine.Item1, e.Locationofmine.Item2]);
            }

        }
    }
}
