/****************************************************************************
 * File			AccountViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/22/2020
 * Purpose		Account Page of the mobile application
 ****************************************************************************/

using System.Diagnostics;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Services;
using seniorCapstone.Models;
using seniorCapstone.Views;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	public class AccountViewModel : PageNavViewModel
	{
		// Private Variables
		private UserSingleton userBackend = UserSingleton.Instance;
		private FieldSingleton fieldBackend = FieldSingleton.Instance;

		private UserTable user = null;

		// Public Properties
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

		//**************************************************************************
		// Constructor:	AccountViewModel
		//
		// Description:	Constructor that sets the commands accessible from the Accound page, 
		//				and loads the user information
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public AccountViewModel ()
		{
			this.loadAccountInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.LogoutCommand = new Command (this.LogoutButton_Clicked);
			this.DeleteAccountCommand = new Command (this.DeleteAccountButton_Clicked);
			this.EditAccountCommand = new Command (this.EditAccountButton_Clicked);
		}

		//**************************************************************************
		// Function:	EditAccountButton_Clicked
		//
		// Description:	Display a popup to edit the users account.
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void EditAccountButton_Clicked ()
		{
			var popupPage = new EditAccountPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.loadAccountInfo ();

			await PopupNavigation.Instance.PushAsync (popupPage);
		}

		//**************************************************************************
		// Function:	LogoutButton_Clicked
		//
		// Description:	Verify that the use wants to log out, and then logs them out
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
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

		//**************************************************************************
		// Function:	DeleteAccountButton_Clicked
		//
		// Description:	Verify that the use wishes to delete their accout, 
		//				then deletes all information from the database connected 
		//				to that account and logs the use out.
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void DeleteAccountButton_Clicked ()
		{
			bool answer = await App.Current.MainPage.DisplayAlert ("Delete Account?",
				"Are you sure you want to delete your account? \nThis cannot be undone.", "Yes", "No");

			if (true == answer)
			{
				// Delete Account
				await this.userBackend.DeleteUser (this.User);

				// Delete Fields of Account
				await this.fieldBackend.DeleteAllFieldsOfUser (App.UserID);

				App.IsUserLoggedIn = false;
				App.UserID = string.Empty;
				Application.Current.MainPage = new NavigationPage (new LoginPage ());
			}
		}

		//**************************************************************************
		// Function:	loadAccountInfo
		//
		// Description:	Read from the database and bind that information to a UserTable variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void loadAccountInfo ()
		{
			this.User = this.userBackend.getSpecificUser (App.UserID);
			if (this.User == null)
			{
				Debug.WriteLine ("Finding Current User Failed");
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
		}
	}
}
