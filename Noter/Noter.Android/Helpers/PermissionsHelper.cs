using System;
using Android.App;
using Android.Content;
using Android.Preferences;

namespace Noter.Droid.Helpers
{
    public static class PermissionsHelper
    {
        private const int REQUEST_CODE = 1;
        private readonly string[] PERMISSIONS = { Manifest.Permission.WriteExternalStorage };

        public static void RequestPermissions(ActivityCompat activity, Action grantedAction)
        {
            var permissions = GetPermissionsToRequest(activity).ToArray();
            if (permissions.Length > 0)
            {
                grantedAction();
            }
            else
            {
                ActivityCompat.RequestPermissions(activity, permissions, REQUEST_CODE);
            }
        }

        private static IEnumerable<string> GetPermissionsToRequest(ActivityCompat activity)
        {
            foreach (var permission in PERMISSIONS)
            {
                if (ActivityCompat.CheckSelfPermission(activity, permission) != Permission.Granted)
                {
                    yield return permission;
                }
            }
        }

        private static bool HandlePermissionsResult(ActivityCompat activity, int requestCode, string[] permissions, int[] grantResults, Action grantedAction)
        {
if (requestCode == PERMISSIONS_REQUEST_CODE)
            {
                for (int i = 0; i < permissions.length; i++)
                {
                    if (i >= grantResults.length)
                    {
                        break;
                    }

                    String permission = permissions[i];
                    int grantResult = grantResults[i];

                    if (grantResult != PackageManager.PERMISSION_GRANTED)
                    {
                        DebugLog.log(TAG, "Permission not granted: results len = " + grantResults.length + " Result code = " + (grantResults.length > 0 ? grantResults[0] : "(empty)"));
                        handlePermissionDenial(activity, permission, permissionRequirer);
                        return true;
                    }
                    else
                    {
                        DebugLog.log(TAG, permission + "permission granted");
                    }
                }

                DebugLog.log(TAG, "All permissions granted");

                if (permissionRequirer != null)
                {
                    activity.runOnUiThread(new Runnable()
        {
          @Override
          public void run()
                    {
                        permissionRequirer.onPermissionsGranted();
                    }
                });
            }

            return true;
        }
    else
    {
      DebugLog.log(TAG, "Got unexpected permission result: " + requestCode);
      return false;
    }
        }

        private static void HandlePermissionDenial()
        {
if (ActivityCompat.shouldShowRequestPermissionRationale(activity, permission))
    {
        DialogHelper.displayAlert(activity, getRationalMessageId(permission), new Runnable()
      {
        @Override
        public void run()
        {
            requestPermissions(activity, permissionRequirer);
        }
    });
}
    else
    {
      DialogHelper.displayAlert(activity, getFailureMessageId(permission), new Runnable()
{
    @Override
        public void run()
    {
        activity.finish();
    }
});
    }
        }

        private static int GetRationalMessageID(string permission)
        {
            switch (permission)
            {
                case Manifest.Permission.WriteExternalStorage:
                    return -1; // Replace with string resource
            }

            return -1;
        }
    }
}

/*
  <Preference
          android:key="reset_defaults"
          android:title="Reset to default values"
          android:summary="This will reset ALL settings to the default values"/>*/
