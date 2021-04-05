/****************************************************************************
 * File			StoppedFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding to display the information
 *				of a specific stopped field
 ****************************************************************************/

using Rg.Plugins.Popup.Services;
using seniorCapstone.Services;
using seniorCapstone.Tables;
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
		readonly IFieldDataService fieldDataService;
		ObservableCollection<FieldTable> fieldEntries;

		private FieldTable stoppedField = null;

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
			this.fieldDataService = new FieldApiDataService (new Uri ("https://evenstreaminfunctionapp.azurewebsites.net"));
			this.FieldEntries = new ObservableCollection<FieldTable> ();
			this.loadStoppedFieldInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
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
		/// Query the database for the selected field
		/// </summary>
		private async void loadStoppedFieldInfo ()
		{
			int count = 0;

			await this.LoadEntries ();

			foreach (FieldTable field in this.FieldEntries)
			{
				if (field.FID == App.FieldID && field.UID == App.UserID)
				{
					count++;
					this.StoppedField = field;
				}
			}

			if (count != 1)
			{
				await Application.Current.MainPage.Navigation.PopAsync ();
				Debug.WriteLine ("Finding Current Field Failed");
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
				await App.Current.MainPage.DisplayAlert ("Field Alert", ex.Message, "OK");
			}
		}


		/// <summary>
		/// Update the database then pop off the current page
		/// </summary>
		public async void RunPivotButton_Clicked ()
		{
			/*using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				List<FieldTable> runPivot = dbConnection.Query<FieldTable>
				("UPDATE FieldTable SET PivotRunning=1 WHERE UID=? AND FID=? AND PivotRunning=0", App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == runPivot)
				{
					Debug.WriteLine ("Running Pivot Failed From the Field Page");
					await Application.Current.MainPage.Navigation.PopAsync ();
				}
				else
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
				}
			}*/
		}

		
		

		




		/* LOST DUE TO MOVE TO AZURE AND NOT ENOUGH TIME TO IMPLEMENT ALL CRUD CAPABILITIES 
		 
		 
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
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 
		 */

	}
}
