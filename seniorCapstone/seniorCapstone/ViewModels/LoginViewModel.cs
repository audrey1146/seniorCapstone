/****************************************************************************
 * File			LoginViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Login functionality
 ****************************************************************************/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Views;
using seniorCapstone.Services;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class LoginViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;


		// Private Variables
		private UserSingleton userBackend = UserSingleton.Instance;
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

		//**************************************************************************
		// Constructor:	LoginViewModel
		//
		// Description:	Constructor that sets up the login and registration commands
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public LoginViewModel ()
		{
			this.LoginCommand = new Command (this.LoginButton_Clicked);
			this.RegistrationCommand = new Command (this.RegistrationButton_Clicked);
		}

		//**************************************************************************
		// Function:	LoginButton_Clicked
		//
		// Description:	When the login button is pressed, the input credntials must be checked.
		//				If they are correct, then login as the user, else display an alert
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void LoginButton_Clicked ()
		{
			if (false == areEntiresFilledOut ())
			{
				await App.Current.MainPage.DisplayAlert ("Login Alert", "Please Fill Out All Fields", "OK");
			}
			else
			{
				if (true == userBackend.areCredentialsCorrect (UserName, Password))
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

		//**************************************************************************
		// Function:	RegistrationButton_Clicked
		//
		// Description:	If the user wants to register an account then push on a Modal registration page
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void RegistrationButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PushModalAsync (new RegistrationPage ());
		}


		//**************************************************************************
		// Function:	OnPropertyChanged
		//
		// Description:	Invoked when a property changes to notify the view and viewmodel
		//
		// Parameters:	propertyName	-	Name of the property changed
		//
		// Returns:		None
		//**************************************************************************
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}

		//**************************************************************************
		// Function:	
		//
		// Description:	Verfies that the user input data for all entries
		//
		// Parameters:	None
		//
		// Returns:		True if all of the entries have some text in them, 
		//				otherwise false if any are empty or null
		//**************************************************************************
		private bool areEntiresFilledOut ()
		{
			return (false == string.IsNullOrEmpty (this.UserName) &&
					false == string.IsNullOrEmpty (this.Password));
		}
	}
}
