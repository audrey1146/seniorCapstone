
using seniorCapstone.Models;
using System.Diagnostics;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using seniorCapstone.Services;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class ArcGISFieldMap : ContentPage
	{
		// Private Variables
		private FieldSingleton fieldBackend = FieldSingleton.Instance;
		private FieldTable currField;

		//**************************************************************************
		// Constructor:	ArcGISFieldMap
		//
		// Description:	Initialize the map
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public ArcGISFieldMap ()
		{
            InitializeComponent ();
			initMap ();
        }

		//**************************************************************************
		// Function:	initMap
		//
		// Description:	Create map from the coordinates of the field
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private void initMap ()
        {
			this.loadFieldInfo ();

			Map fieldMap;
			double latitude = this.convertToDouble (this.currField.Latitude);
			double longitude = this.convertToDouble (this.currField.Longitude);

			// Create a map with 'Imagery with Labels' basemap and an initial location
			fieldMap = new Map (BasemapType.ImageryWithLabels, latitude, longitude, 16);

			// Assign the map to the MapView
			MyMapView.Map = fieldMap;
		}

		//**************************************************************************
		// Function:	loadFieldInfo
		//
		// Description:	Calls the API and loads the returned data into a member variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void loadFieldInfo ()
		{
			this.currField = this.fieldBackend.getSpecificField (App.FieldID);
			if (currField == null)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
		}


		//**************************************************************************
		// Function:	convertToDouble
		//
		// Description:	Converts the ArcGIS string into coordinate doubles
		//
		// Parameters:	coord	-	Full coordinate
		//
		// Returns:		The value of the coordinate
		//**************************************************************************
		private double convertToDouble (string coord)
		{
			double convertedCoord = 0;
			bool bIsNegative = false;
			int poleIndex = coord.Length - 1;

			if ('S' == coord[poleIndex] || 'W' == coord[poleIndex])
			{
				bIsNegative = true;
			}

			// Remove pole at the end
			coord = coord.Remove (poleIndex);

			// Parse string
			double.TryParse (coord, out convertedCoord);

			if (bIsNegative)
			{
				convertedCoord *= -1;
			}

			return (convertedCoord);
		}
	}
}