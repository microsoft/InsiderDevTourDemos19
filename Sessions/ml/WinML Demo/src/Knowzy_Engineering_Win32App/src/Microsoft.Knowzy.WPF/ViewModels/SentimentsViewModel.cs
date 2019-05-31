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

using Caliburn.Micro;
using Microsoft.Knowzy.WPF.ViewModels.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Microsoft.Knowzy.WPF.ViewModels
{
    public class SentimentsViewModel : Screen
    {
        private readonly MainViewModel _mainViewModel;

        public SentimentsViewModel(MainViewModel mainViewModel, IEventAggregator eventAggregator)
        {
            _mainViewModel = mainViewModel;
            DisplayName = Localization.Resources.SentimentsView_Tab;
        }

        public ObservableCollection<ItemViewModel> DevelopmentItems => _mainViewModel.DevelopmentItems;

        public ObservableCollection<SentimentViewModel> DevItems3 => _mainViewModel.DevItems3;
        public ObservableCollection<ReviewViewModel> DevItems2 => _mainViewModel.DevItems2;

        //public ObservableCollection<SentimentViewModel> DevItems3;

        //public void UpdateSentimentDevItems(Dictionary<string, List<int>> mapping)
        //{
        //    DevItems3.Clear();


        //    //DevItems3.Add(new SentimentViewModel(item, _eventAggregator))
        //}

        public void SortProducts(object sortField, bool isSortAscending)
        {
            var field = sortField as string;
            if (string.IsNullOrEmpty(field)) return;

            var view = CollectionViewSource.GetDefaultView(DevItems3);

            var sortDirection = isSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(field, sortDirection));
        }


    }
}
