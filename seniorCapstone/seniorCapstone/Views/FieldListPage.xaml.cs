using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FieldListPage : ContentPage
	{
		public FieldListPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the current user
				List<FieldTable> userFields = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=?", App.UserID);

				// If query fails then pop this page off the stack
				if (null == userFields)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding User Fields Failed");
				}
				else
				{
					fieldsListView.ItemsSource = userFields;
				}
			}
		}
	}
}