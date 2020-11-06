using seniorCapstone.Tables;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FieldListPage : ContentPage
	{
		public List<FieldTable> RunningFields { get; set; }

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

				// Query for the fields
				RunningFields = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND PivotRunning=1", App.UserID);

				List<FieldTable> remainingFields = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND PivotRunning=0", App.UserID);

				// If query fails then pop this page off the stack
				if (null == RunningFields || null == remainingFields)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding User Fields Failed");
				}
				else
				{
					runningListView.ItemsSource = RunningFields;
					remainingListView.ItemsSource = remainingFields;
				}
			}
		}

		async void OnRunningPivot_Selected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				List<FieldTable> selectedField = e.SelectedItem as List<FieldTable>;
				App.FieldID = selectedField[e.SelectedItemIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new RunningFieldPage());
			}
		}

		async void OnStoppedPivot_Selected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				List<FieldTable> selectedField = e.SelectedItem as List<FieldTable>;
				App.FieldID = selectedField[e.SelectedItemIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new StoppedFieldPage ());
			}
		}
	}
}