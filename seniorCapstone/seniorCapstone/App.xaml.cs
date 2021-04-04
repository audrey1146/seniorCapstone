using seniorCapstone.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone
{
	public partial class App : Application
	{
		// path to DB
		public static string DatabasePath = string.Empty;

		// Current User Information
		public static bool IsUserLoggedIn { get; set; }
		public static string UserID { get; set; }
		public static string FieldID { get; set; }

		public App()
		{
			InitializeComponent();
			FieldID = string.Empty;
			if (!IsUserLoggedIn)
			{
				MainPage = new NavigationPage(new LoginPage());
			}
			else
			{
				UserID = string.Empty;
				MainPage = new NavigationPage(new MainPage());
			}
		}

		public App(string DBPath)
		{
			InitializeComponent();
			FieldID = string.Empty;
			DatabasePath = DBPath;
			if (!IsUserLoggedIn)
			{
				MainPage = new NavigationPage(new LoginPage());
			}
			else
			{
				UserID = string.Empty;
				MainPage = new NavigationPage(new MainPage());
			}
		}


		protected override void OnStart ()
		{
		}

		protected override void OnSleep ()
		{
		}

		protected override void OnResume ()
		{
		}
	}
}
