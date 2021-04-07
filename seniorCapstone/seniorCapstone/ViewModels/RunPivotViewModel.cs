/****************************************************************************
 * File			RunPivotViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the ability to run a pivot page
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using seniorCapstone.Helpers;
using seniorCapstone.Services;
using seniorCapstone.Models;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RunPivotViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Varibales
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;
		private int fieldIndex = -1;

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
		public ICommand RunPivotCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public IList<string> FieldOptions { get; set; }
		public int FieldIndex
		{
			get => this.fieldIndex;
			set
			{
				if (this.fieldIndex != value)
				{
					this.fieldIndex = value;
					OnPropertyChanged (nameof (this.FieldIndex));
				}
			}
		}


		/// <summary>
		/// Constructor that gets the viable fields, 
		/// then sets the cancel and run pivot commands.
		/// </summary>
		public RunPivotViewModel ()
		{
			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();

			this.FieldOptions = new ObservableCollection<string> ();
			this.GetPivots ();

			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
		}


		/// <summary>
		/// Gets a list of all of the non-running fields
		/// </summary>
		public async void GetPivots ()
		{
			await this.LoadEntries ();
			await this.updateStoppedPivots ();

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.PivotRunning == false && field.UID == App.UserID)
				{
					this.FieldOptions.Add (field.FieldName);
				}
			}
			this.FieldOptions = this.FieldOptions.OrderBy (q => q).ToList ();
		}


		/// <summary>
		/// Command that will pop the current page off of the stack
		/// </summary>
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
		}

		
		/// <summary>
		/// When the run pivot button is selected, update the databse
		/// and pop off the current page - otherwise display an alert
		/// </summary>
		public async void RunPivotButton_Clicked ()
		{
			if (true == this.AreEntiresFilledOut ())
			{
				RainCat algorithmCalc = new RainCat ();
				FieldTable updatedField = this.getSelectedField ();

				if (updatedField != null)
				{
					updatedField.PivotRunning = true;
					updatedField.StopTime = algorithmCalc.TotalRunTime (ref updatedField);

					await this.fieldDataService.EditEntryAsync (updatedField);
					await Application.Current.MainPage.Navigation.PopModalAsync ();
				}
				else
				{
					await App.Current.MainPage.DisplayAlert ("Run Pivot Alert", "Could Not Run Field", "Ok");
					await Application.Current.MainPage.Navigation.PopModalAsync ();
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Run Pivot Alert", "Choose a Field", "Ok");
			}
		}


		/// <summary>
		/// Invoked when a property changes to notify the view and viewmodel
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
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
				await Application.Current.MainPage.Navigation.PopModalAsync ();
			}
		}


		/// <summary>
		/// If any of the pivots are past their stop time then update the database
		/// </summary>
		private async Task updateStoppedPivots ()
		{
			/*
				YYYY MM DD hh:mm:ss
				DateTime dt = new DateTime(2008, 3, 9, 16, 5, 7);
				String.Format("{0:s}", dt);  // "2008-03-09T16:05:07"  SortableDateTime
			 */

			FieldTable updatedField = new FieldTable ();
			DateTime currentTime = DateTime.Now;
			DateTime stopTime;
			string format = "s";

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.PivotRunning == true && field.UID == App.UserID)
				{
					stopTime = DateTime.ParseExact (field.StopTime, format, CultureInfo.InvariantCulture);

					if (DateTime.Compare (currentTime, stopTime) >= 0)
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


		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		private bool AreEntiresFilledOut ()
		{
			return (-1 != this.FieldIndex);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private FieldTable getSelectedField ()
		{
			FieldTable selected = null;

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FieldName == this.FieldOptions[FieldIndex] 
					&& field.PivotRunning == false && field.UID == App.UserID)
				{
					selected = field;
				}
			}

			return (selected);
		}
	}
}
