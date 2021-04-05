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

        [Column ("WaterUsage")]
        public double WaterUsage { get; set; }

        /* What to add to field table?
        
        [Column("SoilType")]
        public string SoilType { get; set; }
         
        [Column("CropType")]
        public string CropType { get; set; }
            (Empty, ...?) 
         
         */

        public FieldTable ()
		{
            this.FID = string.Empty;
            this.UID = string.Empty;
            this.FieldName = string.Empty;
            this.PivotLength = 0;
            this.SoilType = string.Empty;
            this.Latitude = string.Empty;
            this.Longitude = string.Empty;
            this.PivotRunning = false;
            this.StopTime = string.Empty;
            this.WaterUsage = 0;
        }

        public FieldTable (ref FieldTable fieldEntry)
		{
            this.FID = fieldEntry.FID;
            this.UID = fieldEntry.UID;
            this.FieldName = fieldEntry.FieldName;
            this.PivotLength = fieldEntry.PivotLength;
            this.SoilType = fieldEntry.SoilType;
            this.Latitude = fieldEntry.Latitude;
			this.Longitude = fieldEntry.Longitude;
			this.PivotRunning = fieldEntry.PivotRunning;
			this.StopTime = fieldEntry.StopTime;
            this.WaterUsage = fieldEntry.WaterUsage;
        }

        public void assignTo (FieldTable fieldEntry)
		{
            this.FID = fieldEntry.FID;
            this.UID = fieldEntry.UID;
            this.FieldName = fieldEntry.FieldName;
            this.PivotLength = fieldEntry.PivotLength;
            this.SoilType = fieldEntry.SoilType;
            this.Latitude = fieldEntry.Latitude;
            this.Longitude = fieldEntry.Longitude;
            this.PivotRunning = fieldEntry.PivotRunning;
            this.StopTime = fieldEntry.StopTime;
            this.WaterUsage = fieldEntry.WaterUsage;
        }
    }
}
