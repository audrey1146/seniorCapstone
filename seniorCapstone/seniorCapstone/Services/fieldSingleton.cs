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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
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


		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
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


		/// <summary>
		/// 
		/// </summary>
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


		/// <summary>
		/// 
		/// </summary>
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


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public async Task AddField (FieldTable entry)
		{
			await this.fieldDataService.AddEntryAsync (entry);
			await this.ReloadFieldEntries ();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public async Task DeleteField (FieldTable entry)
		{
			await this.fieldDataService.DeleteEntryAsync (entry);
			await this.ReloadFieldEntries ();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
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


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		public async Task UpdateField (FieldTable entry)
		{
			await this.fieldDataService.EditEntryAsync (entry);
			await this.ReloadFieldEntries ();
		}


		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
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


		/// <summary>
		/// Query the database to check whether the field name already exists for the 
		/// current user
		/// </summary>
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


		/// <summary>
		/// Query the database to check whether the field location already exists for the 
		/// current user
		/// </summary>
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


		/// <summary>
		/// If any of the pivots are past their stop time then update the database
		/// </summary>
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



		/// <summary>
		/// 
		/// </summary>
		private FieldSingleton ()
		{
			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.fieldEntries = new ObservableCollection<FieldTable> ();

			this.LoadMediator ();
		}

		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
		private async void LoadMediator ()
		{
			await this.LoadFieldEntries ();
		}

		/// <summary>
		/// Calls the API and loads the returned data into a member variable
		/// </summary>
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
