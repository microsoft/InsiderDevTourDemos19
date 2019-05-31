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
using Microsoft.Knowzy.Domain;
using Microsoft.Knowzy.Domain.Enums;
using Microsoft.Knowzy.WPF.Messages;

namespace Microsoft.Knowzy.WPF.ViewModels.Models
{
    public class ReviewViewModel : PropertyChangedBase
    {
        private readonly Review _review;
        private readonly IEventAggregator _eventAggregator;

        public ReviewViewModel(IEventAggregator eventAggregator)
        {
            _review = new Review();
            _eventAggregator = eventAggregator;
        }

        public ReviewViewModel(Review review, IEventAggregator eventAggregator)
        {
            _review = review;
            _eventAggregator = eventAggregator;
        }

        public Review review
        {
            get => _review;
        }

        public string Name
        {
            get => _review.Name;
            set
            {
                _review.Name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Customer
        {
            get => _review.Customer;
            set
            {
                _review.Customer = value;
                NotifyOfPropertyChange(() => Customer);
            }
        }

        public string Comment
        {
            get => _review.Comment;
            set
            {
                _review.Comment = value;
                NotifyOfPropertyChange(() => Comment);
            }
        }


        public void EditItem()
        {
            //_eventAggregator.PublishOnUIThread(new EditItemMessage(this));
            //TO IMPLEMENT in EDITITEMVIEWMODEL.CS and etc.
        }
    }
}
