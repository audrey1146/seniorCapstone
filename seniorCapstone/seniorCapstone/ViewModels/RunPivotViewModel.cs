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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using seniorCapstone.Services;
using seniorCapstone.Tables;
using SQLite;
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

			// TODO		CHECK ALL RUNNING FIELDS AND STOP IF PAST END TIME

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

			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the fields
				List <FieldTable> FieldList = dbConnection.Query<FieldTable>
					("SELECT FieldName FROM FieldTable WHERE UID=? AND PivotRunning=0", App.UserID);

				// If query fails then pop this page off the stack
				if (null == FieldList)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding User Fields Failed");
				}

				for (int i = 0; i < FieldList.Count; i++)
				{
					this.FieldOptions.Add (FieldList[i].FieldName);
				}
				this.FieldOptions = this.FieldOptions.OrderBy (q => q).ToList ();
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
				await App.Current.MainPage.DisplayAlert ("Login Alert", ex.Message, "OK");
			}
		}


		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		public bool AreEntiresFilledOut ()
		{
			return (-1 != this.FieldIndex);
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
				using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
				{
					dbConnection.CreateTable<FieldTable> ();

					List<FieldTable> runPivot = dbConnection.Query<FieldTable>
						("UPDATE FieldTable SET PivotRunning=1 WHERE UID=? AND FieldName=? AND PivotRunning=0", 
						App.UserID, this.FieldOptions[FieldIndex]);

					// If query fails then pop this page off the stack
					if (null == runPivot)
					{
						Debug.WriteLine ("Running Pivot Failed");
					}
				}
				await Application.Current.MainPage.Navigation.PopModalAsync ();
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

	}
}
