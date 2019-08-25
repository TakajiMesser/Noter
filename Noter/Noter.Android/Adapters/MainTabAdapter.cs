using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace Noter.Droid.Adapters
{
    public enum MainTabTypes
    {
        MyLibrary,
        Network,
        Settings
    }

    public class MainTabAdapter : FragmentPagerAdapter
    {
        private ContextBoundObject _context;
        private TabLayout _tabLayout;

        public override int Count => Enum.GetValues(typeof(MainTabTypes)).Length;

        public MainTabAdapter(ContextBoundObject context, FragmentManager fragmentManager, TabLayout tabLayout) : base(fragmentManager)
        {
            _context = context;
            _tabLayout = tabLayout;
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) => new Java.Lang.String();

        public override Fragment GetItem(int position)
        {
            switch ((MainTabTypes) position)
            {
                case MainTabTypes.MyLibrary:
                    return MyLibraryFragment.Instantiate();
                case MainTabTypes.Network:
                    return NetworkFragment.Instantiate();
                case MainTabTypes.Settings:
                    return SettingsFragment.Instantiate();
            }

            throw new ArgumentOutOfRangeException("Could not handle position " + position);
        }

        public void SetUpTabIcons()
        {
            _tabLayout.TabSelected += (s, args) => SetTabIconColor(args.Tab, Resource.Color.Blah);
            _tabLayout.TabUnselected += (s, args) => SetTabIconColor(args.Tab, Resource.Color.Blah);

            SetTabIcon(MainTabTypes.MyLibrary, Resource.Drawable.blah, Resource.Color.Blah);
            SetTabIcon(MainTabTypes.Network, Resource.Drawable.blah, Resource.Color.Blah);
            SetTabIcon(MainTabTypes.Settings, Resource.Drawable.blah, Resource.Color.Blah);
        }

        private void SetTabIcon(TabTypes tabType, /*@DrawableRes*/ int drawableResId, /*@ColorRes*/ int colorResId)
        {
            TabLayout.Tab tab = tabLayout.getTabAt(tabType.ordinal());
            if (tab != null)
            {
                tab.setIcon(drawableResId);
                setTabIconColor(tab, colorResId);
            }
        }

        private void SetTabIconColor(TabLayout.Tab tab, /*@ColorRes*/ int colorResId)
        {
            Drawable icon = tab.getIcon();
            if (icon != null)
            {
                icon.setColorFilter(ContextCompat.getColor(context, colorResId), PorterDuff.Mode.SRC_IN);
            }
        }
    }
}
