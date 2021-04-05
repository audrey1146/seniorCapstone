/****************************************************************************
 * File			AddFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Add Field functionality
 ****************************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Services;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class AddFieldViewModel : Helpers.Geolocation
	{
		//Private Varibales
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;

		private string fieldName = string.Empty;
		private int pivotIndex = -1;
		private int soilIndex = -1;

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

		public ICommand AddFieldCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand SyncToPanelCommand { get; set; }
		public IList<int> PivotOptions { get; set; }
		public IList<string> SoilOptions { get; set; }
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
		public int SoilIndex
		{
			get => this.soilIndex;
			set
			{
				if (this.soilIndex != value)
				{
					this.soilIndex = value;
					this.OnPropertyChanged (nameof (this.SoilIndex));
				}
			}
		}

		/// <summary>
		/// Constructor for the AddField VM. Sets the list of 
		/// viable pivot lengths
		/// </summary>
		public AddFieldViewModel ()
		{
			PivotOptions = (IList<int>)Helpers.CenterPivotSpecs.PivotTypes;
			SoilOptions = (IList<string>)Helpers.CenterPivotSpecs.SoilTypes;

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
						SoilType = this.SoilOptions[this.SoilIndex],
						Latitude = this.Latitude,
						Longitude = this.Longitude,
						PivotAngle = 0,
						PivotRunning = 0
					};

					await this.fieldDataService.AddEntryAsync (newField);
					await Application.Current.MainPage.Navigation.PopModalAsync ();
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
			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FieldName == this.FieldName)
				{
					return (false);
				}
			}

			return (true);
		}


		/// <summary>
		/// Query the database to check whether the field location already exists for the 
		/// current user
		/// </summary>
		private bool doesFieldLocationExist ()
		{
			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.Latitude == this.Latitude && field.Longitude == this.Longitude)
				{
					return (false);
				}
			}

			return (true);
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
					-1 != this.PivotIndex &&
					-1 != this.SoilIndex);
		}
	}
}
