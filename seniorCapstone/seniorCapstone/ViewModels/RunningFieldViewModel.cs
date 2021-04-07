/****************************************************************************
 * File			RunningFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding to display the information
 *				of a specific running field
 ****************************************************************************/

using seniorCapstone.Services;
using seniorCapstone.Models;
using System;
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
			FieldTable updatedField = new FieldTable (ref this.runningField);
			updatedField.PivotRunning = false;
			updatedField.StopTime = string.Empty;

			/*updatedField.FID = this.RunningField.FID;
			updatedField.UID = this.RunningField.UID;
			updatedField.FieldName = this.RunningField.FieldName;
			updatedField.PivotLength = this.RunningField.PivotLength;
			updatedField.SoilType = this.RunningField.SoilType;
			updatedField.Latitude = this.RunningField.Latitude;
			updatedField.Longitude = this.RunningField.Longitude;
			updatedField.PivotRunning = false;
			updatedField.StopTime = this.RunningField.StopTime;
			updatedField.WaterUsage = this.RunningField.WaterUsage;*/

			await this.fieldDataService.EditEntryAsync (updatedField);
			await Application.Current.MainPage.Navigation.PopAsync ();
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
				Debug.WriteLine ("Finding Current Field Failed");
				await Application.Current.MainPage.Navigation.PopAsync ();
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
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
		}
	}
}
