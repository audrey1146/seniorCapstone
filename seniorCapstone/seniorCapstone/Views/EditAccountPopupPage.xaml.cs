using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditAccountPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;


		public EditAccountPopupPage ()
		{
			InitializeComponent ();
			this.setPlaceholder ();
		}

		/// <summary>
		/// Get the current user data to display as place holders
		/// </summary>
		private async void setPlaceholder ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable> ();

				// Query for the current user
				List<UserTable> currentUser = dbConnection.Query<UserTable>
					("SELECT * FROM UserTable WHERE UID=?", App.UserID);

				// If query fails then pop this page off the stack
				if (null == currentUser || currentUser.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding Current User Failed");
				}
				else
				{
					firstname.Placeholder = currentUser[0].FirstName;
					lastname.Placeholder = currentUser[0].LastName;
					email.Placeholder = currentUser[0].Email;
				}
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
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable> ();


				// Update based on which entries got filled out
				if (false == string.IsNullOrEmpty (password.Text))
				{
					List<UserTable> updateFirstName = dbConnection.Query<UserTable>
						("UPDATE UserTable SET Password=? WHERE UID=?",
						password.Text, App.UserID);
				}
				if (false == string.IsNullOrEmpty (firstname.Text))
				{
					List<UserTable> updateFirstName = dbConnection.Query<UserTable>
						("UPDATE UserTable SET FirstName=? WHERE UID=?",
						firstname.Text, App.UserID);
				}
				if (false == string.IsNullOrEmpty (lastname.Text))
				{
					List<UserTable> updateLastName = dbConnection.Query<UserTable>
						("UPDATE UserTable SET LastName=? WHERE UID=?",
						lastname.Text, App.UserID);
				}
				if (false == string.IsNullOrEmpty (email.Text))
				{
					// Before insert check that the unique values don't already exist
					List<UserTable> uniqueCheck = dbConnection.Query<UserTable>
						("SELECT * FROM UserTable WHERE Email=?",
						email.Text);

					if (null != uniqueCheck && uniqueCheck.Count == 0)
					{
						List<UserTable> updateEmail = dbConnection.Query<UserTable>
							("UPDATE UserTable SET Email=? WHERE UID=?",
							email.Text, App.UserID);
					}
					else
					{
						await App.Current.MainPage.DisplayAlert ("UpdateAccount Alert", "Email Already Exists", "OK");
					}
				}
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
	}
}