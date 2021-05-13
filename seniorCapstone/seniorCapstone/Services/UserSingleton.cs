/****************************************************************************
 * File		UserSingleton.cs
 * Author	Audrey Lincoln
 * Date		3/20/2021
 * Purpose	Singleton that has access to the user data service
 ****************************************************************************/

using seniorCapstone.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace seniorCapstone.Services
{
	public sealed class UserSingleton
	{
		// Private Variables
		readonly IUserDataService userDataService;
		ObservableCollection<UserTable> userEntries;

		private static readonly Lazy<UserSingleton> lazy = new Lazy<UserSingleton> (() => new UserSingleton ());

		// Public 
		public static UserSingleton Instance { get { return lazy.Value; } }

		//**************************************************************************
		// Function:	getSpecificUser
		//
		// Description:	Get user based off of ID
		//
		// Parameters:	id	-	ID to search for
		//
		// Returns:		null if does not exist; otherwise the user
		//**************************************************************************
		public UserTable getSpecificUser (string id)
		{
			foreach (UserTable user in this.userEntries)
			{
				if (user.UID == id)
				{
					return (user);
				}
			}

			return (null);
		}


		//**************************************************************************
		// Function:	getAllUsers
		//
		// Description:	Get all of the users in the system
		//
		// Parameters:	None
		//
		// Returns:		empty collection if does not exist; otherwise all the users
		//**************************************************************************
		public ObservableCollection<UserTable> getAllUsers ()
		{
			return (this.userEntries);
		}

		//**************************************************************************
		// Function:	AddUser
		//
		// Description:	Add a user entry to the database
		//
		// Parameters:	entry	-	Entry to be added
		//
		// Returns:		None
		//**************************************************************************
		public async Task AddUser (UserTable entry)
		{
			await this.userDataService.AddEntryAsync (entry);
			await this.ReloadAccountEntries ();
		}


		//**************************************************************************
		// Function:	DeleteUser
		//
		// Description:	Delete a user entry from the database
		//
		// Parameters:	entry	-	Entry to be deleted
		//
		// Returns:		None
		//**************************************************************************
		public async Task DeleteUser (UserTable entry)
		{
			await this.userDataService.DeleteEntryAsync (entry);
			await this.ReloadAccountEntries ();
		}


		//**************************************************************************
		// Function:	UpdateUser
		//
		// Description:	Edit a user entry from the database
		//
		// Parameters:	entry	-	Entry to be edited
		//
		// Returns:		None
		//**************************************************************************
		public async Task UpdateUser (UserTable entry)
		{
			await this.userDataService.EditEntryAsync (entry);
			await this.ReloadAccountEntries ();
		}


		//**************************************************************************
		// Function:	areCredentialsCorrect
		//
		// Description:	Verifies whether the credentials the user input 
		//				are correct by querying  into the user table.
		//
		// Parameters:	UserNameEntry	-	Attempted username
		//				PasswordEntry	-	Attempted password
		//
		// Returns:		True if correct; otherwise false
		//**************************************************************************
		public bool areCredentialsCorrect (string UserNameEntry, string PasswordEntry)
		{
			int count = 0;

			foreach (UserTable user in this.userEntries)
			{
				if (user.UserName == UserNameEntry && user.Password == PasswordEntry)
				{
					App.UserID = user.UID;
					count++;
				}
			}

			if (count != 1)
			{
				App.UserID = string.Empty;
				return (false);
			}

			return (true);
		}

		//**************************************************************************
		// Function:	AreEntiresUnique
		//
		// Description:	Verfies that the user input unqiue data for the Email and Username
		//
		// Parameters:	UserName	-	Attempted username
		//				Email		-	Attempted email
		//
		// Returns:		True if unique to the system; otherwise false
		//**************************************************************************
		public bool AreEntiresUnique (string UserName, string Email)
		{
			foreach (UserTable user in this.userEntries)
			{
				if (user.UserName == UserName || user.Email == Email)
				{
					return (false);
				}
			}

			return (true);
		}

		//**************************************************************************
		// Function:	IsEmailUnique
		//
		// Description:	Verfies that the user input unqiue data for the Email
		//
		// Parameters:	Email		-	Attempted email
		//
		// Returns:		True if the values are unique; otherwise false
		//**************************************************************************
		public bool IsEmailUnique (string Email)
		{
			foreach (UserTable user in this.userEntries)
			{
				if (user.Email == Email)
				{
					return (false);
				}
			}

			return (true);
		}

		//**************************************************************************
		// Function:	ReloadAccountEntries
		//
		// Description:	Calls the API and loads the returned data into a member variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async Task ReloadAccountEntries ()
		{
			try
			{
				var entries = await userDataService.GetEntriesAsync ();
				this.userEntries = new ObservableCollection<UserTable> (entries);
			}
			catch (Exception ex)
			{
				Debug.WriteLine ("Loading Accounts Failed");
				Debug.WriteLine (ex.Message);
			}
		}

		//**************************************************************************
		// Constructor:	UserSingleton
		//
		// Description:	Private constructor to set up access to API
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private UserSingleton ()
		{
			this.userDataService = new UserApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.userEntries = new ObservableCollection<UserTable> ();

			this.LoadAccountEntries ();
		}

		//**************************************************************************
		// Function:	LoadAccountEntries
		//
		// Description:	Calls the API and loads the returned data into a member variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void LoadAccountEntries ()
		{
			try
			{
				var entries = await userDataService.GetEntriesAsync ();
				this.userEntries = new ObservableCollection<UserTable> (entries);
			}
			catch (Exception ex)
			{
				Debug.WriteLine ("Loading Accounts Failed");
				Debug.WriteLine (ex.Message);
			}
		}


	}
}

