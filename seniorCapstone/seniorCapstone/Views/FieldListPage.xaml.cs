using seniorCapstone.Services;
using seniorCapstone.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FieldListPage : ContentPage
	{
		// Private Variables
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;

		private IList<FieldTable> RunningFields;
		private IList<FieldTable> RemainingFields;

		// Public Properties
		public ObservableCollection<FieldTable> FieldEntries
		{
			get => this.fieldEntries;
			set
			{
				this.fieldEntries = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public FieldListPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			await this.LoadEntries ();

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.PivotRunning == false && field.UID == App.UserID)
				{
					this.RemainingFields.Add (field);
				}
				else if (field.PivotRunning == true && field.UID == App.UserID)
				{
					this.RunningFields.Add (field);
				}
			}

			this.RunningFields = this.RunningFields.OrderBy (x => x.FieldName).ToList ();
			this.RemainingFields = this.RemainingFields.OrderBy (x => x.FieldName).ToList ();

			runningListView.ItemsSource = this.RunningFields;
			remainingListView.ItemsSource = this.RemainingFields;
		}

		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async Task LoadEntries ()
		{
			try
			{
				var entries = await fieldDataService.GetEntriesAsync ();
				this.FieldEntries = new ObservableCollection<FieldTable> (entries);
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert ("Login Alert", ex.Message, "OK");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnRunningPivot_Selected (object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null)
			{
				App.FieldID = this.RunningFields[e.ItemIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new RunningFieldPage());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnStoppedPivot_Selected (object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null)
			{
				App.FieldID = this.RemainingFields[e.ItemIndex].FID;
				await Application.Current.MainPage.Navigation.PushAsync (new StoppedFieldPage ());
			}
		}





		/* LOST DUE TO MOVE TO AZURE AND NOT ENOUGH TIME TO IMPLEMENT ALL CRUD CAPABILITIES
		 
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
		
		 */

	}
}