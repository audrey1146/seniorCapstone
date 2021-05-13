using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace seniorCapstone.Views
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class SyncPopupPage : PopupPage
	{
		// event callback
		public event EventHandler<object> CallbackEvent;
		//protected override void OnDisappearing () => CallbackEvent?.Invoke (this, EventArgs.Empty);

		public SyncPopupPage ()
		{
			InitializeComponent ();
		}

		//**************************************************************************
		// Function:	SyncButton_Clicked
		//
		// Description:	Close the popup so that the main viewmodel can register the location
		//
		// Parameters:	None
		//
		// Returns:		None
		//**************************************************************************
		public async void SyncButton_Clicked (object sender, EventArgs args)
		{
			await PopupNavigation.Instance.PopAsync (true);

			// Callback is only invoked when the user presses the sync button
			CallbackEvent?.Invoke (this, EventArgs.Empty);
		}
	}
}