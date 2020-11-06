using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class FieldListViewModel : PageNavViewModel
	{


		// Public Properties
		public ICommand RunningPivotPageCommand { get; set; }
		public ICommand StoppedPivotPageCommand { get; set; }
		public IList<FieldTable> RunningFields { get; set; }
		public IList<FieldTable> RemainingFields { get; set; }


		public FieldListViewModel()
		{
			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.RunningPivotPageCommand = new Command<int> (this.OnRunningPivot_Selected);
			this.StoppedPivotPageCommand = new Command<int> (this.OnStoppedPivot_Selected);
		}

		private async void OnAppearing ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the fields
				this.RunningFields = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND PivotRunning=1", App.UserID);

				this.RemainingFields = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND PivotRunning=0", App.UserID);

				// If query fails then pop this page off the stack
				if (null == this.RunningFields || null == this.RemainingFields)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding User Fields Failed");
				}
				/*else
				{
					runningListView.ItemsSource = RunningFields;
					remainingListView.ItemsSource = remainingFields;
				}*/
			}
		}

		public async void OnRunningPivot_Selected (int SelectedIndex)
		{
			if (SelectedIndex != -1)
			{
				App.FieldID = RunningFields[SelectedIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new RunningFieldPage ());
			}
		}

		public async void OnStoppedPivot_Selected (int SelectedIndex)
		{
			if (SelectedIndex != -1)
			{
				App.FieldID = RemainingFields[SelectedIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new StoppedFieldPage ());
			}
		}
	}
}
