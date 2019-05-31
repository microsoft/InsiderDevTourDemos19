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
    public class ItemViewModel : PropertyChangedBase
    {
        private readonly Product _product;
        private readonly IEventAggregator _eventAggregator;

        public ItemViewModel(IEventAggregator eventAggregator)
        {
            _product = new Product();
            _eventAggregator = eventAggregator;
        }

        public ItemViewModel(Product product, IEventAggregator eventAggregator)
        {
            _product = product;
            _eventAggregator = eventAggregator;
        }

        public Product Product
        {
            get => _product;
        }

        public string Id
        {
            get => _product.Id;
            set
            {
                _product.Id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string Engineer
        {
            get => _product.Engineer;
            set
            {
                _product.Engineer = value;
                NotifyOfPropertyChange(() => Engineer);
            }
        }

        public string Name
        {
            get => _product.Name;
            set
            {
                _product.Name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string RawMaterial
        {
            get => _product.RawMaterial;
            set
            {
                _product.RawMaterial = value;
                NotifyOfPropertyChange(() => RawMaterial);
            }
        }

        public DevelopmentStatus Status
        {
            get => _product.Status;
            set
            {
                _product.Status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public DateTime DevelopmentStartDate
        {
            get => _product.DevelopmentStartDate;
            set
            {
                _product.DevelopmentStartDate = value;
                NotifyOfPropertyChange(() => DevelopmentStartDate);
            }
        }

        public DateTime ExpectedCompletionDate
        {
            get => _product.ExpectedCompletionDate;
            set
            {
                _product.ExpectedCompletionDate = value;
                NotifyOfPropertyChange(() => ExpectedCompletionDate);
            }
        }

        public string SupplyManagementContact
        {
            get => _product.SupplyManagementContact;
            set
            {
                _product.SupplyManagementContact = value;
                NotifyOfPropertyChange(() => SupplyManagementContact);
            }
        }

        public string Notes
        {
            get => _product.Notes;
            set
            {
                _product.Notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        public string ImageSource
        {
            get => _product.ImageSource;
            set
            {
                _product.ImageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        //public Review[] Reviews
        //{
        //    get => _product.Reviews;
        //    set
        //    {
        //        _product.Reviews = value;
        //        NotifyOfPropertyChange(() => Reviews);
        //    }
        //}

        public void EditItem()
        {
            _eventAggregator.PublishOnUIThread(new EditItemMessage(this));
        }
    }
}
