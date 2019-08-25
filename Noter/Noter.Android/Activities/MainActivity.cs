using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Noter.Droid.Fragments;
using Noter.Shared.Data;
using Noter.Shared.DataAccessLayer;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Xamarin.Forms;

namespace Noter.Droid.Activities
{
    [Activity(Label = "Noter", Icon = "@drawable/icon", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            SetUpUnhandledExceptionHandlers();
            SetUpTestDatabase();

            // OverridePendingTransition(Resource.Animation.abc_slide_in_top, Resource.Animation.abc_slide_out_top);
            SetContentView(Resource.Layout.activity_main);

            /*var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.Title = "Noter";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_home);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;*/
            PermissionsHelper.RequestPermissions(this, () =>
            {
                // Log some shit about permissions being granted
                var viewPager = FindViewById<ViewPager>(Resource.Id.view_pager);
                var tabLayout = FindViewById<TabLayout>(Resource.Id.tab_layout);

                var tabAdapter = new MainTabAdapter(this, GetSupportFragmentManager(), tabLayout);
                viewPager.OffscreenPageLimit = 4;
                viewPager.Adapter = tabAdapter;
                tabLayout.SetupWithViewPager(viewPager);
                tabAdapter.SetUpTabIcons();
            });
        }

        private void SetUpUnhandledExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Log.Debug("Unhandled Exception", e.ExceptionObject.ToString());
                DebugLog.LazyWrite(this, "UnhandledException: " + e.ExceptionObject);
            };

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                Log.Debug("Unhandled Exception", e.Exception.ToString());
                DebugLog.LazyWrite(this, "UnhandledExceptionRaiser: " + e.Exception);
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log.Debug("Unhandled Exception", e.Exception.ToString());
                DebugLog.LazyWrite(this, "UnobservedTaskException: " + e.Exception);
            };
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

        /*public override bool OnCreateOptionsMenu(IMenu menu)
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
                    Finish();*
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }*/

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
