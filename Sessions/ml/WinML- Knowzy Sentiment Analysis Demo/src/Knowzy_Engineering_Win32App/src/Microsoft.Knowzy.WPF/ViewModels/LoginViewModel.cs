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

using System;
using Caliburn.Micro;
using Microsoft.Knowzy.Authentication;

namespace Microsoft.Knowzy.WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly MainViewModel _mainViewModel;
        private string _userName;
        private string _password;

        public LoginViewModel(IAuthenticationService authenticationService, MainViewModel mainViewModel)
        {
            _authenticationService = authenticationService;
            _mainViewModel = mainViewModel;
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (value == _userName) return;
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public void DoLogin()
        {
            if (string.IsNullOrWhiteSpace(UserName)) return;
            try
            {
                _authenticationService.Login(UserName, Password);
                _mainViewModel.NotifyOfPropertyChange(() => _mainViewModel.LoggedUser);
                DoClose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Password = string.Empty;
            }
        }

        public void DoClose()
        {
            UserName = string.Empty;
            Password = string.Empty;
            _mainViewModel.NotifyOfPropertyChange(() => _mainViewModel.LoggedUser);
            TryClose();
        }
    }
}
