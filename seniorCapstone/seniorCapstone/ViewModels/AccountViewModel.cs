/****************************************************************************
 * File			AccountViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Account Page of the mobile application
 ****************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Services;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class AccountViewModel : PageNavViewModel
	{
		// Private Variables
		readonly IUserDataService userDataService;
		ObservableCollection<UserTable> userEntries;
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;

		private UserTable user = null;

		// Public Properties
		public ObservableCollection<UserTable> UserEntries
		{
			get => this.userEntries;
			set
			{
				this.userEntries = value;
				OnPropertyChanged ();
			}
		}
		public ObservableCollection<FieldTable> FieldEntries
		{
			get => this.fieldEntries;
			set
			{
				this.fieldEntries = value;
				OnPropertyChanged ();
			}
		}
		public ICommand EditAccountCommand { get; set; }
		public ICommand LogoutCommand { get; set; }
		public ICommand DeleteAccountCommand { get; set; }
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
			this.userDataService = new UserApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.UserEntries = new ObservableCollection<UserTable> ();

			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();

			this.loadAccountInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.LogoutCommand = new Command (this.LogoutButton_Clicked);
			this.DeleteAccountCommand = new Command (this.DeleteAccountButton_Clicked);
			this.EditAccountCommand = new Command (this.EditAccountButton_Clicked);
		}


		/// <summary>
		/// Display a popup to edit the users account.
		/// </summary>
		public async void EditAccountButton_Clicked ()
		{
			var popupPage = new EditAccountPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += async (object sender, object e) => await this.loadAccountInfo ();

			await PopupNavigation.Instance.PushAsync (popupPage);
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
				App.UserID = string.Empty;
				Application.Current.MainPage = new NavigationPage (new LoginPage ());
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
				// Delete Account
				await this.userDataService.DeleteEntryAsync (this.User);

				// Delete Fields of Account
				await this.LoadFieldEntries ();

				foreach (FieldTable field in this.FieldEntries)
				{
					if (field.UID == App.UserID)
					{
						await this.fieldDataService.DeleteEntryAsync (field);
					}
				}

				App.IsUserLoggedIn = false;
				App.UserID = string.Empty;
				Application.Current.MainPage = new NavigationPage (new LoginPage ());
			}
		}


		/// <summary>
		/// Read from the database and bind that information to a UserTable variable
		/// </summary>
		private async Task loadAccountInfo ()
		{
			int count = 0;

			await this.LoadAccountEntries ();

			foreach (UserTable user in this.UserEntries)
			{
				if (user.UID == App.UserID)
				{
					this.User = user;
					count++;
				}
			}

			// If query fails then pop this page off the stack
			if (count != 1)
			{
				await Application.Current.MainPage.Navigation.PopAsync ();
				Debug.WriteLine ("Finding Current User Failed");
			}
		}


		/// <summary>
		/// Read from the database and bind that information to a FieldTable variable
		/// </summary>
		private async Task LoadFieldEntries ()
		{
			try
			{
				var entries = await fieldDataService.GetEntriesAsync ();
				this.FieldEntries = new ObservableCollection<FieldTable> (entries);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.Navigation.PopAsync ();
				Debug.WriteLine ("Loading Fields Failed");
			}
		}


		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async Task LoadAccountEntries ()
		{
			try
			{
				var entries = await userDataService.GetEntriesAsync ();
				this.UserEntries = new ObservableCollection<UserTable> (entries);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.Navigation.PopAsync ();
				Debug.WriteLine ("Loading Accounts Failed");
			}
		}

	}
}
