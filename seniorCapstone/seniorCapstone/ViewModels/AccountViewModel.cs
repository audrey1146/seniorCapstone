/****************************************************************************
 * File			AccountViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Account Page of the mobile application
 ****************************************************************************/

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class AccountViewModel : PageNavViewModel
	{
		// Private Variables
		private UserTable user = null;

		// Public Properties
		public ICommand DeleteAccountCommand { get; set; }
		public ICommand LogoutCommand { get; set; }
		public UserTable User
		{
			get => this.user;
			set
			{
				if (this.user != value)
				{
					this.user = value;
					base.OnPropertyChanged (nameof (this.User));
				}
			}
		}


		/// <summary>
		/// Constructor that sets the commands accessible from the Accound page, 
		/// and loads the user information
		/// </summary>
		public AccountViewModel ()
		{
			this.loadAccountInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.DeleteAccountCommand = new Command (this.DeleteAccountButton_Clicked);
			this.LogoutCommand = new Command (this.LogoutButton_Clicked);
		}


		/// <summary>
		/// Read from the database and bind that information to a UserTable variable
		/// </summary>
		private async void loadAccountInfo ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable> ();

				// Query for the current user
				List<UserTable> currentUser = dbConnection.Query<UserTable>
					("SELECT * FROM UserTable WHERE UID=?",
					App.UserID);

				// If query fails then pop this page off the stack
				if (null == currentUser || currentUser.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding Current User Failed");
				}
				else
				{
					this.User = currentUser[0];
				}
			}
		}


		/// <summary>
		/// Verify that the use wishes to delete their accout, then deletes all information from the 
		/// database connected to that account and logs the use out.
		/// </summary>
		public async void DeleteAccountButton_Clicked ()
		{
			bool answer = await App.Current.MainPage.DisplayAlert ("Delete Account?", 
				"Are you sure you want to delete your account? \nThis cannot be undone.", "Yes", "No");

			if (true == answer)
			{
				using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
				{
					dbConnection.CreateTable<UserTable> ();

					List<UserTable> userAccount = dbConnection.Query<UserTable>
					("DELETE FROM UserTable WHERE UID=?", App.UserID);

					userAccount = dbConnection.Query<UserTable>
					("SELECT * FROM UserTable WHERE UID=?", App.UserID);

					if (null == userAccount || userAccount.Count == 0)
					{
						Debug.WriteLine ("Account Deleted");
					}

					dbConnection.CreateTable<FieldTable> ();

					List<FieldTable> userFields = dbConnection.Query<FieldTable>
					("DELETE FROM FieldTable WHERE UID=?", App.UserID);

					userFields = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=?", App.UserID);

					if (null == userFields || userFields.Count == 0)
					{
						Debug.WriteLine ("Fields Deleted");
					}
				}

				App.IsUserLoggedIn = false;
				App.UserID = -1;
				Application.Current.MainPage = new NavigationPage (new LoginPage ());
			}
		}

		/// <summary>
		/// Verify that the use wants to log out, and then logs them out
		/// </summary>
		public async void LogoutButton_Clicked ()
		{
			bool answer = await App.Current.MainPage.DisplayAlert ("Logging Out?",
				"Are you sure you want to logout?", "Yes", "No");

			if (true == answer)
			{
				App.IsUserLoggedIn = false;
				App.UserID = -1;
				Application.Current.MainPage = new NavigationPage (new LoginPage ());
			}
		}
	}
}
