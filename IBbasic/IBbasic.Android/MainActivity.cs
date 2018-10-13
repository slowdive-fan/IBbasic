using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Content;
using Android.Util;

namespace IBbasic.Droid
{
    [Activity(Label = "IBbasic", Icon = "@drawable/ic_ibmini", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        public GameView gv;
        public static Activity ThisActivity { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            ThisActivity = this;

            base.OnCreate(bundle);

            this.RequestedOrientation = ScreenOrientation.Landscape;
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            View decorView = this.Window.DecorView;
            var uiOptions = (int)decorView.SystemUiVisibility;
            var newUiOptions = (int)uiOptions;
            newUiOptions |= (int)SystemUiFlags.LayoutStable;
            newUiOptions |= (int)SystemUiFlags.LayoutHideNavigation;
            newUiOptions |= (int)SystemUiFlags.LayoutFullscreen;
            newUiOptions |= (int)SystemUiFlags.HideNavigation;
            newUiOptions |= (int)SystemUiFlags.Fullscreen;
            newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;
            decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            Window.DecorView.SystemUiVisibilityChange += visibilityListener;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            SaveAndLoad_Android.GetGASInstance().Initialize_NativeGAS(this);

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            int dim1 = displayMetrics.WidthPixels;
            int dim2 = displayMetrics.HeightPixels;

            //int dim1 = (int)(Resources.DisplayMetrics.WidthPixels);
            //int dim2 = (int)(Resources.DisplayMetrics.HeightPixels);

            if (dim1 >= dim2)
            {
                App.ScreenWidth = dim1;
                App.ScreenHeight = dim2;
            }
            else
            {
                App.ScreenWidth = dim2;
                App.ScreenHeight = dim1;
            }
            App a = new App();

            LoadApplication(new App());
            
            //ask for permissions
            // Here, thisActivity is the current activity
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                // Should we show an explanation?
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
                {
                    // Show an explanation to the user *asynchronously* -- don't block
                    // this thread waiting for the user's response! After the user
                    // sees the explanation, try again to request the permission.
                    /*
                    new AlertDialog.Builder(this)
                            .SetIcon(Android.Resource.Drawable.IcDialogAlert)
                            .SetTitle("IBbasic needs READ/WRITE Permissions")
                            .SetMessage("IBbasic needs READ/WRITE Permissions to use the 'Create' feature (create your own adventures) and more. If you choose to deny permissions, the game will still work, but you will only be able to play the included modules.")
                            .Show();
                    */
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);

                }
                else
                {
                    // No explanation needed, we can request the permission.
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);
                    // MY_PERMISSIONS_REQUEST_READ_CONTACTS is an
                    // app-defined int constant. The callback method gets the
                    // result of the request.
                }

            }
        }

        private void visibilityListener(object sender, Android.Views.View.SystemUiVisibilityChangeEventArgs e)
        {
            var newUiOptions = (int)e.Visibility;
            newUiOptions |= (int)SystemUiFlags.LayoutStable;
            newUiOptions |= (int)SystemUiFlags.LayoutHideNavigation;
            newUiOptions |= (int)SystemUiFlags.LayoutFullscreen;
            newUiOptions |= (int)SystemUiFlags.HideNavigation;
            newUiOptions |= (int)SystemUiFlags.Fullscreen;
            newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            uiOptions |= (int)SystemUiFlags.LayoutStable;
            uiOptions |= (int)SystemUiFlags.LayoutHideNavigation;
            uiOptions |= (int)SystemUiFlags.LayoutFullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }
    }
}

