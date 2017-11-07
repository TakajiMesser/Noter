using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Java.Lang;
using System;
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace Noter.Droid.Adapters
{
    public class TabAdapter : FragmentPagerAdapter
    {
        private Activity _activity;
        private FragmentManager _manager;

        private FragmentTransaction _transaction;
        private Dictionary<int, Fragment> _fragmentsByPosition = new Dictionary<int, Fragment>();

        public override int Count { get { return 5; } }

        public TabAdapter(Activity activity, FragmentManager manager) : base(manager)
        {
            _activity = activity;
            _manager = manager;
        }

        public override int GetItemPosition(Java.Lang.Object obj)
        {
            return ContainsFragment(obj) ? PositionUnchanged : PositionNone;
        }

        private bool ContainsFragment(Java.Lang.Object obj)
        {
            return false;
        }

        public override Fragment GetItem(int position)
        {
            /*switch (TabLevel)
            {
                case TabLevels.Regions:
                    return RegionListFragment.Instantiate(this);
                case TabLevels.Subregions:
                    return SubregionListFragment.Instantiate(RegionName, this);
                case TabLevels.Intersections:
                    return IntersectionListFragment.Instantiate(RegionName, SubregionName);
            }*/

            throw new InvalidOperationException();
        }

        public override void NotifyDataSetChanged()
        {
            _activity.RunOnUiThread(() =>
            {
                base.NotifyDataSetChanged();
            });
        }

        /*public void SetTabIcons()
        {
            _tabLayout.GetTabAt((int)TabTypes.List).SetIcon(Resource.Drawable.ic_chrome_reader_mode_white_24dp);
            _tabLayout.GetTabAt((int)TabTypes.Map).SetIcon(Resource.Drawable.ic_dialog_map);
        }*/

        public override void StartUpdate(ViewGroup container)
        {
            //base.StartUpdate(container);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            // Check to see if this fragment already exists
            if (_fragmentsByPosition.ContainsKey(position))
            {
                return _fragmentsByPosition[position];
            }
            else
            {
                if (_transaction == null)
                {
                    _transaction = _manager.BeginTransaction();
                }

                var fragment = GetItem(position);
                _fragmentsByPosition.Add(position, fragment);

                _transaction.Add(container.Id, fragment);

                return fragment;
            }
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            var fragment = @object as Fragment;

            if (_transaction == null)
            {
                _transaction = _manager.BeginTransaction();
            }

            /*if (position != (int)TabTypes.Map)
            {
                _transaction.Remove(fragment);
                _fragmentsByPosition.Remove(position);
            }*/
        }

        public override void SetPrimaryItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            base.SetPrimaryItem(container, position, @object);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return base.IsViewFromObject(view, @object);
        }

        public override IParcelable SaveState()
        {
            return base.SaveState();
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {
            base.RestoreState(state, loader);
        }

        public override void FinishUpdate(ViewGroup container)
        {
            if (_transaction != null)
            {
                _transaction.Commit();

                _activity.RunOnUiThread(() =>
                {
                    _manager.ExecutePendingTransactions();
                });

                _transaction = null;
            }
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String();
        }
    }
}