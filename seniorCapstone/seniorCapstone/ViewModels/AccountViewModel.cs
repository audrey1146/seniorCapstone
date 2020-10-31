/****************************************************************************
 * File			AccountViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Account Page of the mobile application
 ****************************************************************************/

using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class AccountViewModel : PageNavViewModel
	{
		public AccountViewModel ()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
		}

		/*
		// Private Variables
		private string username = string.Empty;
		private string firstname = string.Empty;
		private string lastname = string.Empty;
		private string email = string.Empty;

		// Public Properties
		public string UserName
		{
			get => this.username;
			set
			{
				if (this.username != value)
				{
					this.username = value;
					OnPropertyChanged (nameof (this.UserName));
				}
			}
		}
		public string FirstName
		{
			get => this.firstname;
			set
			{
				if (this.firstname != value)
				{
					this.firstname = value;
					OnPropertyChanged (nameof (this.FirstName));
				}
			}
		}		
		public string LastName
		{
			get => this.lastname;
			set
			{
				if (this.lastname != value)
				{
					this.lastname = value;
					OnPropertyChanged (nameof (this.LastName));
				}
			}
		}
		public string Email
		{
			get => this.email;
			set
			{
				if (this.email != value)
				{
					this.email = value;
					OnPropertyChanged (nameof (this.Email));
				}
			}
		}


		public async void LoadAccount ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable> ();

				// Query for the current user
				List<UserTable> currentUser = dbConnection.Query<UserTable>
					("SELECT * FROM UserTable WHERE UID=?", App.UserID);

				// If query fails then pop this page off the stack
				if (null == currentUser || currentUser.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding Current User Failed");
				}
				else
				{
					this.UserName = currentUser[0].UserName;
					this.FirstName = currentUser[0].FirstName;
					this.LastName = currentUser[0].LastName;
					this.Email = currentUser[0].Email;
				}
			}
		}*/
	}
}
