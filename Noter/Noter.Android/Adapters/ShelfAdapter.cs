using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Noter.Droid.Utilities;
using Noter.Shared.Data;
using System.Collections.Generic;
using System.Linq;

namespace Noter.Droid.Adapters
{
    public class ShelfAdapter : MultiChoiceRecyclerAdapter<Shelf>, IFilterable
    {
        private Context _context;
        private List<Shelf> _shelves = new List<Shelf>();
        private SearchFilter _filter;

        public Filter Filter => _filter;
        public override int ItemCount => _shelves.Count;

        public ShelfAdapter(Context context, IEnumerable<Shelf> shelves)
        {
            _context = context;
            _shelves.AddRange(shelves);
            _filter = new SearchFilter(this);
        }

        public IEnumerable<Shelf> SelectedShelves
        {
            get
            {
                foreach (var position in SelectedPositions)
                {
                    yield return _shelves[position];
                }
            }
        }

        public void DeleteSelectedItems()
        {
            foreach (var position in SelectedPositions.OrderByDescending(p => p))
            {
                _shelves.RemoveAt(position);
            }

            NotifyDataSetChanged();
        }

        protected class ViewHolder : RecyclerView.ViewHolder
        {
            public LinearLayout Layout { get; set; }
            public TextView Title { get; set; }
            public StateListDrawable Background { get; set; }

            public ViewHolder(View itemView) : base(itemView)
            {
                Layout = (LinearLayout)itemView;
                Title = itemView.FindViewById<TextView>(Resource.Id.text);
                Background = itemView.Background as StateListDrawable;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item_shelf, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var shelf = _shelves[position];
            var viewHolder = holder as ViewHolder;

            viewHolder.Layout.Tag = position;
            viewHolder.Layout.Selected = ItemSelected(position);

            viewHolder.Title.Text = shelf.Name;
        }

        private class SearchFilter : Filter
        {
            private ShelfAdapter _parent;
            private List<Shelf> _originalShelves;

            public SearchFilter(ShelfAdapter parent)
            {
                _parent = parent;
                _originalShelves = parent._shelves;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var filterResults = new FilterResults();

                if (constraint != null)
                {
                    var results = new List<Shelf>();

                    if (_originalShelves != null && _originalShelves.Any())
                    {
                        results.AddRange(_originalShelves.Where(r => r.ToString().ToLower().Contains(constraint.ToString().ToLower())));
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
                    _parent._shelves = values.ToArray<Java.Lang.Object>().Select(a => a.ToNetObject<Shelf>()).ToList();
                }

                _parent.NotifyDataSetChanged();

                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}