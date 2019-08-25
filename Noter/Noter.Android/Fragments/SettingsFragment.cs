using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Preference;
using Android.Views;
using Android.Widget;

namespace Noter.Droid.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public static SettingsFragment Instantiate() => new SettingsFragment();

        public override void OnCreatePreferences(Bundle bundle, String rootKey) => SetPreferencesFromResource(Resources.XML.Preferences, rootKey);

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_shelf, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}
