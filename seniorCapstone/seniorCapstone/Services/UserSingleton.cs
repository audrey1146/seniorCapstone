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
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="retUser"></param>
		/// <param name="id"></param>
		public void getSpecificUser (ref UserTable retUser, string id)
		{
			foreach (UserTable user in this.userEntries)
			{
				if (user.UID == id)
				{
					retUser = user;
					return;
				}
			}

			retUser = null;
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

