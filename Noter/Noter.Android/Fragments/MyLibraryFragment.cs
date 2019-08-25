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

namespace Noter.Droid.Fragments
{
    public class MyLibraryFragment : Fragment
    {
        public static MyLibraryFragment Instantiate() => new MyLibraryFragment();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
            inflater.Inflate(Resource.Layout.fragment_my_library, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Read from DB, get list of Shelves, then add to ShelfAdapter to display
        }
    }
}
