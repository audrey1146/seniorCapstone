/****************************************************************************
 * File		Geolocation.cs
 * Author	Audrey Lincoln
 * Date		2/20/2021
 * Purpose	Helper class that gets the coordinates of the device
 ****************************************************************************/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Xamarin.Essentials;

namespace seniorCapstone.Helpers
{
	public class Geolocation : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string latitude = string.Empty;
		private string longitude = string.Empty;
		private LocationDataSource phoneLocation = LocationDataSource.CreateDefault ();

		public string Latitude
		{
			get => this.latitude;
			set
			{
				if (this.latitude != value)
				{
					this.latitude = value;
					this.OnPropertyChanged (nameof (this.Latitude));
				}
			}
		}
		public string Longitude
		{
			get => this.longitude;
			set
			{
				if (this.longitude != value)
				{
					this.longitude = value;
					this.OnPropertyChanged (nameof (this.Longitude));
				}
			}
		}
		public LocationDataSource PhoneLocation
		{
			get => this.phoneLocation;
			set
			{
				if (this.phoneLocation != value)
				{
					this.phoneLocation = value;
					this.OnPropertyChanged (nameof (this.PhoneLocation));
				}
			}
		}

		//**************************************************************************
		// Constructor:	Geolocation
		//
		// Description:	Sets the location changed event
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public Geolocation ()
		{
			this.PhoneLocation.LocationChanged += LocationDisplay_LocationChanged;
		}

		//**************************************************************************
		// Function:	OnPropertyChanged
		//
		// Description:	Invoked when a property changes to notify the V and VM
		//
		// Parameters:	propertyName	-	Name of the specific property
		//
		// Returns:		None
		//**************************************************************************
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}


		//**************************************************************************
		// Function:	LocationDisplay_LocationChanged
		//
		// Description:	Updates the PhoneLocation property when the location changes
		//
		// Parameters:	sender	-	Name of the specific property
		//				e		-	New location
		//
		// Returns:		None
		//**************************************************************************
		private void LocationDisplay_LocationChanged (object sender, Esri.ArcGISRuntime.Location.Location e)
		{
			// Return if position is null; event is called with null position when location display is turned on.
			if (e.Position == null)
			{
				return;
			}
			string temp = CoordinateFormatter.ToLatitudeLongitude (e.Position, LatitudeLongitudeFormat.DecimalDegrees, 6);
			this.convertCoordinates (temp);
		}

		//**************************************************************************
		// Function:	getUserLocation
		//
		// Description:	Uses ArcGIS in order to get the location of the users phone
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void getUserLocation ()
		{
			var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse> ();

			if (status != PermissionStatus.Granted)
			{
				if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
				{
					// Prompt the user to turn on in settings
					// On iOS once a permission has been denied it may not be requested again from the application
					await App.Current.MainPage.DisplayAlert ("Geolocation Alert",
						"Please naviagte to your app settings and allow EvenStreamin to access your location.", "Ok");
					return;
				}
				else // Just ask for permissions
				{
					status = await Permissions.RequestAsync<Permissions.LocationWhenInUse> ();
				}

				if (status != PermissionStatus.Granted)
				{
					await App.Current.MainPage.DisplayAlert ("Geolocation Alert",
						"EvenStreamin requires access to your location in order to locate the center pivot.", "Ok");
					return;
				}
			}

			if (status == PermissionStatus.Granted)
			{
				await this.PhoneLocation.StartAsync ();
				await this.PhoneLocation.StopAsync ();
			}
		}

		//**************************************************************************
		// Function:	convertCoordinates
		//
		// Description:	Convert the ArcGIS string of coordinates into two separate values
		//
		// Parameters:	coordLatLong	-	Latitude and Longitude string, separated by a space
		//
		// Returns:		None
		//**************************************************************************
		private void convertCoordinates (string coordLatLong)
		{
			string[] coords = coordLatLong.Split (' ');
			this.Latitude = coords[0];
			this.Longitude = coords[1];
		}
	}
}
