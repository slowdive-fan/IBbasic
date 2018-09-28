using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using IBbasic;

namespace ElderinStone.Droid
{
    [Activity(Label = "ElderinStone", Icon = "@drawable/ic_icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            this.RequestedOrientation = ScreenOrientation.Landscape;
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
			SaveAndLoad_Android.GetGASInstance().Initialize_NativeGAS(this);

            int dim1 = (int)(Resources.DisplayMetrics.WidthPixels);
            int dim2 = (int)(Resources.DisplayMetrics.HeightPixels);

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

            App.fixedModule = "TheElderinStone";

            App a = new App();
			
			
			LoadApplication(new App());
        }
    }
}