
using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Esri.ArcGISRuntime.UI;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class ArcGISFieldMap : ContentPage
	{
		public ArcGISFieldMap ()
		{
            InitializeComponent ();
            Initialize ();
        }

        private async void Initialize ()
        {
			Map fieldMap;
			MapPoint mapPointCoordinates;
			string coordinates;

			

			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> currentField = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND FID=?", App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == currentField)
				{
					Debug.WriteLine ("Failed to Find Current Field");
					await Application.Current.MainPage.Navigation.PopAsync ();
				}
				else
				{
					/*coordinates = currentField[0].Latitude + ' ' + currentField[0].Longitude;
					mapPointCoordinates = CoordinateFormatter.FromLatitudeLongitude (coordinates, SpatialReferences.Wgs84);
					

					// Create a map with 'Imagery with Labels' basemap and an initial location
					fieldMap = new Map (BasemapType.ImageryWithLabels, mapPointCoordinates.X, mapPointCoordinates.Y, 16);

					MyMapView.Map = fieldMap;*/

					/*
						TO DO:
						-----------
						1.	Write my own converter to take the lat/long and make it a double
							Idea: char array, 6 decimals, S/W convert to negative
						2.	Once we have this, then new Map () will take it in lat long order
						3.	If this doesn't work I drop out
					 */



					// Create a map with 'Imagery with Labels' basemap and an initial location
					Map myMap = new Map (BasemapType.ImageryWithLabels, -33.173240, -64.256609, 16);

					// Assign the map to the MapView
					MyMapView.Map = myMap;

				}
			}
		}


	}
}