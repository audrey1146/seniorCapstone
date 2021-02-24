
using Newtonsoft.Json;
using seniorCapstone.Tables;
using seniorCapstone.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace seniorCapstone
{
	public partial class MainPage : ContentPage
	{
		private const string OWM_APPID = "3ea77495368d801751fbd9266236f508";
		private Location userLocation;


		public MainPage()
		{
			InitializeComponent ();
			getUserLocation ();


			if (userLocation != null)
			{
				this.GetWeatherInfo ();
			}
			else
			{
				temperatureTxt.Text = "EvenStreamin requires access to your location in order to display weather";
			}
		}


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
						"EvenStreamin requires access to your location in order to display weather", "Ok");
					return;
				}
			}

			if (status == PermissionStatus.Granted)
			{
				userLocation = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync ();
			}
		}


		private async void GetWeatherInfo ()
		{
			var url = $"https://api.openweathermap.org/data/2.5/weather?lat={userLocation.Latitude}&lon={userLocation.Longitude}&appid={OWM_APPID}&units=imperial";

			var result = await ApiCaller.Get (url);

			if (result.Successful)
			{
				try
				{
					var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo> (result.Response);
					//descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper ();
					//iconImg.Source = $"w{weatherInfo.weather[0].icon}";
					//cityTxt.Text = weatherInfo.name.ToUpper ();
					temperatureTxt.Text = weatherInfo.main.temp.ToString ("0");
					humidityTxt.Text = $"{weatherInfo.main.humidity}%";
					pressureTxt.Text = $"{weatherInfo.main.pressure} hPa";
					windTxt.Text = $"{weatherInfo.wind.speed} mph";
					cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";

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
