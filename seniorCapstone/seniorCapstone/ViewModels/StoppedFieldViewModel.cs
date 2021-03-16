/****************************************************************************
 * File			StoppedFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding to display the information
 *				of a specific stopped field
 ****************************************************************************/

using Rg.Plugins.Popup.Services;
using seniorCapstone.Tables;
using seniorCapstone.Views;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class StoppedFieldViewModel : PageNavViewModel
	{
		// Private Variables
		private FieldTable stoppedField = null;

		// Public Properties
		public ICommand RunPivotCommand { get; set; }
		public ICommand EditFieldCommand { get; set; }
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
		}


		/// <summary>
		/// Update the database then pop off the current page
		/// </summary>
		public async void RunPivotButton_Clicked ()
		{
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
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
					//await Application.Current.MainPage.Navigation.PushAsync (new RunningFieldPage ());
				}
			}
		}

		public void EditFieldButton_Clicked ()
		{
			var popupPage = new EditStoppedFieldPopupPage ();

			// the method where you do whatever you want to after the popup is closed
			popupPage.CallbackEvent += (object sender, object e) => this.loadStoppedFieldInfo ();

			PopupNavigation.Instance.PushAsync (popupPage);
		}

		/// <summary>
		/// Query the database for the selected field
		/// </summary>
		private async void loadStoppedFieldInfo ()
		{
			// Reading from Database
			using (SQLiteConnection dbConnection = new SQLiteConnection (App.DatabasePath))
			{
				dbConnection.CreateTable<FieldTable> ();

				// Query for the current field
				List<FieldTable> currentField = dbConnection.Query<FieldTable>
					("SELECT * FROM FieldTable WHERE UID=? AND FID=?",
					App.UserID, App.FieldID);

				// If query fails then pop this page off the stack
				if (null == currentField || currentField.Count != 1)
				{
					await Application.Current.MainPage.Navigation.PopAsync ();
					Debug.WriteLine ("Finding Current Field Failed");
				}
				else
				{
					this.StoppedField = currentField[0];
				}
			}
		}
	}
}
