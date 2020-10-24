using seniorCapstone.Tables;
using SQLite;
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

		public void RegisterButton_Clicked(object sender, System.EventArgs e)
		{
			UserTable newUser = new UserTable()
			{
				UserName = username.Text,
				FirstName = firstname.Text,
				LastName = lastname.Text,
				Email = email.Text

			};

			// Insert into DB (using closes the DB for me)
			using (SQLiteConnection dbConnection = new SQLiteConnection(App.DatabasePath))
			{
				dbConnection.CreateTable<UserTable>();
				dbConnection.Insert(newUser);
			}
		}

		public async void CancelButton_Clicked(object sender, System.EventArgs e)
		{
			await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
		}
	}
}