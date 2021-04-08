/****************************************************************************
 * File			UserApiDataService.cs
 * Author		Audrey Lincoln + 'Mastering Xamarin.Forms' Book
 * Date			4/3/2021
 * Purpose		Implementation of IUserDataService, with access  
 *				to the BaseHttpService
 * Note         Defined in Chapter 6 page 116 in the book
 ****************************************************************************/

using seniorCapstone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="retUser"></param>
		/// <param name="id"></param>
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


		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<UserTable> getAllUsers ()
		{
			return (this.userEntries);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public async Task AddUser (UserTable entry)
		{
			await this.userDataService.AddEntryAsync (entry);
			await this.ReloadAccountEntries ();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public async Task DeleteUser (UserTable entry)
		{
			await this.userDataService.DeleteEntryAsync (entry);
			await this.ReloadAccountEntries ();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public async Task UpdateUser (UserTable entry)
		{
			await this.userDataService.EditEntryAsync (entry);
			await this.ReloadAccountEntries ();
		}


		/// <summary>
		/// Verifies whether the credentials the user input are correct by querying 
		/// into the user table.
		/// </summary>
		/// <param name="UserNameEntry"></param>
		/// <param name="PasswordEntry"></param>
		/// <returns></returns>
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


		/// <summary>
		/// Verfies that the user input unqiue data for the Email and Username
		/// </summary>
		/// <returns>
		/// True if the values are unique; otherwise false
		/// </returns>
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


		/// <summary>
		/// Verfies that the user input unqiue data for the Email and Username
		/// </summary>
		/// <returns>
		/// True if the values are unique; otherwise false
		/// </returns>
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

		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
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


		/// <summary>
		/// 
		/// </summary>
		private UserSingleton ()
		{
			this.userDataService = new UserApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.userEntries = new ObservableCollection<UserTable> ();

			this.LoadAccountEntries ();
		}

		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
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

