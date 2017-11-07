using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Noter.Droid.Utilities;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;

namespace Noter.Droid.Adapters
{
    public abstract class MultiSearchAdapter<T> : RecyclerView.Adapter, IFilterable
    {
        protected Activity _activity;
        protected List<T> _currentItems = new List<T>();
        protected List<int> _selectedIndices = new List<int>();

        private SearchFilter _filter;
        public Filter Filter { get { return _filter; } }

        public override int ItemCount => _currentItems.Count;
        public IEnumerable<T> SelectedItems
        {
            get
            {
                foreach (var index in _selectedIndices)
                {
                    yield return _currentItems[index];
                }
            }
        }

        public MultiSearchAdapter(Activity activity, List<T> items)
        {
            _activity = activity;
            _currentItems = items;
            _filter = new SearchFilter(this);
        }

        public void SetSelected(int position)
        {
            int index = position - 1;

            if (_selectedIndices.Contains(index))
            {
                _selectedIndices.Remove(index);
            }
            else
            {
                _selectedIndices.Add(index);
            }

            NotifyDataSetChanged();
        }

        public void SelectAll()
        {
            _selectedIndices.Clear();
            _selectedIndices.AddRange(Enumerable.Range(0, _currentItems.Count));
            NotifyDataSetChanged();
        }

        public virtual void DeleteSelectedItems()
        {
            foreach (var index in _selectedIndices)
            {
                _currentItems.RemoveAt(index);
            }

            _selectedIndices.Clear();
            NotifyDataSetChanged();
        }

        public void ClearSelectedItems()
        {
            _selectedIndices.Clear();
            NotifyDataSetChanged();
        }

        private class SearchFilter : Filter
        {
            private MultiSearchAdapter<T> _adapter;
            private List<T> _originalItems;

            public SearchFilter(MultiSearchAdapter<T> adapter)
            {
                _adapter = adapter;
                _originalItems = adapter._currentItems;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var filterResults = new FilterResults();

                if (constraint != null)
                {
                    var results = new List<T>();

                    if (_originalItems != null && _originalItems.Any())
                    {
                        results.AddRange(_originalItems.Where(r => r.ToString().ToLower().Contains(constraint.ToString().ToLower())));
                    }

                    filterResults.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                    filterResults.Count = results.Count;

                    constraint.Dispose();
                }

                return filterResults;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                {
                    _adapter._currentItems = values.ToArray<Java.Lang.Object>().Select(a => a.ToNetObject<T>()).ToList();
                }

                _adapter.NotifyDataSetChanged();

                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}