using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
		public RegistrationPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// When the user presses the button to register their data 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void RegisterButton_Clicked(object sender, System.EventArgs e)
		{
			int returnValue = 0;

			if (false == areEntiresFilledOut())
			{
				await DisplayAlert("Alert", "Please Fill Out All Fields", "OK");
			}
			else
			{
				UserTable newUser = new UserTable()
				{
					UserName = username.Text,
					Password = password.Text,
					FirstName = firstname.Text,
					LastName = lastname.Text,
					Email = email.Text

				};

				// Insert into DB (using closes the DB for me)
				using (SQLiteConnection dbConnection = new SQLiteConnection(App.DatabasePath))
				{
					dbConnection.CreateTable<UserTable>();

					// Before insert check that the unique values don't already exist
					List<UserTable> uniqueCheck = dbConnection.Query<UserTable>
						("SELECT * FROM UserTable WHERE UserName=? OR Email=?",
						username.Text, email.Text);

					if (null != uniqueCheck && uniqueCheck.Count == 0)
					{
						returnValue = dbConnection.Insert(newUser);
					}

					if (1 == returnValue)
					{
						await Application.Current.MainPage.Navigation.PopModalAsync();
					}
					else
					{
						await DisplayAlert("Alert", "Username or Email Already Exists", "OK");
					}
				}
			}		
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void CancelButton_Clicked (object sender, System.EventArgs e)
		{
			await Application.Current.MainPage.Navigation.PopModalAsync();
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
			return (false == string.IsNullOrEmpty(username.Text) &&
					false == string.IsNullOrEmpty(password.Text) &&
					false == string.IsNullOrEmpty(firstname.Text) &&
					false == string.IsNullOrEmpty(lastname.Text) &&
					false == string.IsNullOrEmpty(email.Text));
		}
	}
}