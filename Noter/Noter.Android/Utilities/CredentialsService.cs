using Android.Content;
using Com.TTS.APhA.ImportAPI;
using Com.TTS.APhA.UAC;
using EcoDrive.Shared.DataAccessLayer;
using EcoDrive.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Noter.Droid.Utilities
{
    public static class CredentialsService
    {
        public const string APPLICATION_NAME = "Noter";

        public static Account Account => AccountStore.Create().FindAccountsForService(APPLICATION_NAME).FirstOrDefault();

        public static bool AccountExists => Account != null;

        public static string Username => Account?.Username;
        public static string Password => Account?.Properties["Password"];
        public static IEnumerable<string> HubAddresses => Account?.Properties["Addresses"].Split(',').AsEnumerable();
        public static string HubAddress => HubAddresses.First();
        public static bool Remember => (Account != null) ? bool.Parse(Account.Properties["Remember"]) : false;

        public static PSAUserCredentials PSACredentials => new PSAUserCredentials()
        {
            Username = Username,
            Password = Password
        };

        public static void SaveCredentials(string username, string password, string address, bool remember)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty");

            if (AccountExists && Username != username)
            {
                // Once we change users, clear out any old hub DB data
                DeleteCredentials();
                DBAccess.ResetTables();
            }

            var account = new Account()
            {
                Username = username
            };
            account.Properties.Add("Password", password);
            account.Properties.Add("Remember", remember.ToString());
            account.Properties.Add("Addresses", address);

            if (AccountExists)
            {
                var hubAddresses = string.Join(",", HubAddresses.Where(a => a != address));
                if (!string.IsNullOrEmpty(hubAddresses))
                {
                    account.Properties["Addresses"] += "," + hubAddresses;
                }

                DeleteCredentials();
            }

            AccountStore.Create().Save(account, APPLICATION_NAME);
        }

        public static void DeleteCredentials()
        {
            if (AccountExists)
            {
                AccountStore.Create().Delete(Account, APPLICATION_NAME);
            }
        }

        public static async Task RenewPSASession()
        {
            if (Communication.NeedsToRenewSession(PSACredentials))
            {
                var request = new PSALoginRequest(PSACredentials);

                string response = await PSACommunication.GetResponseAsync(HubAddress, PSALoginRequest.DefaultServiceName, request.ToParameterString());
                var loginResponse = PSALoginResponse.ParseXMLString(response);

                if (loginResponse.Status != Com.TTS.Util.GUI.WSStatusTypes.OK)
                {
                    throw new UnauthorizedAccessException("Failed to reach PSA back-end. Returned message: " + loginResponse.Message + ")");
                }

                Communication.LastSessionRenewal = DateTime.Now;
                Communication.ActiveSessionUsername = PSACredentials.Username;
                Communication.ActiveSessionCode = loginResponse.SessionInfo.SessionCode;
                Communication.UserCoverage = loginResponse.Coverage;
            }
        }

        public static async Task<PSALoginResponse> LoginToPSA(Context context, string username, string password, string hubAddress)
        {
            Communication.ActiveHostServer = hubAddress;

            var credentials = new PSAUserCredentials()
            {
                Username = username,
                Password = password
            };

            var request = new PSALoginRequest(credentials);

            DebugLog.LazyWrite(context, "Getting response async...");
            string response = await PSACommunication.GetResponseAsync(hubAddress, PSALoginRequest.DefaultServiceName, request.ToParameterString());
            DebugLog.LazyWrite(context, "Got response async -> " + response);
            var loginResponse = PSALoginResponse.ParseXMLString(response);
            DebugLog.LazyWrite(context, "Parsed response async!");

            if (loginResponse.Status == Com.TTS.Util.GUI.WSStatusTypes.OK)
            {
                Communication.LastSessionRenewal = DateTime.Now;
                Communication.ActiveSessionUsername = username;
                Communication.ActiveSessionCode = loginResponse.SessionInfo.SessionCode;
                Communication.UserCoverage = loginResponse.Coverage;
            }

            return loginResponse;
        }
    }
}
