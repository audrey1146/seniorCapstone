using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}


		/// <summary>
		/// When the login button is pressed, the input credntials must be checked.
		/// If they are correct, then login as the user, else display an alert
		/// </summary>
		public async void LoginButton_Clicked(object sender, System.EventArgs e)
		{
			if (true == areCredentialsCorrect(usernameEntry.Text, passwordEntry.Text))
			{
				App.IsUserLoggedIn = true;
				Navigation.InsertPageBefore(new MainPage(), this);
				await Navigation.PopAsync();
			}
			else
			{
				// Todo:  Come up with a better alert
				messageLabel.Text = "Login Failed";
				passwordEntry.Text = string.Empty;
			}
		}


		/// <summary>
		/// If the user wants to register an account then push on a Modal registration page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void RegistrationButton_Clicked(object sender, System.EventArgs e)
		{
			await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage());
		}


		/// <summary>
		/// Verifies whether the credentials the user input are correct by querying 
		/// into the user table.
		/// </summary>
		/// <param name="UserNameEntry"></param>
		/// <param name="PasswordEntry"></param>
		/// <returns></returns>
		private bool areCredentialsCorrect(string UserNameEntry, string PasswordEntry)
		{
			bool isUser = false;

			// Verify input by a query to the databse
			using (SQLiteConnection dbConnection = new SQLiteConnection(App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable>();

				// Query to get a single entry with specific Username and Password
				List<UserTable> userID = dbConnection.Query<UserTable>
					("SELECT UID FROM UserTable WHERE UserName=? AND Password=?",
					UserNameEntry, PasswordEntry);

				if (null != userID && userID.Count == 1)
				{
					isUser = true;
					App.UserID = userID[0].UID; // Other entries null because I am only returning the UID
				}
			}
			return isUser;
		}

	}
}