using SQLite;
using SQLiteNetExtensions.Attributes;

namespace seniorCapstone.Tables
{
	[Table("FieldTable")]
	class FieldTable
	{
        [PrimaryKey, AutoIncrement]
        [Column("FID")]
        public int FID { get; set; }

        [Column("UID")]
        public int UID { get; set; }

        [Column("FieldName")]
        public string FieldName { get; set; }

        [Column("PivotLength")]
        public int PivotLength { get; set; }

        // BELOW IS FROM HARDWARE / SOFTWARE LOCALIZATION
        [Column("Longitude")]
        public string Longitude { get; set; }

        [Column("Latitude")]
        public string Latitude { get; set; }

        // BELOW DEFAULT ZERO
        [Column("PivotAngle")]
        public int PivotAngle { get; set; }

        [Column("PivotRunning")]
        public int PivotRunning { get; set; }
    }
}
