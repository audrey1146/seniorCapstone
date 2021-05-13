/****************************************************************************
 * File			RunningFieldViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding to display the information
 *				of a specific running field
 ****************************************************************************/

using seniorCapstone.Services;
using seniorCapstone.Models;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Globalization;

namespace seniorCapstone.ViewModels
{
	class RunningFieldViewModel : PageNavViewModel
	{
		// Private Variables
		private FieldSingleton fieldBackend = FieldSingleton.Instance;
		private FieldTable runningField = null;
		private string parsedStopTime;
		private string parsedWaterUsage;

		// Public Properties
		public ICommand StopPivotCommand { get; set; }
		public FieldTable RunningField
		{
			get => this.runningField;
			set
			{
				if (this.runningField != value)
				{
					this.runningField = value;
					base.OnPropertyChanged (nameof (this.RunningField));
				}
			}
		}
		public string ParsedStopTime
		{
			get => this.parsedStopTime;
			set
			{
				if (this.parsedStopTime != value)
				{
					this.parsedStopTime = value;
					base.OnPropertyChanged (nameof (this.parsedStopTime));
				}
			}
		}
		public string ParsedWaterUsage
		{
			get => this.parsedWaterUsage;
			set
			{
				if (this.parsedWaterUsage != value)
				{
					this.parsedWaterUsage = value;
					base.OnPropertyChanged (nameof (this.parsedWaterUsage));
				}
			}
		}

		//**************************************************************************
		// Constructor:	RunningFieldViewModel
		//
		// Description:	Get the information for the specific field to display,
		//				then set the change page and stop pivot commands
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public RunningFieldViewModel ()
		{
			this.loadRunningFieldInfo ();

			base.ChangePageCommand = new Command<string> (base.ChangePage);
			this.StopPivotCommand = new Command (this.StopPivotButton_Clicked);
		}

		//**************************************************************************
		// Function:	StopPivotButton_Clicked
		//
		// Description:	Update the database and pop off the running page
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void StopPivotButton_Clicked ()
		{
			FieldTable updatedField = new FieldTable (ref this.runningField);
			updatedField.PivotRunning = false;
			updatedField.StopTime = string.Empty;

			await this.fieldBackend.UpdateField (updatedField);
			await Application.Current.MainPage.Navigation.PopAsync ();
		}

		//**************************************************************************
		// Function:	loadRunningFieldInfo
		//
		// Description:	Queries that database for the specific field selected
		//
		// Parameters:	None
		//
		// Returns:		None	
		//**************************************************************************
		private async void loadRunningFieldInfo ()
		{
			this.RunningField = this.fieldBackend.getSpecificField (App.FieldID);
			if (this.RunningField == null)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await Application.Current.MainPage.Navigation.PopAsync ();
			}

			this.parseTime ();
			this.parseWaterUsage ();
		}


		//**************************************************************************
		// Function:	parseTime
		//
		// Description:	Parse out the time of the running pivot to display
		//
		// Parameters:	None
		//
		// Returns:		None	
		//**************************************************************************
		private void parseTime ()
		{
			string format = "s";
			DateTime stopTime = DateTime.ParseExact (this.RunningField.StopTime, format, CultureInfo.InvariantCulture);

			this.ParsedStopTime = String.Format ("{0:f}", stopTime);
		}


		//**************************************************************************
		// Function:	parseWaterUsage
		//
		// Description:	Parse out the water usage of the running pivot to display
		//
		// Parameters:	None
		//
		// Returns:		None	
		//**************************************************************************
		private void parseWaterUsage ()
		{
			double temp = Math.Round (this.RunningField.WaterUsage, 3);
			this.ParsedWaterUsage = temp.ToString ();
		}
	}
}
