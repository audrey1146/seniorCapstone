using System;
using System.IO;
using Foundation;
using UIKit;

namespace seniorCapstone.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Platform Specific DB Path
            string dbFileName = "EvenStreamin_db.db3";
            string dbFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            string dbFullPath = Path.Combine(dbFileLocation, dbFileName);

            global::Xamarin.Forms.Forms.Init();

			// Use overloaded contructor
			LoadApplication (new App (dbFullPath));

            return base.FinishedLaunching(app, options);
        }
    }
}
