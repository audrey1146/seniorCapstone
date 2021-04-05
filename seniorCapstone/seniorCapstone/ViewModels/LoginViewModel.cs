/****************************************************************************
 * File			LoginViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Login functionality
 ****************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using seniorCapstone.Services;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace seniorCapstone.ViewModels
{
	public class LoginViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Variables
		readonly IUserDataService userDataService;
		ObservableCollection<UserTable> userEntries;

		private string username = string.Empty;
		private string password = string.Empty;

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

		public ICommand LoginCommand { get; set; }
		public ICommand RegistrationCommand { get; set; }
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
		public string Password
		{
			get => this.password;
			set
			{
				if (this.password != value)
				{
					this.password = value;
					OnPropertyChanged (nameof (this.Password));
				}
			}
		}

		/// <summary>
		/// Constructor that sets up the login and registration commands
		/// </summary>
		public LoginViewModel ()
		{
			this.userDataService = new UserApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.UserEntries = new ObservableCollection<UserTable> ();

			this.LoginCommand = new Command (this.LoginButton_Clicked);
			this.RegistrationCommand = new Command (this.RegistrationButton_Clicked);
		}


		/// <summary>
		/// When the login button is pressed, the input credntials must be checked.
		/// If they are correct, then login as the user, else display an alert
		/// </summary>
		public async void LoginButton_Clicked ()
		{
			await this.LoadEntries ();

			if (false == areEntiresFilledOut ())
			{
				await App.Current.MainPage.DisplayAlert ("Login Alert", "Please Fill Out All Fields", "OK");
			}
			else
			{
				if (true == areCredentialsCorrect (UserName, Password))
				{
					App.IsUserLoggedIn = true;
					Application.Current.MainPage = new NavigationPage (new MainPage ());
				}
				else
				{
					await App.Current.MainPage.DisplayAlert ("Login Alert",
						"The username and password you entered did not match our records. \nPlease double-check and try again.", "OK");
					this.Password = string.Empty;
				}
			}
		}


		/// <summary>
		/// If the user wants to register an account then push on a Modal registration page
		/// </summary>
		public async void RegistrationButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PushModalAsync (new RegistrationPage ());
		}


		/// <summary>
		/// Invoked when a property changes to notify the view and viewmodel
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}


		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		private bool areEntiresFilledOut ()
		{
			return (false == string.IsNullOrEmpty (this.UserName) &&
					false == string.IsNullOrEmpty (this.Password));
		}


		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async Task LoadEntries ()
		{
			try
			{
				var entries = await userDataService.GetEntriesAsync ();
				this.UserEntries = new ObservableCollection<UserTable> (entries);
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert ("Login Alert", ex.Message, "OK");
			}
		}


		/// <summary>
		/// Verifies whether the credentials the user input are correct by querying 
		/// into the user table.
		/// </summary>
		/// <param name="UserNameEntry"></param>
		/// <param name="PasswordEntry"></param>
		/// <returns></returns>
		private bool areCredentialsCorrect (string UserNameEntry, string PasswordEntry)
		{
			int count = 0;

			foreach (UserTable user in this.UserEntries)
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

	}
}
