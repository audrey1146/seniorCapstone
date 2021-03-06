﻿/****************************************************************************
 * File			AddFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Add Field functionality
 ****************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class AddFieldViewModel : Helpers.Geolocation
	{
		//Private Varibales
		private string fieldName = string.Empty;
		private int pivotIndex = -1;

		// Public Properties
		public ICommand AddFieldCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand SyncToPanelCommand { get; set; }
		public IList<int> PivotOptions { get; set; }
		public string FieldName
		{
			get => this.fieldName;
			set
			{
				if (this.fieldName != value)
				{
					this.fieldName = value;
					this.OnPropertyChanged (nameof (this.FieldName));
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
					this.OnPropertyChanged (nameof (this.PivotIndex));
				}
			}
		}


		/// <summary>
		/// Constructor for the AddField VM. Sets the list of 
		/// viable pivot lengths
		/// </summary>
		public AddFieldViewModel ()
		{
			PivotOptions = new ObservableCollection<int> ()
			{
				400,
				300,
				60
			};
			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.AddFieldCommand = new Command (this.AddFieldButton_Clicked);
			this.SyncToPanelCommand = new Command (this.SyncToPanelButton_Clicked);
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

			if (true == this.areEntiresFilledOut ())
			{
				if (true == doesFieldNameExist ())
				{
					await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Field Name Already Exists", "Ok");
				}
				else if (true == doesFieldLocationExist ())
				{
					await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Field Location Already Exists", "Ok");
				}
				else
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
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Fill out entire form to add field", "Ok");
			}
		}


		/// <summary>
		/// Creates a popup to appear, with a callback method to get the users location
		/// </summary>
		public void SyncToPanelButton_Clicked ()
		{
			var popupPage = new SyncPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.getUserLocation ();

			PopupNavigation.Instance.PushAsync (popupPage);
		}


		/// <summary>
		/// Query the database to check whether the field name already exists for the 
		/// current user
		/// </summary>
		private bool doesFieldNameExist ()
		{
			bool isField = false;

			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Make sure the name doesn't already exist
				List<FieldTable> fieldID = dbConnection.Query<FieldTable>
				("SELECT FID FROM FieldTable WHERE FieldName=? AND UID=?",
				this.FieldName, App.UserID);

				if (null == fieldID || fieldID.Count != 0)
				{
					isField = true;
				}
			}
			return isField;
		}


		/// <summary>
		/// Query the database to check whether the field location already exists for the 
		/// current user
		/// </summary>
		private bool doesFieldLocationExist ()
		{
			bool isField = false;

			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> fieldID = dbConnection.Query<FieldTable>
				("SELECT FID FROM FieldTable WHERE Latitude=? AND Longitude=? AND UID=?",
				this.Latitude, this.Longitude, App.UserID);

				if (null == fieldID || fieldID.Count != 0)
				{
					isField = true;
				}
			}
			return isField;
		}


		/// <summary>
		/// Verfies that the user input data for all entries
		/// </summary>
		/// <returns>
		/// True if all of the entries have some text in them, otherwise false 
		/// if any are empty or null
		/// </returns>
		private bool areEntiresFilledOut ()
		{
			return (false == string.IsNullOrEmpty (this.FieldName) &&
					false == string.IsNullOrEmpty (this.Latitude) &&
					false == string.IsNullOrEmpty (this.Longitude) &&
					-1 != this.PivotIndex);
		}

	}
}
