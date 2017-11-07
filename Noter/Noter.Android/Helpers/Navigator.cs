using System;

using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Noter.Droid.Activities;

namespace Noter.Droid.Helpers
{
    public class Navigator
    {
        private AppCompatActivity _activity;
        private DrawerLayout _drawerLayout;
        private int _frameLayoutID;

        public Navigator(AppCompatActivity activity, int toolbarID, int navigationViewID, int drawerLayoutID, int frameLayoutID)
        {
            _activity = activity;

            var toolbar = _activity.FindViewById<Toolbar>(toolbarID);
            _activity.SetSupportActionBar(toolbar);

            _activity.SupportActionBar.SetHomeAsUpIndicator(null);
            _activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            _activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            _activity.SupportActionBar.SetHomeButtonEnabled(true);

            var navigationView = _activity.FindViewById<NavigationView>(navigationViewID);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            _drawerLayout = _activity.FindViewById<DrawerLayout>(drawerLayoutID);
            var drawerToggle = new ActionBarDrawerToggle(_activity, _drawerLayout, Resource.String.drawer_open, Resource.String.drawer_close);
            _drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            _frameLayoutID = frameLayoutID;
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_home:
                    _activity.StartActivity(new Intent(_activity, typeof(MainActivity)));
                    _activity.Finish();
                    break;
                /*case Resource.Id.nav_trip:
                    _activity.StartActivity(new Intent(_activity, typeof(TripActivity)));
                    _activity.Finish();
                    break;
                case Resource.Id.nav_list:
                    _activity.StartActivity(new Intent(_activity, typeof(RegionListActivity)));
                    _activity.Finish();
                    break;
                case Resource.Id.nav_map:
                    _activity.StartActivity(new Intent(_activity, typeof(RegionMapActivity)));
                    _activity.Finish();
                    break;
                case Resource.Id.nav_database:
                    _activity.StartActivity(new Intent(_activity, typeof(DatabaseActivity)));
                    _activity.Finish();
                    break;
                case Resource.Id.nav_settings:
                    _activity.StartActivity(new Intent(_activity, typeof(SettingsActivity)));
                    _activity.Finish();
                    break;
                case Resource.Id.nav_logout:
                    UserDB.Clear();
                    _activity.StartActivity(new Intent(_activity, typeof(LoginActivity)));
                    _activity.Finish();
                    break;*/
            }

            _drawerLayout.CloseDrawers();
        }
    }
}