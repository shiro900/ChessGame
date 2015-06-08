#if __ANDROID__
using System;
using System.Diagnostics;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Util;

namespace ChessGame
{
    [Activity(
        Label = "ChessGame" , 
        MainLauncher = true , 
        Icon = "@drawable/icon" , 
        Theme = "@style/Theme.Splash" , 
        AlwaysRetainTaskState = true , 
        LaunchMode = Android.Content.PM.LaunchMode.SingleInstance ,
        ScreenOrientation = ScreenOrientation.SensorLandscape , 
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
        )]
    public class ChessActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Get screen resolution
            DisplayMetrics metrics = new DisplayMetrics();
            Window.WindowManager.DefaultDisplay.GetMetrics(metrics);
            Resolution.ScreenWidth = metrics.WidthPixels;
            Resolution.ScreenHeight = metrics.HeightPixels;

            //Remove title bar request
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);
            ChessGame game = new ChessGame();
            SetContentView((View)game.Services.GetService(typeof(View)));
            game.Run();
        }
    }
}
#endif