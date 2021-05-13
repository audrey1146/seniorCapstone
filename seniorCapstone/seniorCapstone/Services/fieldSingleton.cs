/****************************************************************************
 * File		FieldSingleton.cs
 * Author	Audrey Lincoln
 * Date		3/20/2021
 * Purpose	Singleton that has access to the field data service
 ****************************************************************************/

using seniorCapstone.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace seniorCapstone.Services
{
	public sealed class FieldSingleton
	{
		// Private Variables
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;

		private static readonly Lazy<FieldSingleton> lazy = new Lazy<FieldSingleton> (() => new FieldSingleton ());

		// Public 
		public static FieldSingleton Instance { get { return lazy.Value; } }

		//**************************************************************************
		// Function:	getSpecificField
		//
		// Description:	Get field based off of ID
		//
		// Parameters:	id	-	ID to search for
		//
		// Returns:		null if does not exist; otherwise the field
		//**************************************************************************
		public FieldTable getSpecificField (string id)
		{
			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.FID == id && field.UID == App.UserID)
				{
					return (field);
				}
			}

			return (null);
		}

		//**************************************************************************
		// Function:	getSpecificFieldByName
		//
		// Description:	Get field based off of the name and the current user
		//
		// Parameters:	fieldName	-	name to search for
		//
		// Returns:		null if does not exist; otherwise the field
		//**************************************************************************
		public FieldTable getSpecificFieldByName (string fieldName)
		{
			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.FieldName == fieldName && field.UID == App.UserID)
				{
					return (field);
				}
			}

			return (null);
		}


		//**************************************************************************
		// Function:	getAllUsersFields
		//
		// Description:	Get all of the fields of one user
		//
		// Parameters:	userID	-	ID to search for
		//
		// Returns:		null if does not exist; otherwise the fields
		//**************************************************************************
		public ObservableCollection<FieldTable> getAllUsersFields (string userID)
		{
			ObservableCollection<FieldTable> userFields = new ObservableCollection<FieldTable> ();

			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.UID == userID)
				{
					userFields.Add (field);
				}
			}

			return (userFields);
		}

		//**************************************************************************
		// Function:	getAllUsersStoppedFields
		//
		// Description:	Get all of the field names of one user that are stopped
		//
		// Parameters:	None
		//
		// Returns:		empty collection if does not exist; otherwise the field names
		//**************************************************************************
		public ObservableCollection<string> getAllUsersStoppedFields ()
		{
			ObservableCollection<string> stoppedFields = new ObservableCollection<string> ();

			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.PivotRunning == false && field.UID == App.UserID)
				{
					stoppedFields.Add (field.FieldName);
				}
			}

			return (stoppedFields);
		}

		//**************************************************************************
		// Function:	AddField
		//
		// Description:	Add a field entry to the database
		//
		// Parameters:	entry	-	Entry to be added
		//
		// Returns:		None
		//**************************************************************************
		public async Task AddField (FieldTable entry)
		{
			await this.fieldDataService.AddEntryAsync (entry);
			await this.ReloadFieldEntries ();
		}


		//**************************************************************************
		// Function:	DeleteField
		//
		// Description:	Delete a field entry from the database
		//
		// Parameters:	entry	-	Entry to be deleted
		//
		// Returns:		None
		//**************************************************************************
		public async Task DeleteField (FieldTable entry)
		{
			await this.fieldDataService.DeleteEntryAsync (entry);
			await this.ReloadFieldEntries ();
		}


		//**************************************************************************
		// Function:	DeleteAllFieldsOfUser
		//
		// Description:	Delete a all field entries from the database
		//
		// Parameters:	userID	-	User ID to delete fields from
		//
		// Returns:		None
		//**************************************************************************
		public async Task DeleteAllFieldsOfUser (string userID)
		{
			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.UID == userID)
				{
					await this.DeleteField (field);
				}
			}
		}


		//**************************************************************************
		// Function:	UpdateField
		//
		// Description:	Edit a field entry from the database
		//
		// Parameters:	entry	-	Entry to be edited
		//
		// Returns:		None
		//**************************************************************************
		public async Task UpdateField (FieldTable entry)
		{
			await this.fieldDataService.EditEntryAsync (entry);
			await this.ReloadFieldEntries ();
		}


		//**************************************************************************
		// Function:	ReloadFieldEntries
		//
		// Description:	Calls the API and loads the returned data into a member variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async Task ReloadFieldEntries ()
		{
			try
			{
				var entries = await fieldDataService.GetEntriesAsync ();
				this.fieldEntries = new ObservableCollection<FieldTable> (entries);
			}
			catch (Exception ex)
			{
				Debug.WriteLine ("Loading Accounts Failed");
				Debug.WriteLine (ex.Message);
			}
		}

		//**************************************************************************
		// Function:	DoesFieldNameExist
		//
		// Description:	Query the database to check whether the field name 
		//				already exists for the  current user
		//
		// Parameters:	FieldName	-	Name of the field to search for
		//
		// Returns:		True if exists, otherwise false
		//**************************************************************************
		public bool DoesFieldNameExist (string FieldName)
		{
			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.FieldName == FieldName && field.UID == App.UserID)
				{
					return (true);
				}
			}

			return (false);
		}

		//**************************************************************************
		// Function:	DoesFieldLocationExist
		//
		// Description:	Query the database to check whether the field location  
		//				already exists for the current user
		//
		// Parameters:	Lat		-	Latitude of the field
		//				Long	-	Longitude of the field
		//
		// Returns:		True if exists, otherwise false
		//**************************************************************************
		public bool DoesFieldLocationExist (string Lat, string Long)
		{
			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.Latitude == Lat && field.Longitude == Long)
				{
					return (true);
				}
			}

			return (false);
		}

		//**************************************************************************
		// Function:	updateStoppedPivots
		//
		// Description:	If any of the pivots are past their stop time then update the database
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async Task updateStoppedPivots ()
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

			foreach (FieldTable field in this.fieldEntries)
			{
				if (field.PivotRunning == true && field.UID == App.UserID)
				{
					stopTime = DateTime.ParseExact (field.StopTime, format, CultureInfo.InvariantCulture);

					if (DateTime.Compare (currentTime, stopTime) >= 0)
					{
						updatedField.assignTo (field);
						updatedField.PivotRunning = false;
						updatedField.StopTime = string.Empty;

						await this.UpdateField (updatedField);
						await this.fieldDataService.EditEntryAsync (updatedField);
						//await Application.Current.MainPage.Navigation.PopAsync ();
					}
				}
			}
		}


		//**************************************************************************
		// Constructor:	FieldSingleton
		//
		// Description:	Private constructor to set up access to API
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private FieldSingleton ()
		{
			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.fieldEntries = new ObservableCollection<FieldTable> ();

			this.LoadMediator ();
		}


		//**************************************************************************
		// Function:	LoadMediator
		//
		// Description:	Calls the API and loads the returned data into a member variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void LoadMediator ()
		{
			await this.LoadFieldEntries ();
		}


		//**************************************************************************
		// Function:	LoadFieldEntries
		//
		// Description:	Calls the API and loads the returned data into a member variable
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async Task LoadFieldEntries ()
		{
			try
			{
				var entries = await fieldDataService.GetEntriesAsync ();
				this.fieldEntries = new ObservableCollection<FieldTable> (entries);
			}
			catch (Exception ex)
			{
				Debug.WriteLine ("Loading Accounts Failed");
				Debug.WriteLine (ex.Message);
			}
		}
	}
}
