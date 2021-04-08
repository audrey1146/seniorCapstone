using seniorCapstone.Services;
using seniorCapstone.Models;
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
		// Private Variables
		private FieldSingleton fieldBackend = FieldSingleton.Instance;

		private List<FieldTable> RunningFields;
		private List<FieldTable> RemainingFields;

		/// <summary>
		/// 
		/// </summary>
		public FieldListPage()
		{
			InitializeComponent();

			this.RunningFields = new List<FieldTable>();
			this.RemainingFields = new List<FieldTable>();
		}


		/// <summary>
		/// 
		/// </summary>
		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			await this.fieldBackend.updateStoppedPivots ();

			ObservableCollection<FieldTable> fieldEntries = this.fieldBackend.getAllUsersFields (App.UserID);

			this.RunningFields.Clear ();
			this.RemainingFields.Clear ();

			foreach (FieldTable field in fieldEntries)
			{
				if (field.PivotRunning == true && field.UID == App.UserID)
				{
					this.RunningFields.Add (field);
				}
				else if (field.PivotRunning == false && field.UID == App.UserID)
				{
					this.RemainingFields.Add (field);
				}
			}

			if (this.RemainingFields != null)
			{
				this.RemainingFields = this.RemainingFields.OrderBy (x => x.FieldName).ToList ();
			}
			if (this.RunningFields != null)
			{
				this.RunningFields = this.RunningFields.OrderBy (x => x.FieldName).ToList ();
			}
			

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
				await this.fieldBackend.DeleteField (fieldMenuItem);
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
	}
}