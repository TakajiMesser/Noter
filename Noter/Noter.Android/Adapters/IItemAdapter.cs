using Android.App;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Noter.Shared.Data;
using System.Collections.Generic;

namespace Noter.Droid.Adapters
{
    public interface IItemAdapter<T>
    {
        T GetItemAt(int position);
    }
}