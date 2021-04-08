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
using seniorCapstone.Models;
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

		/// <summary>
		/// Constructor that sets up the login and registration commands
		/// </summary>
		public LoginViewModel ()
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
	}
}
