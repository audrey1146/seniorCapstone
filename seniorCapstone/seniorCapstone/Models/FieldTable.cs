/****************************************************************************
 * File     FieldTable.cs
 * Author   Audrey Lincoln
 * Date	    9/20/2020
 * Purpose	Information for a field entry 
 ****************************************************************************/

using SQLite;

namespace seniorCapstone.Models
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


		//**************************************************************************
		// Constructor: FieldTable
		//
		// Description:	Sets table values to default values
		//
		// Parameters:	None
		//
		// Returns:     None
		//**************************************************************************
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

		//**************************************************************************
		// Constructor: FieldTable
		//
		// Description:	Paramaterized constructor to set values equal to another
		//
		// Parameters:	fieldEntry  -   Object to become equal to
		//
		// Returns:     None
		//**************************************************************************
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

		//**************************************************************************
		// Function:    assignTo
		//
		// Description:	Set values equal to another object
		//
		// Parameters:	fieldEntry  -   Object to become equal to
		//
		// Returns:     None
		//**************************************************************************
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
