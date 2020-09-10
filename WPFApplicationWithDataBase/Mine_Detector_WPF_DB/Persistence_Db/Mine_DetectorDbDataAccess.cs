using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Mine_Detector_WPF_DB.Persistence
{
    /// <summary>
    /// Mine_Detector perzisztencia adatbáziskezelő típusa.
    /// </summary>
	public class Mine_DetectorDbDataAccess : IMine_DetectorDataAccess
    {
        private GameContext _context;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="connection">Adatbázis connection string.</param>
        public Mine_DetectorDbDataAccess(String connection)
        {
            _context = new GameContext(connection);
            _context.Database.CreateIfNotExists(); // adatbázis séma létrehozása, ha nem létezik
        }

        

        /// <summary>
        /// Játékállapot betöltése.
        /// </summary>
        /// <param name="name">Név vagy elérési útvonal.</param>
        /// <returns>A beolvasott játéktábla.</returns>
        public async Task<Mine_DetectorTable> LoadAsync(String name)
        {
            try
            {
                Game game = await _context.Games
                    .Include(g => g.Fields)
                    .SingleAsync(g => g.Name == name); // játék állapot lekérdezése
                Mine_DetectorTable table = new Mine_DetectorTable(game.TableSize); // játéktábla modell létrehozása

                foreach (Field field in game.Fields) // mentett mezők feldolgozása
                {
                    table.SetValue(field.X, field.Y, field.Number, field.IsLocked);
                }

                return table;
            }
            catch
            {
                throw new Mine_DetectorDataException();
            }
        }

        /// <summary>
        /// Játékállapot mentése.
        /// </summary>
        /// <param name="name">Név vagy elérési útvonal.</param>
        /// <param name="table">A kiírandó játéktábla.</param>
        public async Task SaveAsync(String name, Mine_DetectorTable table)
        {
            try
            {
                // játékmentés keresése azonos névvel
                Game overwriteGame = await _context.Games
                    .Include(g => g.Fields)
                    .SingleOrDefaultAsync(g => g.Name == name);
                if (overwriteGame != null)
                    _context.Games.Remove(overwriteGame); // törlés

                Game dbGame = new Game
                {
                    TableSize = table.Size,
                   
                    Name = name
                }; // új mentés létrehozása

                for (Int32 i = 0; i < table.FieldSize; ++i)
                {
                    for (Int32 j = 0; j < table.FieldSize; ++j)
                    {
                        Field field = new Field
                        {
                            X = i,
                            Y = j,
                            Number = table.GetValue(i, j),
                            IsLocked = table.IsLocked(i, j),

                        };
                        dbGame.Fields.Add(field);
                    }
                } // mezők mentése

                _context.Games.Add(dbGame); // mentés hozzáadása a perzisztálandó objektumokhoz
                await _context.SaveChangesAsync(); // mentés az adatbázisba
            }
            catch (Exception ex)
            {
                throw new Mine_DetectorDataException();
            }
        }

        /// <summary>
        /// Játékállapot mentések lekérdezése.
        /// </summary>
        public async Task<ICollection<SaveEntry>> ListAsync()
        {
            try
            {
                return await _context.Games
                    .OrderByDescending(g => g.Time) // rendezés mentési idő szerint csökkenő sorrendben
                    .Select(g => new SaveEntry { Name = g.Name, Time = g.Time }) // leképezés: Game => SaveEntry
                    .ToListAsync();
            }
            catch
            {
                throw new Mine_DetectorDataException();
            }
        }
    }
}
