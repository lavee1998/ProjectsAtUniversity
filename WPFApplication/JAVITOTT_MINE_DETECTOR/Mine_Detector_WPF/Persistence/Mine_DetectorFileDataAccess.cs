using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mine_Detector_WPF.Persistence
{
    public class Mine_DetectorFileDataAccess : IMine_DetectorDataAccess
    {


        public async Task<Mine_DetectorTable> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
                {
                    String line = await reader.ReadLineAsync();
                    String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
                    Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla méretét
                    if(tableSize == 6)
                    {
                        
                    }
                    Players actplayer = Int32.Parse(numbers[1]) == 1 ? Players.Player1 : Players.Player2; // beolvassuk a házak méretét

                    Mine_DetectorTable table = new Mine_DetectorTable(tableSize); // létrehozzuk a táblát
                    table.Actplayer = actplayer;

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table.SetValue(i, j, Int32.Parse(numbers[j]), false);
                        }
                    }

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        String[] locks = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            if (locks[j] == "1")
                            {
                                table.SetLocked(i, j);
                            }
                        }
                    }

                    return table;
                }
            }
            catch
            {
                throw new Mine_DetectorDataException();
            }
        }



        public async Task SaveAsync(String path, Mine_DetectorTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.Write(table.Size + " "); // kiírjuk a méreteket
                    writer.Write(((table.Actplayer == Players.Player1) ? "1" : "2") + " ");



                    await writer.WriteLineAsync();

                    for (Int32 i = 0; i < table.Size; i++)
                    {
                        for (Int32 j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync(table[i, j] + " "); // kiírjuk az értékeket
                        }
                        await writer.WriteLineAsync();
                    }

                    for (Int32 i = 0; i < table.Size; i++)
                    {
                        for (Int32 j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync((table.IsLocked(i, j) ? "1" : "0") + " "); // kiírjuk a zárolásokat
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new Mine_DetectorDataException();
            }
        }
    }
}
