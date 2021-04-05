using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using seniorCapstone.Services;
using System.Threading.Tasks;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditAccountPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;

		// Private Variables
		readonly IUserDataService userDataService;
		ObservableCollection<UserTable> userEntries;
		UserTable singleUser;

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


		/// <summary>
		/// 
		/// </summary>
		public EditAccountPopupPage ()
		{
			InitializeComponent ();
			this.userDataService = new UserApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.UserEntries = new ObservableCollection<UserTable> ();
			this.setPlaceholders ();
		}


		/// <summary>
		/// 
		/// </summary>
		private async void setPlaceholders ()
		{
			int count = 0;

			await this.LoadEntries ();

			foreach (UserTable user in this.UserEntries)
			{
				if (user.UID == App.UserID)
				{
					count++;
					singleUser = user;
					firstname.Placeholder = user.FirstName;
					lastname.Placeholder = user.LastName;
					email.Placeholder = user.Email;
				}
			}

			if (count != 1)
			{
				Debug.WriteLine ("Finding Current User Failed");
				await PopupNavigation.Instance.PopAsync (true);
			}
		}


		/// <summary>
		/// Get the current user data to display as place holders
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
				Debug.WriteLine ("Finding Current User Failed");
				Debug.WriteLine (ex.Message);
				await PopupNavigation.Instance.PopAsync (true);
			}
		}


		/// <summary>
		/// When the submit button is clicked this command will check update the 
		/// table entry according to the values they specified
		/// </summary>
		/// /// <param name="sender"></param>
		/// <param name="args"></param>
		public async void SubmitButton_Clicked (object sender, EventArgs args)
		{
			UserTable newUser = new UserTable ();
			bool bPopOff = true;

			newUser.UID = this.singleUser.UID;
			newUser.UserName = this.singleUser.UserName;
			newUser.Password = this.singleUser.Password;
			newUser.FirstName = this.singleUser.FirstName;
			newUser.LastName = this.singleUser.LastName;
			newUser.Email = this.singleUser.Email;

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
				if (true == doesEmailExist ())
				{
					await App.Current.MainPage.DisplayAlert ("Update Account Alert", "Email Already Exists", "OK");
					bPopOff = false;
				}
				else
				{
					newUser.Email = email.Text;
				}
			}

			if (true == bPopOff)
			{
				await this.userDataService.EditEntryAsync (newUser);

				await PopupNavigation.Instance.PopAsync (true);
				CallbackEvent?.Invoke (this, EventArgs.Empty);
			}
		}


		/// <summary>
		/// Pop off the popup page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public async void CancelButton_Clicked (object sender, EventArgs args)
		{
			await PopupNavigation.Instance.PopAsync (true);
		}


		/// <summary>
		/// Query the database to check whether the email exists
		/// in the entire system
		/// </summary>
		private bool doesEmailExist ()
		{
			foreach (UserTable user in this.UserEntries)
			{
				if (user.Email == email.Text)
				{
					return (false);
				}
			}

			return (true);
		}
	}
}