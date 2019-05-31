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

using System.Collections.Generic;
using Caliburn.Micro;
using Microsoft.Knowzy.Common.Contracts;
using Microsoft.Knowzy.Domain;
using Microsoft.Knowzy.WPF.Messages;
using Microsoft.Knowzy.WPF.ViewModels.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Knowzy.Authentication;

namespace Microsoft.Knowzy.WPF.ViewModels
{
    public class MainViewModel : Screen
    {
        private const int TabListView = 0;
        private const int TabGridView = 1;
        private const int TabReviewsView = 2;
        //private const int TabSentimentsView = 3;
        private readonly IDataProvider _dataProvider;
        public readonly IEventAggregator _eventAggregator;
        private readonly IAuthenticationService _authenticationService;


        public MainViewModel(IDataProvider dataProvider, IEventAggregator eventAggregator, IAuthenticationService authenticationService)
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            _authenticationService = authenticationService;
        }
        
        private int _selectedIndexTab;

        public int SelectedIndexTab
        {
            get => _selectedIndexTab;
            set
            {
                _selectedIndexTab = value;
                NotifyOfPropertyChange(() => SelectedIndexTab);
            }
        }

        public List<Screen> ScreenList { get; set; }

        public ObservableCollection<ItemViewModel> DevelopmentItems { get; set; } = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ReviewViewModel> DevItems2 { get; set; } = new ObservableCollection<ReviewViewModel>();
        public ObservableCollection<SentimentViewModel> DevItems3 { get; set; } = new ObservableCollection<SentimentViewModel>();
        public string LoggedUser
        {
            get
            {
                NotifyOfPropertyChange(() => HasLoggedUser);
                return _authenticationService.UserLogged;
            }
        }

        public bool HasLoggedUser => !string.IsNullOrWhiteSpace(_authenticationService.UserLogged);

        protected override void OnViewAttached(object view, object context)
        {
            foreach (var item in _dataProvider.GetData())
            {
                DevelopmentItems.Add(new ItemViewModel(item, _eventAggregator));
            }

            foreach (var item in _dataProvider.GetDataReviews())
            {
                DevItems2.Add(new ReviewViewModel(item, _eventAggregator));
            }

            //foreach (var item in _dataProvider.GetDataSentiments())
            //{
            //    DevItems3.Add(new SentimentViewModel(item, _eventAggregator));
            //}

            base.OnViewAttached(view, context);
        }

        public void UpdateSentimentDevItems(Dictionary<string, List<int>> mapping)
        {
            DevItems3.Clear();

            foreach (var kvp in mapping)
            {
                Sentiment sentiment = new Sentiment();
                sentiment.Name = kvp.Key;
                sentiment.Positive = kvp.Value[0].ToString();
                sentiment.Negative = kvp.Value[1].ToString();

                DevItems3.Add(new SentimentViewModel(sentiment, _eventAggregator));
            }
        }

        public void ShowListView()
        {
            if (SelectedIndexTab == TabListView) return;
            SelectedIndexTab = TabListView;
        }

        public void ShowGridView()
        {
            if (SelectedIndexTab == TabGridView) return;
            SelectedIndexTab = TabGridView;
        }

        public void ShowReviewsView()
        {
            if (SelectedIndexTab == TabReviewsView) return;
            SelectedIndexTab = TabReviewsView;
        }

        //public void ShowSentimentsView()
        //{
        //    if (SelectedIndexTab == TabSentimentsView) return;
        //    SelectedIndexTab = TabSentimentsView;
        //}

        public void NewItem()
        {
            var item = new ItemViewModel(_eventAggregator);
            _eventAggregator.PublishOnUIThread(new EditItemMessage(item));

            if (item.Id == null) return;
            DevelopmentItems.Add(item);
        }

        public void Login()
        {
            _eventAggregator.PublishOnUIThread(new OpenLoginMessage());
        }

        public void Logout()
        {
            _authenticationService.Logout();
            NotifyOfPropertyChange(() => LoggedUser);
        }

        public void About()
        {
            _eventAggregator.PublishOnUIThread(new OpenAboutMessage());
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void Save()
        {
            var products = DevelopmentItems?.Select(item => item.Product).ToArray();
            _dataProvider.SetData(products);
        }
    }
}
