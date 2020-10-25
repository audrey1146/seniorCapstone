using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{
		public AccountPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection(App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable>();

				
			}
		}
	}
}