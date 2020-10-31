using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
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

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection(App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable>();

				// Query for the current user
				List<UserTable> currentUser = dbConnection.Query<UserTable>
					("SELECT * FROM UserTable WHERE UID=?",
					App.UserID);

				// If query fails then pop this page off the stack
				if (null == currentUser || currentUser.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync();
					Debug.WriteLine("Finding Current User Failed");
				}
				else
				{
					username.Text = currentUser[0].UserName;
					firstname.Text = currentUser[0].FirstName;
					lastname.Text = currentUser[0].LastName;
					email.Text = currentUser[0].Email;
				}
			}
		}
	}
}