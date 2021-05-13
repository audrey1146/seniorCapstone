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

		//**************************************************************************
		// Constructor:	App
		//
		// Description:	Set up the first page
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public App ()
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

		//**************************************************************************
		// Constructor:	App
		//
		// Description:	Set up the first page when using a SQLite DB
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public App (string DBPath)
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
