namespace Noter.Droid.Utilities
{
    public static class CredentialsService
    {
        public static string ApplicationName { get { return "Noter"; } }

        /*public static string Username
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(ApplicationName).FirstOrDefault();
                return account?.Username;
            }
        }

        public static string Password
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(ApplicationName).FirstOrDefault();
                return account?.Properties["Password"];
            }
        }

        /*public static string FirstHubAddress
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(ApplicationName).FirstOrDefault();
                return account?.Properties["HubAddress"];
            }
        }

        public static async Task RenewPSASession(PSAUserCredentials credentials)
        {
            if (Communication.NeedsToRenewSession(credentials))
            {
                var request = new PSALoginRequest(SharedData.Instance.Credentials);

                string response = await PSACommunication.GetResponseAsync(Communication.ActiveHostServer, PSALoginRequest.DefaultServiceName, request.ToParameterString());
                var loginResponse = PSALoginResponse.ParseXMLString(response);

                if (loginResponse.Status != Com.TTS.Util.GUI.WSStatusTypes.OK)
                {
                    throw new UnauthorizedAccessException("Failed to reach PSA back-end (" + loginResponse.Message + ")");
                }

                Communication.LastSessionRenewal = DateTime.Now;
                Communication.ActiveSessionUsername = credentials.Username;
                Communication.ActiveSessionCode = loginResponse.SessionInfo.SessionCode;
                Communication.UserCoverage = loginResponse.Coverage;
            }
        }

        public static async Task<PSALoginResponse> LoginToPSA(string username, string password, string hubAddress)
        {
            SharedData.Instance.Username = username;
            SharedData.Instance.Password = password;

            SharedData.Instance.HubAddress = hubAddress;
            Communication.ActiveHostServer = hubAddress;

            var request = new PSALoginRequest(SharedData.Instance.Credentials);

            string response = await PSACommunication.GetResponseAsync(Communication.ActiveHostServer, PSALoginRequest.DefaultServiceName, request.ToParameterString());
            var loginResponse = PSALoginResponse.ParseXMLString(response);

            if (loginResponse.Status == Com.TTS.Util.GUI.WSStatusTypes.OK)
            {
                Communication.LastSessionRenewal = DateTime.Now;
                Communication.ActiveSessionUsername = username;
                Communication.ActiveSessionCode = loginResponse.SessionInfo.SessionCode;
                Communication.UserCoverage = loginResponse.Coverage;
            }

            return loginResponse;
        }

        public static void SaveCredentials(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = username
                };
                account.Properties.Add("Password", password);

                AccountStore.Create(Forms.Context).Save(account, ApplicationName);
            }
        }

        public static void DeleteCredentials()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(ApplicationName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create(Forms.Context).Delete(account, ApplicationName);
            }
        }


        public static bool DoCredentialsExist()
        {
            return AccountStore.Create(Forms.Context).FindAccountsForService(ApplicationName).Any();
        }*/
    }
}