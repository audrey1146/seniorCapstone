using SQLite;

namespace seniorCapstone.Tables
{
	[Table("FieldTable")]
	public class FieldTable
	{
        [PrimaryKey, AutoIncrement]
        [Column("FID")]
        public string FID { get; set; }

        [Column("UID")]
        public string UID { get; set; }

        [Column("FieldName")]
        public string FieldName { get; set; }

        [Column("PivotLength")]
        public int PivotLength { get; set; }

        [Column ("SoilType")]
        public string SoilType { get; set; }

        [Column ("Latitude")]
        public string Latitude { get; set; }

        [Column("Longitude")]
        public string Longitude { get; set; }

        [Column("PivotRunning")]
        public bool PivotRunning { get; set; }

        [Column ("StopTime")]
        public string StopTime { get; set; }

        /* What to add to field table?
        
        [Column("SoilType")]
        public string SoilType { get; set; }
         
        [Column("CropType")]
        public string CropType { get; set; }
            (Empty, ...?) 
         
         */
    }
}
