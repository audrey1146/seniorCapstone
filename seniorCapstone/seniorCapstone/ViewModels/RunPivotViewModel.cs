/****************************************************************************
 * File			RunPivotViewModel.cs
 * Author		Audrey Lincoln
 * Date			10/30/2020
 * Purpose		Functions and binding for the ability to run a pivot page
 ****************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using seniorCapstone.Helpers;
using seniorCapstone.Services;
using seniorCapstone.Models;
using Xamarin.Forms;

namespace seniorCapstone.ViewModels
{
	class RunPivotViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Private Varibales
		private FieldSingleton fieldBackend = FieldSingleton.Instance;
		private int fieldIndex = -1;

		// Public Properties
		public ICommand RunPivotCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public IList<string> FieldOptions { get; set; }
		public int FieldIndex
		{
			get => this.fieldIndex;
			set
			{
				if (this.fieldIndex != value)
				{
					this.fieldIndex = value;
					OnPropertyChanged (nameof (this.FieldIndex));
				}
			}
		}

		//**************************************************************************
		// Function:	RunPivotViewModel
		//
		// Description:	Constructor that gets the viable fields, 
		//				then sets the cancel and run pivot commands.
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public RunPivotViewModel ()
		{
			this.GetPivots ();

			this.CancelCommand = new Command (this.CancelButton_Clicked);
			this.RunPivotCommand = new Command (this.RunPivotButton_Clicked);
		}

		//**************************************************************************
		// Function:	GetPivots
		//
		// Description:	Gets a list of all of the non-running fields
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void GetPivots ()
		{
			await this.fieldBackend.updateStoppedPivots ();
			this.FieldOptions = (IList<string>)this.fieldBackend.getAllUsersStoppedFields ();
			this.FieldOptions = this.FieldOptions.OrderBy (q => q).ToList ();
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
		// Function:	RunPivotButton_Clicked
		//
		// Description:	When the run pivot button is selected, update the databse
		//				and pop off the current page - otherwise display an alert
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void RunPivotButton_Clicked ()
		{
			if (true == this.AreEntiresFilledOut ())
			{
				RainCat algorithmCalc = new RainCat ();
				FieldTable updatedField = this.fieldBackend.getSpecificFieldByName (this.FieldOptions[this.FieldIndex]);

				if (updatedField != null)
				{
					updatedField.PivotRunning = true;
					updatedField.StopTime = RainCat.TotalRunTime (ref updatedField);

					await this.fieldBackend.UpdateField (updatedField);
					await Application.Current.MainPage.Navigation.PopModalAsync ();
				}
				else
				{
					await App.Current.MainPage.DisplayAlert ("Run Pivot Alert", "Could Not Run Field", "Ok");
					await Application.Current.MainPage.Navigation.PopModalAsync ();
				}
			}
			else
			{
				await App.Current.MainPage.DisplayAlert ("Run Pivot Alert", "Choose a Field", "Ok");
			}
		}

		//**************************************************************************
		// Function:	OnPropertyChanged
		//
		// Description:	Invoked when a property changes to notify the view and viewmodel
		//
		// Parameters:	propertyName	-	Name of the property changed
		//
		// Returns:		None
		//**************************************************************************
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}

		//**************************************************************************
		// Function:	AreEntiresFilledOut
		//
		// Description:	Verfies that the user input data for all entries
		//
		// Parameters:	None
		//
		// Returns:		True if all of the entries have some text in them, otherwise false 
		//				if any are empty or null
		//**************************************************************************
		private bool AreEntiresFilledOut ()
		{
			return (-1 != this.FieldIndex);
		}
	}
}
