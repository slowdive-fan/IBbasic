using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using IBbasic;
using Android.Util;

namespace LanternaExile.Droid
{
    [Activity(Label = "LanternaExile", Icon = "@drawable/ic_icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

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

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			
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

            App.fixedModule = "Lanterna";

            App a = new App();
			
            LoadApplication(new App());
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