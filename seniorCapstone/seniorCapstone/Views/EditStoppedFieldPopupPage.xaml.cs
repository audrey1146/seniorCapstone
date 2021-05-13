﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Models;
using Xamarin.Forms.Xaml;
using seniorCapstone.Services;
using seniorCapstone.Helpers;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class EditStoppedFieldPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;

		// Private Variables
		private FieldSingleton fieldBackend = FieldSingleton.Instance;
		FieldTable singleField;


		//**************************************************************************
		// Constructor:	EditStoppedFieldPopupPage
		//
		// Description:	Initialize the soil and pivot selections
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public EditStoppedFieldPopupPage ()
		{
			InitializeComponent ();
			this.setPlaceholder ();

			soiltype.ItemsSource = (System.Collections.IList)Models.SoilModel.SoilNames;
			pivotlength.ItemsSource = (System.Collections.IList)Models.CenterPivotModel.PivotTypes;
		}

		//**************************************************************************
		// Function:	setPlaceholder
		//
		// Description:	Get the current field data to display as place holders
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		private async void setPlaceholder ()
		{
			this.singleField = this.fieldBackend.getSpecificField (App.FieldID);
			if (singleField == null)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await PopupNavigation.Instance.PopAsync (true);
			}
		}

		//**************************************************************************
		// Function:	SubmitButton_Clicked
		//
		// Description:	When the submit button is clicked this command will check update the 
		//				table entry according to the values they specified
		//
		// Parameters:	sender	-	Object that sent the message
		//				e		-	List View item that was tapped
		//
		// Returns:		None
		//**************************************************************************
		public async void SubmitButton_Clicked (object sender, EventArgs args)
		{
			FieldTable updatedField = new FieldTable (ref singleField);
			bool bPopOff = true;

			if (false == this.areEntriesFilled ())
			{
				await App.Current.MainPage.DisplayAlert ("Update Field Alert", "No New Entries", "OK");
				bPopOff = false;
			}
			else
			{
				// Update based on which entries got filled out
				if (false == string.IsNullOrEmpty (fieldname.Text))
				{
					// Verify Unique field name
					if (true == this.fieldBackend.DoesFieldNameExist (fieldname.Text))
					{
						await App.Current.MainPage.DisplayAlert ("Update Field Alert", "Field Name Already Exists", "OK");
						bPopOff = false;
					}
					else
					{
						updatedField.FieldName = fieldname.Text;
					}
				}
				if (-1 != pivotlength.SelectedIndex)
				{
					updatedField.PivotLength = (int)pivotlength.ItemsSource[pivotlength.SelectedIndex];
				}
				if (-1 != soiltype.SelectedIndex)
				{
					updatedField.SoilType = (string)soiltype.ItemsSource[soiltype.SelectedIndex];
				}
			}
			

			// If valid new field then edit existing
			if (true == bPopOff)
			{
				// If pivot length or soil type were changed need to redo equation
				if (-1 != pivotlength.SelectedIndex || -1 != soiltype.SelectedIndex)
				{
					updatedField.WaterUsage = RainCat.WaterUsage (ref updatedField);
				}

				await this.fieldBackend.UpdateField (updatedField);

				await PopupNavigation.Instance.PopAsync (true);
				CallbackEvent?.Invoke (this, EventArgs.Empty);
			}
		}


		//**************************************************************************
		// Function:	CancelButton_Clicked
		//
		// Description:	Popoff the popup page
		//
		// Parameters:	sender	-	Object that sent the message
		//				e		-	List View item that was tapped
		//
		// Returns:		None
		//**************************************************************************
		public async void CancelButton_Clicked (object sender, EventArgs args)
		{
			await PopupNavigation.Instance.PopAsync (true);
		}


		//**************************************************************************
		// Function:	areEntriesFilled
		//
		// Description:	Check that at least one field is filled
		//
		// Parameters:	None
		//
		// Returns:		True if filled out; otherwise false
		//**************************************************************************
		private bool areEntriesFilled ()
		{
			return (false == string.IsNullOrEmpty (fieldname.Text)
				|| -1 != pivotlength.SelectedIndex
				|| -1 != soiltype.SelectedIndex);
		}
	}
}