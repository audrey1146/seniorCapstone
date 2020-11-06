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
		// Public Properties
		public ICommand DeleteAccountCommand { get; set; }

		public AccountViewModel ()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.DeleteAccountCommand = new Command (this.DeleteAccountButton_Clicked);
		}

		/// <summary>
		/// 
		/// </summary>
		public async void DeleteAccountButton_Clicked ()
		{
			bool answer = await App.Current.MainPage.DisplayAlert ("Delete Account?", 
				"Are you sure you want to delete your account? This cannot be undone.", "Yes", "No");

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
	}
}
