
using Newtonsoft.Json;
using seniorCapstone.Models;
using seniorCapstone.Services;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace seniorCapstone
{
	public partial class MainPage : ContentPage
	{
		private FieldSingleton fieldBackend = FieldSingleton.Instance;
		private const string OWM_APPID = "3ea77495368d801751fbd9266236f508";
		private Location userLocation;

		//**************************************************************************
		// Constructor:	MainPage
		//
		// Description:	Initialize the View, and set up user location
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public MainPage ()
		{
			InitializeComponent ();
			getUserLocation ();


			if (this.userLocation != null)
			{
				this.GetWeatherInfo ();
			}
			else
			{
				temperatureTxt.Text = "EvenStreamin requires access to your location in order to display weather";
			}
		}


		//**************************************************************************
		// Function:	getUserLocation
		//
		// Description:	Get current location
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
					await App.Current.MainPage.DisplayAlert ("Weather Alert",
						"Please naviagte to your app settings and allow EvenStreamin to access your location.", "Ok");
					return;
				}
				else // Just ask for permissions
				{
					status = await Permissions.RequestAsync<Permissions.LocationWhenInUse> ();
				}

				if (status != PermissionStatus.Granted)
				{
					await App.Current.MainPage.DisplayAlert ("Weather Alert",
						"EvenStreamin requires access to your location to display weather", "Ok");
					return;
				}
			}

			if (status == PermissionStatus.Granted)
			{
				userLocation = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync ();
			}
		}


		//**************************************************************************
		// Function:	GetWeatherInfo
		//
		// Description:	Call OpenWeatherMap API to get the current weather info
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void GetWeatherInfo ()
		{
			var url = $"https://api.openweathermap.org/data/2.5/weather?lat={userLocation.Latitude}&lon={userLocation.Longitude}&appid={OWM_APPID}&units=imperial";

			var result = await ApiCaller.Get (url);

			if (result.Successful)
			{
				try
				{
					var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo> (result.Response);
					
					if (weatherInfo.weather[0].main == "Clear")
					{
						//weatherframe.BackgroundColor = Color.LemonChiffon;

						weatherframe.BackgroundColor = Color.FromRgb (255, 247, 180);
					}
					else
					{
						weatherframe.BackgroundColor = Color.Gainsboro;
					}
					
					temperatureTxt.Text = weatherInfo.main.temp.ToString ("0");
					humidityTxt.Text = $"{weatherInfo.main.humidity}%";
					pressureTxt.Text = $"{weatherInfo.main.pressure} hPa";
					windTxt.Text = $"{weatherInfo.wind.speed} mph";
					cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

					//descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper ();
					//descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper ();
					//iconImg.Source = $"w{weatherInfo.weather[0].icon}";
					//cityTxt.Text = weatherInfo.name.ToUpper ();
					//var dt = new DateTime ().ToUniversalTime ().AddSeconds (weatherInfo.dt);
					//dateTxt.Text = dt.ToString ("dddd, MMM dd").ToUpper ();
				}
				catch (Exception ex)
				{
					await DisplayAlert ("Weather Info", ex.Message, "OK");
				}
			}
			else
			{
				await DisplayAlert ("Weather Info", "No weather information found", "OK");
			}
		}


	} 
}
