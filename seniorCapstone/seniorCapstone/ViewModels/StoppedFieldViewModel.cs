/****************************************************************************
 * File			StoppedFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding to display the information
 *				of a specific stopped field
 ****************************************************************************/

using Rg.Plugins.Popup.Services;
using seniorCapstone.Helpers;
using seniorCapstone.Services;
using seniorCapstone.Models;
using seniorCapstone.Views;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class StoppedFieldViewModel : PageNavViewModel
	{
		// Private Variables
		private FieldSingleton fieldBackend = FieldSingleton.Instance;

		private FieldTable stoppedField = null;

		// Public Properties
		public ICommand RunPivotCommand { get; set; }
		public ICommand EditFieldCommand { get; set; }
		public ICommand ViewMapCommand { get; set; }
		public FieldTable StoppedField
		{
			get => this.stoppedField;
			set
			{
				if (this.stoppedField != value)
				{
					this.stoppedField = value;
					base.OnPropertyChanged (nameof (this.StoppedField));
				}
			}
		}


		//**************************************************************************
		// Constructor:	StoppedFieldViewModel
		//
		// Description:	Constructor that loads the field to be displayed 
		//				then sets the change page and run pivot commands
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public StoppedFieldViewModel ()
		{
			this.loadStoppedFieldInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
			this.EditFieldCommand = new Command (this.EditFieldButton_Clicked);
			this.ViewMapCommand = new Command (this.ViewMapButton_Clicked);
		}


		//**************************************************************************
		// Function:	ViewMapButton_Clicked
		//
		// Description:	Push on the ArcGIS map view
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void ViewMapButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PushAsync (new ArcGISFieldMap ());
		}



		//**************************************************************************
		// Function:	EditFieldButton_Clicked
		//
		// Description:	Bring up a popup to edit the field
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public void EditFieldButton_Clicked ()
		{
			var popupPage = new EditStoppedFieldPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.loadStoppedFieldInfo ();

			PopupNavigation.Instance.PushAsync (popupPage);
		}

		//**************************************************************************
		// Function:	RunPivotButton_Clicked
		//
		// Description:	Update the database then pop off the current page
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void RunPivotButton_Clicked ()
		{
			RainCat algorithmCalc = new RainCat ();
			FieldTable updatedField = new FieldTable (ref this.stoppedField);

			updatedField.PivotRunning = true;
			updatedField.StopTime = RainCat.TotalRunTime (ref this.stoppedField);

			await this.fieldBackend.UpdateField (updatedField);
			await Application.Current.MainPage.Navigation.PopAsync ();
		}

		//**************************************************************************
		// Function:	loadStoppedFieldInfo
		//
		// Description:	Query the database for the selected field
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void loadStoppedFieldInfo ()
		{
			this.StoppedField = this.fieldBackend.getSpecificField (App.FieldID);
			if (this.StoppedField == null)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await Application.Current.MainPage.Navigation.PopAsync ();
			}
		}
	}
}
