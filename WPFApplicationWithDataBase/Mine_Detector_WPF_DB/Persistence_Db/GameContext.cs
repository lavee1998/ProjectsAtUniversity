using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Mine_Detector_WPF_DB.Persistence
{
  
        class GameContext : DbContext
        {
            public GameContext(String connection)
                : base(connection)
            {
            }

            public DbSet<Game> Games { get; set; }
            public DbSet<Field> Fields { get; set; }

  
    }
    
}
