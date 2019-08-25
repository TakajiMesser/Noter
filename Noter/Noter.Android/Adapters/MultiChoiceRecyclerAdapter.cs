using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Noter.Droid.Adapters
{
    public abstract class MultiChoiceRecyclerAdapter<T> : RecyclerView.Adapter, AbsListView.IMultiChoiceModeListener
    {
        private List<int> _selectedPositions = new List<int>();
        private ActionMode _actionMode;
        private AbsListView.IMultiChoiceModeListener _listener;

        public IEnumerable<int> SelectedPositions
        {
            get
            {
                foreach (var position in _selectedPositions.OrderBy(p => p))
                {
                    yield return position;
                }
            }
        }

        public bool ItemSelected(int position) => _selectedPositions.Contains(position);

        public void SetAllItemsChecked(bool @checked)
        {
            if (@checked)
            {
                for (var i = 0; i < ItemCount; i++)
                {
                    if (!_selectedPositions.Contains(i))
                    {
                        _selectedPositions.Add(i);
                    }
                }
            }
            else
            {
                _selectedPositions.Clear();
            }

            NotifyDataSetChanged();
        }

        public void SetMultiChoiceModeListener(AbsListView.IMultiChoiceModeListener listener)
        {
            _listener = listener;
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            if (@checked)
            {
                _selectedPositions.Add(position);
            }
            else
            {
                _selectedPositions.Remove(position);
                if (!_selectedPositions.Any())
                {
                    mode.Finish();
                }
            }

            NotifyItemChanged(position);
            _listener.OnItemCheckedStateChanged(mode, position, id, @checked);
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            return _listener.OnActionItemClicked(mode, item);
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            NotifyDataSetChanged();
            return _listener.OnCreateActionMode(mode, menu);
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            _selectedPositions.Clear();
            NotifyDataSetChanged();
            _listener.OnDestroyActionMode(mode);
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return _listener.OnPrepareActionMode(mode, menu);
        }

        public class ItemClickEventArgs : EventArgs
        {
            public int Position { get; set; }
            public T Item { get; set; }

            public ItemClickEventArgs(int position, T item)
            {
                Position = position;
                Item = item;
            }
        }

        public class ItemLongClickEventArgs : EventArgs
        {
            public int Position { get; set; }
            public T Item { get; set; }

            public ItemLongClickEventArgs(int position, T item)
            {
                Position = position;
                Item = item;
            }
        }
    }
}