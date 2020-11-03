using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class LoginViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Varibales
		private string username = string.Empty;
		private string password = string.Empty;

		// Public Properties
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
		/// Constructor that sets up the specific commands
		/// </summary>
		public LoginViewModel()
		{
			this.LoginCommand = new Command (this.LoginButton_Clicked);
			this.RegistrationCommand = new Command (this.RegistrationButton_Clicked);
		}

		/// <summary>
		/// When the login button is pressed, the input credntials must be checked.
		/// If they are correct, then login as the user, else display an alert
		/// </summary>
		public async void LoginButton_Clicked ()
		{
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
					await App.Current.MainPage.DisplayAlert ("Login Alert", "Login Failed", "OK");
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
		/// 
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
		/// Verifies whether the credentials the user input are correct by querying 
		/// into the user table.
		/// </summary>
		/// <param name="UserNameEntry"></param>
		/// <param name="PasswordEntry"></param>
		/// <returns></returns>
		private bool areCredentialsCorrect (string UserNameEntry, string PasswordEntry)
		{
			bool isUser = false;

			// Verify input by a query to the databse
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable> ();

				// Query to get a single entry with specific Username and Password
				List<UserTable> userID = dbConnection.Query<UserTable>
					("SELECT UID FROM UserTable WHERE UserName=? AND Password=?",
					UserNameEntry, PasswordEntry);

				if (null != userID && userID.Count == 1)
				{
					isUser = true;
					App.UserID = userID[0].UID; // Other entries null because I am only returning the UID
				}
			}
			return isUser;
		}
	}
}
