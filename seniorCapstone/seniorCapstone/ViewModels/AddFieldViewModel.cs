/****************************************************************************
 * File			AddFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Add Field functionality
 ****************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace seniorCapstone.ViewModels
{
	class AddFieldViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		//Private Varibales
		private string fieldName = string.Empty;
		private string latitude = string.Empty;
		private string longitude = string.Empty;
		private int pivotIndex = -1;
		private LocationDataSource phoneLocation = LocationDataSource.CreateDefault ();

		// Public Properties
		public ICommand AddFieldCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand SyncToPanelCommand { get; set; }
		public IList<int> PivotOptions { get; set; }
		public string FieldName
		{
			get => this.fieldName;
			set
			{
				if (this.fieldName != value)
				{
					this.fieldName = value;
					this.OnPropertyChanged (nameof (this.FieldName));
				}
			}
		}
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
		public int PivotIndex
		{
			get => this.pivotIndex;
			set
			{
				if (this.pivotIndex != value)
				{
					this.pivotIndex = value;
					this.OnPropertyChanged (nameof (this.PivotIndex));
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


		/// <summary>
		/// Constructor for the AddField VM. Sets the list of 
		/// viable pivot lengths
		/// </summary>
		public AddFieldViewModel ()
		{
			PivotOptions = new ObservableCollection<int> ()
			{
				400,
				300,
				60
			};
			this.PhoneLocation.LocationChanged += LocationDisplay_LocationChanged;

			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.AddFieldCommand = new Command (this.AddFieldButton_Clicked);
			this.SyncToPanelCommand = new Command (this.SyncToPanelButton_Clicked);
		}


		/// <summary>
		/// Command that will pop the current page off of the stack
		/// </summary>
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
		}


		/// <summary>
		/// Command that when pressed will attempt to add the field to the Field Table
		/// </summary>
		public async void AddFieldButton_Clicked ()
		{
			int returnValue = 0;

			if (true == this.areEntiresFilledOut ())
			{
				if (true == doesFieldNameExist ())
				{
					await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Field Name Already Exists", "Ok");
				}
				else if (true == doesFieldLocationExist ())
				{
					await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Field Location Already Exists", "Ok");
				}
				else
				{
					FieldTable newField = new FieldTable ()
					{
						UID = App.UserID,
						FieldName = this.FieldName,
						PivotLength = this.PivotOptions[this.PivotIndex],
						Latitude = this.Latitude,
						Longitude = this.Longitude,
						PivotAngle = 0,
						PivotRunning = 0
					};

					// Insert into DB (using closes the DB for me)
					using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
					{
						dbConnection.CreateTable<FieldTable> ();

						returnValue = dbConnection.Insert (newField);

						if (1 == returnValue)
						{
							await Application.Current.MainPage.Navigation.PopModalAsync ();
						}
						else
						{
							await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Did Not Insert Correctly", "Ok");
						}
					}
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Fill out entire form to add field", "Ok");
			}
		}


		/// <summary>
		/// Creates a popup to appear, with a callback method to get the users location
		/// </summary>
		public void SyncToPanelButton_Clicked ()
		{
			var popupPage = new SyncPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.getUserLocation ();

			PopupNavigation.Instance.PushAsync (popupPage);
		}


		/// <summary>
		/// Updates the PhoneLocation property each time the location changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LocationDisplay_LocationChanged (object sender, Esri.ArcGISRuntime.Location.Location e)
		{
			// Return if position is null; event is called with null position when location display is turned on.
			if (e.Position == null)
			{
				return;
			}
			string temp = CoordinateFormatter.ToLatitudeLongitude (e.Position, LatitudeLongitudeFormat.DecimalDegrees, 15);
			this.convertCoordinates (temp);
		}


		/// <summary>
		/// Invoked when a property changes to notify the view and viewmodel
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}


		/// <summary>
		/// Uses ArcGIS in order to get the location of the users phone
		/// </summary>
		private async void getUserLocation ()
		{
			var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse> ();

			if (status != PermissionStatus.Granted)
			{
				if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
				{
					// Prompt the user to turn on in settings
					// On iOS once a permission has been denied it may not be requested again from the application
					await App.Current.MainPage.DisplayAlert ("Add Field Alert",
						"Please naviagte to your app settings and allow EvenStreamin to access your location.", "Ok");
					return;
				}
				else // Just ask for permissions
				{
					status = await Permissions.RequestAsync<Permissions.LocationWhenInUse> ();
				}

				if (status != PermissionStatus.Granted)
				{
					await App.Current.MainPage.DisplayAlert ("Add Field Alert",
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


		/// <summary>
		/// Convert the ArcGIS string of coordinates into two separate values
		/// </summary>
		/// <param name="coordLatLong"></param>
		private void convertCoordinates (string coordLatLong)
		{
			// The latitude-longitude string will contain a space separating the latitude from the longitude value
			string[] coords = coordLatLong.Split (' ');
			this.Latitude = coords[0];
			this.Longitude = coords[1];
		}


		/// <summary>
		/// Query the database to check whether the field name already exists for the 
		/// current user
		/// </summary>
		private bool doesFieldNameExist ()
		{
			bool isField = false;

			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Make sure the name doesn't already exist
				List<FieldTable> fieldID = dbConnection.Query<FieldTable>
				("SELECT FID FROM FieldTable WHERE FieldName=? AND UID=?",
				this.FieldName, App.UserID);

				if (null == fieldID || fieldID.Count != 0)
				{
					isField = true;
				}
			}
			return isField;
		}


		/// <summary>
		/// Query the database to check whether the field location  already exists for the 
		/// current user
		/// </summary>
		private bool doesFieldLocationExist ()
		{
			bool isField = false;

			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> fieldID = dbConnection.Query<FieldTable>
				("SELECT FID FROM FieldTable WHERE Latitude=? AND Longitude=? AND UID=?",
				this.Latitude, this.Longitude, App.UserID);

				if (null == fieldID || fieldID.Count != 0)
				{
					isField = true;
				}
			}
			return isField;
		}


		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		private bool areEntiresFilledOut ()
		{
			return (false == string.IsNullOrEmpty (this.FieldName) &&
					false == string.IsNullOrEmpty (this.Latitude) &&
					false == string.IsNullOrEmpty (this.Longitude) &&
					-1 != this.PivotIndex);
		}

	}
}
