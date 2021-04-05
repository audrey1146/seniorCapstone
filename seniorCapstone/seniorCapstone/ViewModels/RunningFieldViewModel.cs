/****************************************************************************
 * File			RunningFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding to display the information
 *				of a specific running field
 ****************************************************************************/

using seniorCapstone.Services;
using seniorCapstone.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RunningFieldViewModel : PageNavViewModel
	{
		// Private Variables
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;
		private FieldTable runningField = null;

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
		public ICommand StopPivotCommand { get; set; }
		public FieldTable RunningField
		{
			get => this.runningField;
			set
			{
				if (this.runningField != value)
				{
					this.runningField = value;
					base.OnPropertyChanged (nameof (this.RunningField));
				}
			}
		}


		/// <summary>
		/// Get the information for the specific field to display,
		/// then set the change page and stop pivot commands
		/// </summary>
		public RunningFieldViewModel ()
		{
			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();
			this.loadRunningFieldInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.StopPivotCommand = new Command (this.StopPivotButton_Clicked);
		}


		/// <summary>
		/// Update the database and pop off the running page
		/// </summary>
		public async void StopPivotButton_Clicked ()
		{
			/*using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> runPivot = dbConnection.Query<FieldTable>
				("UPDATE FieldTable SET PivotRunning=0 WHERE UID=? AND FID=? AND PivotRunning=1", App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == runPivot)
				{
					Debug.WriteLine ("Stopping Pivot Failed From the Field Page");
					await Application.Current.MainPage.Navigation.PopAsync ();
				}
				else
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					//await Application.Current.MainPage.Navigation.PushAsync (new StoppedFieldPage ());
				}
			}*/
		}


		/// <summary>
		/// Queries that database for the specific field selected
		/// </summary>
		private async void loadRunningFieldInfo ()
		{
			int count = 0;

			await this.LoadEntries ();

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FID == App.FieldID && field.UID == App.UserID)
				{
					count++;
					this.RunningField = field;
				}
			}

			if (count != 1)
			{
				await Application.Current.MainPage.Navigation.PopAsync ();
				Debug.WriteLine ("Finding Current Field Failed");
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
				await App.Current.MainPage.DisplayAlert ("Field Alert", ex.Message, "OK");
			}
		}
	}
}
