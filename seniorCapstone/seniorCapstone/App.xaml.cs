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
		public static int UserID { get; set; }

		public App()
		{
			InitializeComponent();

			//MainPage = new MainPage();
			// Wrap MainPage with Navigation Page
			MainPage = new NavigationPage (new MainPage ()); 
		}

		public App(string DBPath)
		{
			InitializeComponent();

			DatabasePath = DBPath;
			MainPage = new NavigationPage(new MainPage());
			if (!IsUserLoggedIn)
			{
				MainPage = new NavigationPage(new LoginPage());
			}
			else
			{
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
