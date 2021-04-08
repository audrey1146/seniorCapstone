using Rg.Plugins.Popup.Pages;
using System;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using seniorCapstone.Models;
using Xamarin.Forms.Xaml;
using seniorCapstone.Services;

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


		/// <summary>
		/// 
		/// </summary>
		public EditStoppedFieldPopupPage ()
		{
			InitializeComponent ();
			this.setPlaceholder ();

			soiltype.ItemsSource = (System.Collections.IList)Models.SoilModel.SoilNames;
			pivotlength.ItemsSource = (System.Collections.IList)Models.CenterPivotModel.PivotTypes;
		}


		/// <summary>
		/// Get the current user data to display as place holders
		/// </summary>
		private async void setPlaceholder ()
		{
			this.singleField = this.fieldBackend.getSpecificField (App.FieldID);
			if (singleField == null)
			{
				Debug.WriteLine ("Finding Current Field Failed");
				await PopupNavigation.Instance.PopAsync (true);


				//await Application.Current.MainPage.Navigation.PopAsync ();
				//Debug.WriteLine ("Finding Current Field Failed");
				//await PopupNavigation.Instance.PopAsync (true);
			}
		}


		/// <summary>
		/// When the submit button is clicked this command will check update the 
		/// table entry according to the values they specified
		/// </summary>
		/// /// <param name="sender"></param>
		/// <param name="args"></param>
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
					// TODO call RainCat to set the WaterUsage
					// updatedField.WaterUsage = RainCat.WaterUsage (ref updatedField);
				}

				await this.fieldBackend.UpdateField (updatedField);

				await PopupNavigation.Instance.PopAsync (true);
				CallbackEvent?.Invoke (this, EventArgs.Empty);
			}
		}


		/// <summary>
		/// Pop off the popup page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public async void CancelButton_Clicked (object sender, EventArgs args)
		{
			await PopupNavigation.Instance.PopAsync (true);
		}


		/// <summary>
		/// Check that at least one field is filled
		/// </summary>
		private bool areEntriesFilled ()
		{
			return (false == string.IsNullOrEmpty (fieldname.Text)
				|| -1 != pivotlength.SelectedIndex
				|| -1 != soiltype.SelectedIndex);
		}
	}
}