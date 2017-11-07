using Android.App;
using Android.Graphics.Drawables;
using Android.Widget;
using System;
using Xamarin.Forms.Platform.Android;

namespace Noter.Droid.Helpers
{
    public static class AlertDialogHelper
    {
        /// <summary>
        /// Displays an alert
        /// </summary>
        /// <param name="activity">The Activity to display the alert in</param>
        /// <param name="message">The message to display within the alert dialog</param>
        public static void DisplayAlert(Activity activity, string title, string message)
        {
            activity.RunOnUiThread(() =>
            {
                var titleView = new TextView(activity)
                {
                    Text = title,
                    TextSize = 24,
                    Gravity = Android.Views.GravityFlags.Center
                };
                titleView.SetTextColor(Xamarin.Forms.Color.White.ToAndroid());
                titleView.SetPadding(5, 5, 5, 2);
                titleView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.indicator_input_error, 0, 0, 0);

                var dialog = new AlertDialog.Builder(activity, Resource.Style.AlertsDialogTheme)
                    .SetMessage(message)
                    .SetCancelable(true)
                    .SetNegativeButton("OK", (s, args) => { })
                    .SetCustomTitle(titleView)
                    .Create();

                dialog.Window.SetBackgroundDrawable(new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()));

                dialog.Show();
            });
        }

        public static void DisplayDialog(Activity activity, string title, string message, Action positiveAction, Action negativeAction = null)
        {
            var titleView = new TextView(activity)
            {
                Text = title,
                TextSize = 20,
                Gravity = Android.Views.GravityFlags.Center,
            };
            titleView.SetTextColor(Xamarin.Forms.Color.White.ToAndroid());
            titleView.SetPadding(5, 5, 5, 2);
            titleView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.ic_menu_notifications, 0, 0, 0);

            negativeAction = negativeAction ?? (() => { });

            var dialog = new AlertDialog.Builder(activity, Resource.Style.AlertsDialogTheme)
                .SetCustomTitle(titleView)
                .SetMessage(message)
                .SetCancelable(true)
                .SetNegativeButton("Cancel", (s, args) => negativeAction())
                .SetPositiveButton("OK", (s, args) => positiveAction())
                .Create();

            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()));
            dialog.Show();
        }

        public static void DisplayResults(Activity activity, string title, string message)
        {
            activity.RunOnUiThread(() =>
            {
                var titleView = new TextView(activity)
                {
                    Text = title,
                    TextSize = 20,
                    Gravity = Android.Views.GravityFlags.Center,
                };
                titleView.SetTextColor(Xamarin.Forms.Color.White.ToAndroid());
                titleView.SetPadding(5, 5, 5, 2);
                titleView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.mode, 0, 0, 0);

                var dialog = new AlertDialog.Builder(activity, Resource.Style.AlertsDialogTheme)
                    .SetCustomTitle(titleView)
                    .SetMessage(message)
                    .SetCancelable(true)
                    .SetNegativeButton("OK", (s, args) => { })
                    .Create();

                dialog.Window.SetBackgroundDrawable(new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()));

                dialog.Show();
            });
        }

        public static void DisplayMarker(Activity activity, string title, Action action)
        {
            var titleView = new TextView(activity)
            {
                Text = title,
                TextSize = 20,
                Gravity = Android.Views.GravityFlags.Center,
            };
            titleView.SetTextColor(Xamarin.Forms.Color.White.ToAndroid());
            titleView.SetPadding(5, 5, 5, 2);
            titleView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.ic_menu_myplaces, 0, 0, 0);

            var dialog = new AlertDialog.Builder(activity, Resource.Style.AlertsDialogTheme)
                .SetCustomTitle(titleView)
                .SetCancelable(true)
                .SetNegativeButton("Cancel", (s, args) => { })
                .SetPositiveButton("Select", (s, args) => action())
                .Create();

            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()));
            dialog.Show();
        }

        public static void DisplayMarker(Activity activity, string title, string message)
        {
            var titleView = new TextView(activity)
            {
                Text = title,
                TextSize = 20,
                Gravity = Android.Views.GravityFlags.Center,
            };
            titleView.SetTextColor(Xamarin.Forms.Color.White.ToAndroid());
            titleView.SetPadding(5, 5, 5, 2);
            titleView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.ic_menu_myplaces, 0, 0, 0);

            var dialog = new AlertDialog.Builder(activity, Resource.Style.AlertsDialogTheme)
                .SetCustomTitle(titleView)
                .SetMessage(message)
                .SetCancelable(true)
                .SetNegativeButton("OK", (s, args) => { })
                .Create();

            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Xamarin.Forms.Color.Transparent.ToAndroid()));
            dialog.Show();
        }
    }
}