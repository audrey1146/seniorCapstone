using seniorCapstone.Services;
using seniorCapstone.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();
		}


		/// <summary>
		/// 
		/// </summary>
		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			await this.LoadEntries ();
			await this.updateStoppedPivots ();

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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnDelete_Selected (object sender, EventArgs e)
		{
			FieldTable fieldMenuItem = ((MenuItem)sender).CommandParameter as FieldTable;

			bool answer = await App.Current.MainPage.DisplayAlert ("Delete Field?",
				"Are you sure you want to delete:  " + fieldMenuItem.FieldName + "? \nThis cannot be undone.",
				"Yes", "No");

			if (true == answer)
			{
				await this.fieldDataService.DeleteEntryAsync (fieldMenuItem);
				Debug.WriteLine ("Field Deleted");
				this.OnAppearing ();
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
				Debug.WriteLine ("Loading Fields Failed");
				Debug.WriteLine (ex.Message);
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
		}


		/// <summary>
		/// If any of the pivots are past their stop time then update the database
		/// </summary>
		private async Task updateStoppedPivots ()
		{
			/*
				YYYY MM DD hh:mm:ss
				DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7, 123);
				String.Format("{0:s}", dt);  // "2008-03-09T16:05:07"  SortableDateTime
			 */

			FieldTable updatedField = new FieldTable ();
			DateTime currentTime = DateTime.Now;
			DateTime stopTime;

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.PivotRunning == true && field.UID == App.UserID)
				{
					stopTime = DateTime.ParseExact (field.StopTime, "{0:s}", CultureInfo.InvariantCulture);

					if (DateTime.Compare (currentTime, stopTime) <= 0)
					{
						updatedField.assignTo (field);
						updatedField.PivotRunning = false;
						updatedField.StopTime = string.Empty;

						await this.fieldDataService.EditEntryAsync (updatedField);
						await Application.Current.MainPage.Navigation.PopAsync ();
					}
				}
			}
		}
	}
}