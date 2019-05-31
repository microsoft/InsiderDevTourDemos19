// ******************************************************************

// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.

// ******************************************************************

using Microsoft.Knowzy.AuthenticationService;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Knowzy.WPF.Views
{
    public partial class LoginView 
    {
        public LoginView()
        {
            InitializeComponent();

            try
            {
                // This is a little hacky. The Account Helper makes Windows calls, 
                // so calling it will throw an exception when not run in a Windows package.
                // Use this to determine whether hello should be available.
                // TODO: Replace with legitimate check.
                AccountHelper.ValidateAccountCredentials("");
                _useWindowsHello = true;
            }
            catch
            {
                _useWindowsHello = false;
            }
        }

        private bool _useWindowsHello;
        private Account _account;

        protected override async void OnContentRendered(EventArgs e)
        {
            // Check Microsoft Passport is setup and available on this machine
            if (_useWindowsHello && await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
                PasswordTextBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void PassportSignInButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            if (_useWindowsHello)
            {
                SignInPassport();
            }
            else
            {
                (DataContext as ViewModels.LoginViewModel).DoLogin();
            }
        }

        private async void SignInPassport()
        {
            PassportSignInButton.IsEnabled = false;
            ErrorMessage.Text = "Validating...";

            var username = UsernameTextBox.Text;
            if (AccountHelper.ValidateAccountCredentials(username))
            {
                List<Account> accounts = await AccountHelper.LoadAccountListAsync();

                int accountIndex = -1;
                for (var i = 0; i < accounts.Count; i++)
                {
                    var account = accounts[i];
                    if (account.Username == username)
                    {
                        accountIndex = i;
                        break;
                    }
                }

                if (accountIndex == -1)
                {
                    _account = AccountHelper.AddAccount(username);
                    Debug.WriteLine("Successfully signed in with traditional credentials and created local account instance!");
                }
                else
                {
                    _account = accounts[accountIndex];
                }

                if (await MicrosoftPassportHelper.CreatePassportKeyAsync(_account.Username))
                {
                    Debug.WriteLine("Successfully signed in with Microsoft Passport!");
                    (DataContext as ViewModels.LoginViewModel).DoLogin();
                }

                ErrorMessage.Text = "";
            }
            else
            {
                ErrorMessage.Text = "Invalid Credentials";
            }
            PassportSignInButton.IsEnabled = true;
        }
    }
}
