using Rg.Plugins.Popup.Pages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditAccountPopupPage : PopupPage
	{
		public EditAccountPopupPage ()
		{
			InitializeComponent ();
			this.setPlaceholder ();
		}

		/// <summary>
		/// 
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
					firstname.Text = currentUser[0].FirstName;
					lastname.Text = currentUser[0].LastName;
					email.Text = currentUser[0].Email;
				}
			}
		}


		/// <summary>
		/// When the register button is clicked this command with verify that all fields are filled
		/// out, and that the email/username don't already exist
		/// </summary>
		/*public async void RegisterButton_Clicked ()
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
					FirstName = this.FirstName,
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
		}*/


	}
}