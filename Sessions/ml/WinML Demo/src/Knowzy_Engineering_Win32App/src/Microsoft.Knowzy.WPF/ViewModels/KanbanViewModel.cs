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
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Microsoft.Knowzy.Domain.Enums;
using Microsoft.Knowzy.WPF.ViewModels.Models;

namespace Microsoft.Knowzy.WPF.ViewModels
{
    public sealed class KanbanViewModel : Screen
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<StatusLaneViewModel> _lanes;

        public KanbanViewModel(MainViewModel mainViewModel, IEventAggregator eventAggregator)
        {
            _mainViewModel = mainViewModel;
            _eventAggregator = eventAggregator;
            DisplayName = Localization.Resources.GridView_Tab;
        }

        public ObservableCollection<ItemViewModel> DevelopmentItems => _mainViewModel.DevelopmentItems;

        public ObservableCollection<StatusLaneViewModel> Lanes
        {
            get => _lanes;
            private set
            {
                if (Equals(value, _lanes)) return;
                _lanes = value;
                NotifyOfPropertyChange(() => Lanes);
            }
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            InitializeLanes();
        }

        public void InitializeLanes()
        {
            Lanes = new ObservableCollection<StatusLaneViewModel>();
            var level = 0;
            foreach (var status in Enum.GetValues(typeof(DevelopmentStatus)))
            {
                Lanes.Add(new StatusLaneViewModel()
                {
                    Status = (DevelopmentStatus)status,
                    CascadeLevel = level,
                    Items = _mainViewModel.DevelopmentItems?.Where(item => item.Status == (DevelopmentStatus)status).ToList()
                });
                level++;
            }
        }
    }
}
