using System;
using System.Net.Http;
using System.Threading.Tasks;
using SQLite;
// import com.google.api.client.auth.oauth2.Credential;
// import com.google.api.services.drive.Drive;

namespace Noter.Shared.DataAccessLayer
{
    public static class GoogleDriveHelper
    {
        public static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(10);

        public static void DoSomeShit()
        {
            /*var token = AccountManager.Get(context)
                .blockingGetAuthToken(< android.accounts.Account >, "oauth2:" + DriveScopes.DRIVE, false);
            var credential = new GoogleCredential().setAccessToken(token);
            var service = new Drive.Builder(new NetHttpTransport(), new JacksonFactory(), credential)
                .setApplicationName("app name")
                .build();*/
        }
    }
}
