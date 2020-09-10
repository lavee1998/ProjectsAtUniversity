using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mine_Detector_WPF_DB.Persistence
{
    public interface IMine_DetectorDataAccess
    {
       // Mine_DetectorTable Mine_DetectorTable { get; set; }

        Task<Mine_DetectorTable> LoadAsync(String path);

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(String path, Mine_DetectorTable table);

        Task<ICollection<SaveEntry>> ListAsync();
    }
}
