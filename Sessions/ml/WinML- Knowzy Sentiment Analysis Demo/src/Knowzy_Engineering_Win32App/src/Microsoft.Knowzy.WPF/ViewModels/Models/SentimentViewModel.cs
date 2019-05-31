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
    public class SentimentViewModel : PropertyChangedBase
    {
        private readonly Sentiment _sentiment;
        private readonly IEventAggregator _eventAggregator;

        public SentimentViewModel(IEventAggregator eventAggregator)
        {
            _sentiment = new Sentiment();
            _eventAggregator = eventAggregator;
        }

        public SentimentViewModel(Sentiment sentiment, IEventAggregator eventAggregator)
        {
            _sentiment = sentiment;
            _eventAggregator = eventAggregator;
        }

        public Sentiment sentiment
        {
            get => _sentiment;
        }

        public string Name
        {
            get => _sentiment.Name;
            set
            {
                _sentiment.Name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Positive
        {
            get => _sentiment.Positive;
            set
            {
                _sentiment.Positive = value;
                NotifyOfPropertyChange(() => Positive);
            }
        }

        public string Negative
        {
            get => _sentiment.Negative;
            set
            {
                _sentiment.Negative = value;
                NotifyOfPropertyChange(() => Negative);
            }
        }


        public void EditItem()
        {
            //_eventAggregator.PublishOnUIThread(new EditItemMessage(this));
            //TO IMPLEMENT in EDITITEMVIEWMODEL.CS and etc.
        }
    }
}
