using Rg.Plugins.Popup.Pages;
using System;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Models;
using Xamarin.Forms.Xaml;
using seniorCapstone.Services;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditAccountPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;

		// Private Variables
		private UserSingleton userBackend = UserSingleton.Instance;
		UserTable singleUser;


		//**************************************************************************
		// Constructor:	EditAccountPopupPage
		//
		// Description:	Initialize the place holders
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public EditAccountPopupPage ()
		{
			InitializeComponent ();
			this.setPlaceholders ();
		}


		//**************************************************************************
		// Function:	setPlaceholder
		//
		// Description:	Get the current user data to display as place holders
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void setPlaceholders ()
		{
			this.singleUser = this.userBackend.getSpecificUser (App.UserID);
			if (this.singleUser == null)
			{
				Debug.WriteLine ("Finding Current User Failed");
				await PopupNavigation.Instance.PopAsync (true);
			}
			else
			{
				firstname.Placeholder = singleUser.FirstName;
				lastname.Placeholder = singleUser.LastName;
				email.Placeholder = singleUser.Email;
			}
		}


		//**************************************************************************
		// Function:	SubmitButton_Clicked
		//
		// Description:	When the submit button is clicked this command will check update the 
		//				table entry according to the values they specified
		//
		// Parameters:	sender	-	Object that sent the message
		//				e		-	List View item that was tapped
		//
		// Returns:		None
		//**************************************************************************
		public async void SubmitButton_Clicked (object sender, EventArgs args)
		{
			UserTable newUser = new UserTable (ref this.singleUser);
			bool bPopOff = true;

			if (false == this.areEntriesFilled ())
			{
				await App.Current.MainPage.DisplayAlert ("Update Account Alert", "No New Entries", "OK");
				bPopOff = false;
			}
			else
			{
				// Update based on which entries got filled out
				if (false == string.IsNullOrEmpty (password.Text))
				{
					newUser.Password = password.Text;
				}
				if (false == string.IsNullOrEmpty (firstname.Text))
				{
					newUser.FirstName = firstname.Text;
				}
				if (false == string.IsNullOrEmpty (lastname.Text))
				{
					newUser.LastName = lastname.Text;
				}
				if (false == string.IsNullOrEmpty (email.Text))
				{
					// Before update check that the unique values don't already exist
					if (true == this.userBackend.IsEmailUnique (email.Text))
					{
						await App.Current.MainPage.DisplayAlert ("Update Account Alert", "Email Already Exists", "OK");
						bPopOff = false;
					}
					else
					{
						newUser.Email = email.Text;
					}
				}
			}
			
			if (true == bPopOff)
			{
				await this.userBackend.UpdateUser (newUser);

				await PopupNavigation.Instance.PopAsync (true);
				CallbackEvent?.Invoke (this, EventArgs.Empty);
			}
		}


		//**************************************************************************
		// Function:	CancelButton_Clicked
		//
		// Description:	Popoff the popup page
		//
		// Parameters:	sender	-	Object that sent the message
		//				e		-	List View item that was tapped
		//
		// Returns:		None
		//**************************************************************************
		public async void CancelButton_Clicked (object sender, EventArgs args)
		{
			await PopupNavigation.Instance.PopAsync (true);
		}


		//**************************************************************************
		// Function:	areEntriesFilled
		//
		// Description:	Check that at least one field is filled
		//
		// Parameters:	None
		//
		// Returns:		True if filled out; otherwise false
		//**************************************************************************
		private bool areEntriesFilled ()
		{
			return (false == string.IsNullOrEmpty (password.Text)
				|| false == string.IsNullOrEmpty (firstname.Text)
				|| false == string.IsNullOrEmpty (lastname.Text)
				|| false == string.IsNullOrEmpty (email.Text));
		}
	}
}