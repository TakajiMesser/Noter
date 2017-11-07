using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Noter.Droid.Utilities;
using Android.Support.V7.Widget;

namespace Noter.Droid.Adapters
{
    public class SearchFilter<T> : Filter
    {
        private MultiChoiceRecyclerAdapter<T> _parent;
        private List<T> _originalItems;

        public SearchFilter(MultiChoiceRecyclerAdapter<T> parent)
        {
            _parent = parent;
            //_originalItems = parent.Items;
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
                //_parent.Items = values.ToArray<Java.Lang.Object>().Select(a => a.ToNetObject<T>()).ToList();
            }

            _parent.NotifyDataSetChanged();

            constraint.Dispose();
            results.Dispose();
        }
    }
}