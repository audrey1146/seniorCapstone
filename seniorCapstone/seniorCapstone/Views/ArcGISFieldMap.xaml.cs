
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

		/// <summary>
		/// 
		/// </summary>
		public ArcGISFieldMap ()
		{
            InitializeComponent ();
			initMap ();
        }

		/// <summary>
		/// 
		/// </summary>
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


		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async void loadFieldInfo ()
		{
			this.currField = this.fieldBackend.getSpecificField (App.FieldID);
			if (currField == null)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
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