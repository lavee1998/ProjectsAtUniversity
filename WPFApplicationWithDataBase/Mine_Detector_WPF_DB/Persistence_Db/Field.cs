using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;


namespace Mine_Detector_WPF_DB.Persistence
{
    /// <summary>
    /// Mező entitás típusa.
    /// </summary>
    class Field
    {
        /// <summary>
        /// Egyedi azonosító.
        /// </summary>
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// Vízszintes koordináta.
        /// </summary>
        public Int32 X { get; set; }
        /// <summary>
        /// Függőleges koordináta.
        /// </summary>
        public Int32 Y { get; set; }
        /// <summary>
        /// Tárolt érték.
        /// </summary>
      //  public Color Color { get; set; }


        public Int32 Number { get; set; }
        /// <summary>
        /// Zárolt tulajdonság lekérdezése.
        /// </summary>
        public Boolean IsLocked { get; set; }

        /// <summary>
        /// Kapcsolt játék.
        /// </summary>
        public Game Game { get; set; }

      
    }
}