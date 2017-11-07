namespace Noter.Droid.Utilities
{
    public interface ICredentialsService
    {
        string ApplicationName { get; }
        string Username { get; }
        string Password { get; }
        string FirstHubAddress { get; }

        //bool LoginToPSA(string username, string password, string hubAddress);
        void SaveCredentials(string username, string password);
        void DeleteCredentials();
        bool DoCredentialsExist();
    }
}