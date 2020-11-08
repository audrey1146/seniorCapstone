using seniorCapstone.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FieldListPage : ContentPage
	{
		private IList<FieldTable> RunningFields;
		private IList<FieldTable> RemainingFields;

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
				else
				{
					this.RunningFields = this.RunningFields.OrderBy (x => x.FieldName).ToList ();
					this.RemainingFields = this.RemainingFields.OrderBy (x => x.FieldName).ToList ();

					runningListView.ItemsSource = this.RunningFields;
					remainingListView.ItemsSource = this.RemainingFields;
				}
			}
		}

		public async void OnRunningPivot_Selected (object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null)
			{
				App.FieldID = this.RunningFields[e.ItemIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new RunningFieldPage());
			}
		}

		public async void OnStoppedPivot_Selected (object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null)
			{
				App.FieldID = this.RemainingFields[e.ItemIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new StoppedFieldPage ());
			}
		}

		public async void OnDelete_Selected (object sender, EventArgs e)
		{
			FieldTable fieldMenuItem = ((MenuItem)sender).CommandParameter as FieldTable;

			bool answer = await App.Current.MainPage.DisplayAlert ("Delete Field?",
				"Are you sure you want to delete:  " +  fieldMenuItem.FieldName + "? \nThis cannot be undone.", 
				"Yes", "No");

			if (true == answer)
			{
				using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
				{
					dbConnection.CreateTable<FieldTable> ();

					List<FieldTable> deleteCheck = dbConnection.Query<FieldTable>
					("DELETE FROM FieldTable WHERE UID=? AND FID=?", App.UserID, fieldMenuItem.FID);

					deleteCheck = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND FID=?", App.UserID, fieldMenuItem.FID);

					if (null == deleteCheck || deleteCheck.Count == 0)
					{
						Debug.WriteLine ("Field Deleted");
						this.OnAppearing ();
					}
				}
			}
		}
	}
}