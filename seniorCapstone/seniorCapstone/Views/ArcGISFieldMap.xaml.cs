
using seniorCapstone.Models;
using System.Diagnostics;
using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using seniorCapstone.Services;
using System.Threading.Tasks;
using System;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class ArcGISFieldMap : ContentPage
	{
		// Private Variables
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;

		// Public Properties
		public ObservableCollection<FieldTable> FieldEntries
		{
			get => this.fieldEntries;
			set
			{
				this.fieldEntries = value;
				OnPropertyChanged ();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public ArcGISFieldMap ()
		{
            InitializeComponent ();

			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();
			initMap ();
        }

		/// <summary>
		/// 
		/// </summary>
        private async void initMap ()
        {
			Map fieldMap;
			int count = 0;
			double latitude = 0;
			double longitude = 0;

			await this.LoadEntries ();

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FID == App.FieldID && field.UID == App.UserID)
				{
					count++;
					latitude = this.convertToDouble (field.Latitude);
					longitude = this.convertToDouble (field.Longitude);
				}
			}

			if (count != 1)
			{
				Debug.WriteLine ("Failed to Find Current Field");
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
			else
			{
				// Create a map with 'Imagery with Labels' basemap and an initial location
				fieldMap = new Map (BasemapType.ImageryWithLabels, latitude, longitude, 16);

				// Assign the map to the MapView
				MyMapView.Map = fieldMap;
			}
		}


		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async Task LoadEntries ()
		{
			try
			{
				var entries = await fieldDataService.GetEntriesAsync ();
				this.FieldEntries = new ObservableCollection<FieldTable> (entries);
			}
			catch (Exception ex)
			{
				Debug.WriteLine ("Loading Fields Failed");
				Debug.WriteLine (ex.Message);
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