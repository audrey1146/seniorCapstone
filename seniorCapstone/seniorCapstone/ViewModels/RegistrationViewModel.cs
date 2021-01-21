/****************************************************************************
 * File			RegistrationViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Registration functionality
 ****************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RegistrationViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Varibales
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


		/// <summary>
		/// Constructor that sets up the Cancel and Register commands
		/// </summary>
		public RegistrationViewModel ()
		{
			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.RegisterCommand = new Command (this.RegisterButton_Clicked);
		}


		/// <summary>
		/// When the register button is clicked this command with verify that all fields are filled
		/// out, and that the email/username don't already exist
		/// </summary>
		public async void RegisterButton_Clicked ()
		{
			int returnValue = 0;

			if (false == areEntiresFilledOut ())
			{
				await App.Current.MainPage.DisplayAlert ("Registration Alert", "Please Fill Out All Fields", "OK");
			}
			else
			{
				UserTable newUser = new UserTable ()
				{
					UserName = this.UserName,
					Password = this.Password,
					FirstName =this.FirstName,
					LastName = this.LastName,
					Email = this.Email

				};

				using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
				{
					dbConnection.CreateTable<UserTable> ();

					// Before insert check that the unique values don't already exist
					List<UserTable> uniqueCheck = dbConnection.Query<UserTable>
						("SELECT * FROM UserTable WHERE UserName=? OR Email=?",
						this.UserName, this.Email);

					if (null != uniqueCheck && uniqueCheck.Count == 0)
					{
						returnValue = dbConnection.Insert (newUser);
					}

					if (1 == returnValue)
					{
						await Application.Current.MainPage.Navigation.PopModalAsync ();
					}
					else
					{
						await App.Current.MainPage.DisplayAlert ("Registration Alert", "Username or Email Already Exists", "OK");
					}
				}
			}
		}


		/// <summary>
		/// Command that will pop the current page off of the stack
		/// </summary>
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
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
					false == string.IsNullOrEmpty (this.FirstName) &&
					false == string.IsNullOrEmpty (this.LastName) &&
					false == string.IsNullOrEmpty (this.Password) &&
					false == string.IsNullOrEmpty (this.Email));
		}
	}
}
