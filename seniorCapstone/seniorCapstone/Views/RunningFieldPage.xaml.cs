using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class RunningFieldPage : ContentPage
	{
		public RunningFieldPage ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the current user
				List<FieldTable> currentField = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND FID=?",
					App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == currentField || currentField.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding Current Field Failed");
				}
				else
				{
					fieldname.Text = currentField[0].FieldName;
					latitude.Text = currentField[0].Latitude;
					longitude.Text = currentField[0].Longitude;
					pivotlength.Text = currentField[0].PivotLength.ToString () + " meters";
				}
			}
		}

	}
}