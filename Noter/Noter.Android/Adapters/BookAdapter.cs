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
    public class BookAdapter : MultiChoiceRecyclerAdapter<Book>, IFilterable
    {
        private Context _context;
        private List<Book> _books = new List<Book>();
        private SearchFilter _filter;

        public Filter Filter => _filter;
        public override int ItemCount => _books.Count;

        public BookAdapter(Context context, IEnumerable<Book> books)
        {
            _context = context;
            _books.AddRange(books);
            _filter = new SearchFilter(this);
        }

        public IEnumerable<Book> SelectedBooks
        {
            get
            {
                foreach (var position in SelectedPositions)
                {
                    yield return _books[position];
                }
            }
        }

        public void DeleteSelectedItems()
        {
            foreach (var position in SelectedPositions.OrderByDescending(p => p))
            {
                _books.RemoveAt(position);
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
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item_book, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var book = _books[position];
            var viewHolder = holder as ViewHolder;

            viewHolder.Layout.Tag = position;
            viewHolder.Layout.Selected = ItemSelected(position);

            viewHolder.Title.Text = book.Title;
        }

        private class SearchFilter : Filter
        {
            private BookAdapter _parent;
            private List<Book> _originalBooks;

            public SearchFilter(BookAdapter parent)
            {
                _parent = parent;
                _originalBooks = parent._books;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var filterResults = new FilterResults();

                if (constraint != null)
                {
                    var results = new List<Book>();

                    if (_originalBooks != null && _originalBooks.Any())
                    {
                        results.AddRange(_originalBooks.Where(r => r.ToString().ToLower().Contains(constraint.ToString().ToLower())));
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
                    _parent._books = values.ToArray<Java.Lang.Object>().Select(a => a.ToNetObject<Book>()).ToList();
                }

                _parent.NotifyDataSetChanged();

                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}