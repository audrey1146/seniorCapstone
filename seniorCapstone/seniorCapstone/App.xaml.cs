using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone
{
	public partial class App : Application
	{
		// path to DB
		public static string DatabasePath = string.Empty;

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
