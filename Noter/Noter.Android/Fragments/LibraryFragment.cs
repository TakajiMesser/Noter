using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Noter.Droid.Adapters;
using Noter.Shared.Data;
using Noter.Shared.DataAccessLayer;
using System.Linq;

namespace Noter.Droid.Fragments
{
    public class LibraryFragment : Fragment
    {
        private RecyclerView _recycler;
        private ShelfAdapter _adapter;

        public static LibraryFragment Instantiate()
        {
            return new LibraryFragment();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_library, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _recycler = view.FindViewById<RecyclerView>(Resource.Id.library_recycler_view);

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            _recycler.SetLayoutManager(layoutManager);

            //_recycler.SetItemAnimator(new DefaultItemAnimator());
            var shelves = DBTable.GetAll<Shelf>().ToList();
            _adapter = new ShelfAdapter(Activity, shelves);
            _recycler.SetAdapter(_adapter);
        }
    }
}