using Android.App;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Noter.Shared.Data;
using System.Collections.Generic;

namespace Noter.Droid.Adapters
{
    public class PageAdapter : MultiSearchAdapter<Page>
    {
        public PageAdapter(Activity activity, List<Page> pages) : base(activity, pages) { }

        protected class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView Title { get; set; }
            public StateListDrawable Background { get; set; }

            public ViewHolder(View itemView) : base(itemView)
            {
                Title = itemView.FindViewById<TextView>(Resource.Id.text);
                Background = itemView.Background as StateListDrawable;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item_log, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var page = _currentItems[position];

            var viewHolder = holder as ViewHolder;
            viewHolder.Title.Text = page.PageNr.ToString();

            // Position is index?
            var stateSet = _selectedIndices.Contains(position) ? new[] { Android.Resource.Attribute.StateActivated } : new int[] { };
            viewHolder.Background.SetState(stateSet);
        }
    }
}