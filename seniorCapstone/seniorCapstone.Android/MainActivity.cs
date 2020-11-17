using Android.App;
using Android.Content.PM;
using Android.OS;
using System.IO;

namespace seniorCapstone.Droid
{
    [Activity(Label = "seniorCapstone", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Instance = this;

            // Platform Specific DB Path
            string dbFileName = "EvenStreamin_db.db3";
            string dbFileLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbFullPath = Path.Combine(dbFileLocation, dbFileName);

            Rg.Plugins.Popup.Popup.Init (this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Using Overloaded Constructor
            LoadApplication(new App(dbFullPath));
        }


        public override void OnRequestPermissionsResult (int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult (requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult (requestCode, permissions, grantResults);
        }


        #region LocationDisplay
        /*

        private const int LocationPermissionRequestCode = 99;
        private const int LocationRequesNoMap = 97;

        private Esri.ArcGISRuntime.Location.LocationDataSource _lastUsedLocationDataSource;
        private TaskCompletionSource<bool> _permissionTCS;

        public async Task<bool> AskForLocationPermission ()
        {
            if (ContextCompat.CheckSelfPermission (this, LocationService) != Permission.Granted)
            {
                _permissionTCS = new TaskCompletionSource<bool> ();
                RequestPermissions (new[] { Manifest.Permission.AccessFineLocation }, LocationRequesNoMap);
                return await _permissionTCS.Task;
            }
            else return true;
        }

        public async void AskForLocationPermission (Esri.ArcGISRuntime.Location.LocationDataSource myLocationDataSource)
        {
            // Save the mapview for later.
            _lastUsedLocationDataSource = myLocationDataSource;

            // Only check if permission hasn't been granted yet.
            if (ContextCompat.CheckSelfPermission (this, LocationService) != Permission.Granted)
            {
                // Show the standard permission dialog.
                // Once the user has accepted or denied, OnRequestPermissionsResult is called with the result.
                RequestPermissions (new[] { Manifest.Permission.AccessFineLocation }, LocationPermissionRequestCode);
            }
            else
            {
                try
                {
                    // Explicit DataSource.LoadAsync call is used to surface any errors that may arise.
                    await myLocationDataSource.StartAsync ();
                    //myMapView.LocationDisplay.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine (ex);
                    ShowMessage (ex.Message, "Failed to start location display.");
                }
            }
        }

        public override async void OnRequestPermissionsResult (int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == LocationPermissionRequestCode)
            {
                // If the permissions were granted, enable location.
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted && _lastUsedLocationDataSource != null)
                {
                    System.Diagnostics.Debug.WriteLine ("User affirmatively gave permission to use location. Enabling location.");
                    try
                    {
                        // Explicit DataSource.LoadAsync call is used to surface any errors that may arise.
                        //await _lastUsedMapView.LocationDisplay.DataSource.StartAsync ();
                        //_lastUsedMapView.LocationDisplay.IsEnabled = true;
                        await _lastUsedLocationDataSource.StartAsync ();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine (ex);
                        ShowMessage (ex.Message, "Failed to start location display.");
                    }
                }
                else
                {
                    ShowMessage ("Location permissions not granted.", "Failed to start location display.");
                }

                // Reset the mapview.
                _lastUsedLocationDataSource = null;
            }
            else if (requestCode == LocationRequesNoMap)
            {
                _permissionTCS.TrySetResult (grantResults.Length == 1 && grantResults[0] == Permission.Granted);
            }
        }

        private void ShowMessage (string message, string title = "Error") => new AlertDialog.Builder (this).SetTitle (title).SetMessage (message).Show ();
        */
        #endregion LocationDisplay
    }
}