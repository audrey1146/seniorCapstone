/****************************************************************************
 * File			RegistrationViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Registration functionality
 ****************************************************************************/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Services;
using seniorCapstone.Models;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RegistrationViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Varibales
		private UserSingleton userBackend = UserSingleton.Instance;

		private string username = string.Empty;
		private string password = string.Empty;
		private string firstname = string.Empty;
		private string lastname = string.Empty;
		private string email = string.Empty;

		// Public Properties
		public ICommand RegisterCommand { get; set; }
		public ICommand CancelCommand { get; set; }
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
					OnPropertyChanged (nameof (this.email));
				}
			}
		}

		//**************************************************************************
		// Constructor:	RegistrationViewModel
		//
		// Description:	Constructor that sets up the Cancel and Register commands
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public RegistrationViewModel ()
		{
			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.RegisterCommand = new Command (this.RegisterButton_Clicked);
		}

		//**************************************************************************
		// Function:	RegisterButton_Clicked
		//
		// Description:	When the register button is clicked this command with 
		//				verify that all fields are filled out, and that the 
		//				email/username don't already exist
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void RegisterButton_Clicked ()
		{
			if (false == areEntiresFilledOut ())
			{
				await App.Current.MainPage.DisplayAlert ("Registration Alert", "Please Fill Out All Fields", "OK");
			}
			else if (false == userBackend.AreEntiresUnique (this.UserName, this.Email))
			{
				await App.Current.MainPage.DisplayAlert ("Registration Alert", "Username or Email Already Exists", "OK");
			}
			else
			{
				UserTable newUser = new UserTable ()
				{
					UserName = this.UserName,
					Password = this.Password,
					FirstName = this.FirstName,
					LastName = this.LastName,
					Email = this.Email
				};

				await this.userBackend.AddUser (newUser);
				await Application.Current.MainPage.Navigation.PopModalAsync ();
			}
		}

		//**************************************************************************
		// Function:	CancelButton_Clicked
		//
		// Description:	Command that will pop the current page off of the stack
		//
		// Parameters:	None
		//
		// Returns:		None	
		//**************************************************************************
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
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
		// Function:	areEntiresFilledOut
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
					false == string.IsNullOrEmpty (this.FirstName) &&
					false == string.IsNullOrEmpty (this.LastName) &&
					false == string.IsNullOrEmpty (this.Password) &&
					false == string.IsNullOrEmpty (this.Email));
		}
	}
}
