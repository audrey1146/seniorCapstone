/****************************************************************************
 * File			AddFieldViewmodel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Tables;
using SQLite;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class AddFieldViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		// Private Varibales
		private string fieldName = string.Empty;
		private string latitude = string.Empty;
		private string longitude = string.Empty;
		private int pivotIndex = -1;


		// Public Properties
		public ICommand AddFieldCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public IList<int> PivotOptions { get; set; }
		public string FieldName
		{
			get => this.fieldName;
			set
			{
				if (this.fieldName != value)
				{
					this.fieldName = value;
					OnPropertyChanged (nameof (this.FieldName));
				}
			}
		}
		public string Latitude
		{
			get => this.latitude;
			set
			{
				if (this.latitude != value)
				{
					this.latitude = value;
					OnPropertyChanged (nameof (this.Latitude));
				}
			}
		}
		public string Longitude
		{
			get => this.longitude;
			set
			{
				if (this.longitude != value)
				{
					this.longitude = value;
					OnPropertyChanged (nameof (this.Longitude));
				}
			}
		}
		public int PivotIndex
		{
			get => this.pivotIndex;
			set
			{
				if (this.pivotIndex != value)
				{
					this.pivotIndex = value;
					OnPropertyChanged (nameof (this.PivotIndex));
				}
			}
		}


		/// <summary>
		/// Constructor for the AddField VM. Sets the list of 
		/// viable pivot lengths
		/// </summary>
		public AddFieldViewModel ()
		{
			PivotOptions = new ObservableCollection<int>()
			{
				400,
				300,
				60
			};

			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.AddFieldCommand = new Command (this.AddFieldButton_Clicked);
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
			return (false == string.IsNullOrEmpty (this.FieldName) &&
					false == string.IsNullOrEmpty (this.Latitude) &&
					false == string.IsNullOrEmpty (this.Longitude) &&
					-1 != this.PivotIndex);
		}

		/// <summary>
		/// Command that will pop the current page off of the stack
		/// </summary>
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
		}

		/// <summary>
		/// Command that when pressed will attempt to add the field to the Field Table
		/// </summary>
		public async void AddFieldButton_Clicked ()
		{
			int returnValue = 0;

			if (true == this.AreEntiresFilledOut ())
			{
				FieldTable newField = new FieldTable ()
				{
					UID = App.UserID,
					FieldName = this.FieldName,
					PivotLength = this.PivotOptions[this.PivotIndex],
					Latitude = this.Latitude,
					Longitude = this.Longitude,
					PivotAngle = 0,
					PivotRunning = 0
				};

				// Insert into DB (using closes the DB for me)
				using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
				{
					dbConnection.CreateTable<FieldTable> ();

					returnValue = dbConnection.Insert (newField);

					if (1 == returnValue)
					{
						await Application.Current.MainPage.Navigation.PopModalAsync ();
					}
					else
					{
						await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Did Not Insert Correctly", "Ok");
					}
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Fill Out All Fields", "Ok");
			}
		}


		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
