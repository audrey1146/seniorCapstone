/****************************************************************************
 * File			AddFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the Add Field functionality
 ****************************************************************************/

using System.Collections.Generic;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Services;
using seniorCapstone.Models;
using seniorCapstone.Views;
using Xamarin.Forms;
using seniorCapstone.Helpers;

namespace seniorCapstone.ViewModels
{
	class AddFieldViewModel : Helpers.Geolocation
	{
		//Private Varibales
		private FieldSingleton fieldBackend = FieldSingleton.Instance;
		private string fieldName = string.Empty;
		private int pivotIndex = -1;
		private int soilIndex = -1;

		// Public Properties
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

		//**************************************************************************
		// Constructor:	AddFieldViewModel
		//
		// Description:	Constructor for the AddField VM. Sets the list of 
		//				viable pivot lengths
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public AddFieldViewModel ()
		{
			PivotOptions = (IList<int>)Models.CenterPivotModel.PivotTypes;
			SoilOptions = (IList<string>)Models.SoilModel.SoilNames;

			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.AddFieldCommand = new Command (this.AddFieldButton_Clicked);
			this.SyncToPanelCommand = new Command (this.SyncToPanelButton_Clicked);
		}

		//**************************************************************************
		// Function:	CancelButton_Clicked
		//
		// Description:	Command that will pop the current page off of the stack
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void CancelButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();
		}

		//**************************************************************************
		// Function:	AddFieldButton_Clicked
		//
		// Description:	Command will attempt to add the field to the Field Table
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void AddFieldButton_Clicked ()
		{
			if (true == this.areEntiresFilledOut ())
			{
				if (true == this.fieldBackend.DoesFieldNameExist (this.FieldName))
				{
					await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Field Name Already Exists", "Ok");
				}
				else if (true == this.fieldBackend.DoesFieldLocationExist (this.Latitude, this.Longitude))
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
						PivotRunning = false,
						StopTime = string.Empty,
						WaterUsage = 0
					};

					// Calls RainCat to set the water usage
					newField.WaterUsage = RainCat.WaterUsage (ref newField);

					await this.fieldBackend.AddField (newField);
					await Application.Current.MainPage.Navigation.PopModalAsync ();
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Add Field Alert", "Fill out entire form to add field", "Ok");
			}
		}

		//**************************************************************************
		// Function:	SyncToPanelButton_Clicked
		//
		// Description:	Creates a popup to appear, with a callback method to get the users location
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public void SyncToPanelButton_Clicked ()
		{
			var popupPage = new SyncPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.getUserLocation ();

			PopupNavigation.Instance.PushAsync (popupPage);
		}

		//**************************************************************************
		// Function:	areEntiresFilledOut
		//
		// Description:	Verfies that the user input data for all entries
		//
		// Parameters:	None
		//
		// Returns:		True if all of the entries have some text in them, 
		//				otherwise false  if any are empty or null
		//**************************************************************************
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
