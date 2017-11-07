using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Noter.Droid.Fragments;
using Noter.Shared.Data;
using System;
using Noter.Shared.DataAccessLayer;
using Android.Support.Design.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Xamarin.Forms;

namespace Noter.Droid.Activities
{
    [Activity (Label = "Noter", Icon = "@drawable/icon", Theme="@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
            Xamarin.Forms.Forms.Init(this, bundle);

            // OverridePendingTransition(Resource.Animation.abc_slide_in_top, Resource.Animation.abc_slide_out_top);
            SetContentView(Resource.Layout.activity_base);

            var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.Title = "Noter";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_home);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_layout, GetTabFrom(Resource.Id.menu_library))
                .Commit();

            SetUpTestDatabase();
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            var fragment = GetTabFrom(e.Item.ItemId);

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_layout, fragment)
                .Commit();
        }

        private Fragment GetTabFrom(int id)
        {
            switch (id)
            {
                case Resource.Id.menu_library:
                    return LibraryFragment.Instantiate();
                case Resource.Id.menu_shared:
                    return ShelfFragment.Instantiate();
                case Resource.Id.menu_settings:
                    return HomeFragment.Instantiate();
            }

            throw new ArgumentOutOfRangeException("Cannot handle resource ID " + id);
        }

        private void SetUpTestDatabase()
        {
            DBAccess.ResetTables();
            var shelf = new Shelf()
            {
                Name = "Shelf 01"
            };
            DBTable.Insert(shelf);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    return true;
                case Resource.Id.action_settings:
                    //StartActivity(new Intent(this, typeof(SettingsActivity)));
                    return true;
                case Resource.Id.action_feedback:
                    Device.OpenUri(new Uri("mailto:takaji.messer@gmail.com?subject=Noter-Feedback"));
                    return true;
                case Resource.Id.action_logout:
                    /*StartActivity(new Intent(this, typeof(LoginActivity))
                        .SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask)
                        .PutExtra("logout", true));
                    Finish();*/
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount > 1)
            {
                FragmentManager.PopBackStack();
            }
            /*else
            {
                StartActivity(new Intent(this, typeof(HomeActivity)));
                Finish();
            }*/
        }
    }
}
