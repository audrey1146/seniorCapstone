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
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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


		/// <summary>
		/// Constructor that loads the field to be displayed then sets the change page 
		/// and run pivot commands
		/// </summary>
		public StoppedFieldViewModel ()
		{
			this.loadStoppedFieldInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
			this.EditFieldCommand = new Command (this.EditFieldButton_Clicked);
			this.ViewMapCommand = new Command (this.ViewMapButton_Clicked);
		}


		/// <summary>
		/// 
		/// </summary>
		public async void ViewMapButton_Clicked ()
		{
			await Application.Current.MainPage.Navigation.PushAsync (new ArcGISFieldMap ());
		}



		/// <summary>
		/// 
		/// </summary>
		public void EditFieldButton_Clicked ()
		{
			var popupPage = new EditStoppedFieldPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.loadStoppedFieldInfo ();

			PopupNavigation.Instance.PushAsync (popupPage);
		}


		/// <summary>
		/// Update the database then pop off the current page
		/// </summary>
		public async void RunPivotButton_Clicked ()
		{
			RainCat algorithmCalc = new RainCat ();
			FieldTable updatedField = new FieldTable (ref this.stoppedField);

			updatedField.PivotRunning = true;
			updatedField.StopTime = RainCat.TotalRunTime (ref this.stoppedField);

			await this.fieldBackend.UpdateField (updatedField);
			await Application.Current.MainPage.Navigation.PopAsync ();
		}


		/// <summary>
		/// Query the database for the selected field
		/// </summary>
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
