using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace Microsoft.Knowzy.AuthenticationService
{
    public static class AccountHelper
    {
        // In the real world this would not be needed as there would be a server implemented that would host a user account database.
        // For this tutorial we will just be storing accounts locally.
        private const string USER_ACCOUNT_LIST_FILE_NAME = "accountlist.txt";
        private static string _accountListPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, USER_ACCOUNT_LIST_FILE_NAME);
        public static List<Account> AccountList = new List<Account>();

        /// <summary>
        /// Create and save a useraccount list file. (Updating the old one)
        /// </summary>
        private static async void SaveAccountListAsync()
        {
            string accountsXml = SerializeAccountListToXml();

            if (File.Exists(_accountListPath))
            {
                StorageFile accountsFile = await StorageFile.GetFileFromPathAsync(_accountListPath);
                await FileIO.WriteTextAsync(accountsFile, accountsXml);
            }
            else
            {
                StorageFile accountsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(USER_ACCOUNT_LIST_FILE_NAME);
                await FileIO.WriteTextAsync(accountsFile, accountsXml);
            }
        }

        /// <summary>
        /// Gets the useraccount list file and deserializes it from XML to a list of useraccount objects.
        /// </summary>
        /// <returns>List of useraccount objects</returns>
        public static async Task<List<Account>> LoadAccountListAsync()
        {
            if (File.Exists(_accountListPath))
            {
                StorageFile accountsFile = await StorageFile.GetFileFromPathAsync(_accountListPath);

                string accountsXml = await FileIO.ReadTextAsync(accountsFile);
                DeserializeXmlToAccountList(accountsXml);
            }

            return AccountList;
        }

        /// <summary>
        /// Uses the local list of accounts and returns an XML formatted string representing the list
        /// </summary>
        /// <returns>XML formatted list of accounts</returns>
        public static string SerializeAccountListToXml()
        {
            XmlSerializer xmlizer = new XmlSerializer(typeof(List<Account>));
            StringWriter writer = new StringWriter();
            xmlizer.Serialize(writer, AccountList);

            return writer.ToString();
        }

        /// <summary>
        /// Takes an XML formatted string representing a list of accounts and returns a list object of accounts
        /// </summary>
        /// <param name="listAsXml">XML formatted list of accounts</param>
        /// <returns>List object of accounts</returns>
        public static List<Account> DeserializeXmlToAccountList(string listAsXml)
        {
            XmlSerializer xmlizer = new XmlSerializer(typeof(List<Account>));
            TextReader textreader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(listAsXml)));

            return AccountList = (xmlizer.Deserialize(textreader)) as List<Account>;
        }

        public static Account AddAccount(string username)
        {
            // Create a new account with the username
            Account account = new Account() { Username = username };
            // Add it to the local list of accounts
            AccountList.Add(account);
            // SaveAccountList and return the account
            SaveAccountListAsync();
            return account;
        }

        public static void RemoveAccount(Account account)
        {
            // Remove the account from the accounts list
            AccountList.Remove(account);
            // Re save the updated list
            SaveAccountListAsync();
        }

        public static bool ValidateAccountCredentials(string username)
        {
            // In the real world, this method would call the server to authenticate that the account exists and is valid.
            // For this tutorial however we will just have a existing sample user that is just "sampleUsername"
            // If the username is null or does not match "sampleUsername" it will fail validation. In which case the user should register a new passport user

            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            return true;
        }
    }
}
