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
using System.IO;
using Caliburn.Micro;
using Microsoft.Knowzy.Domain.Enums;
using Microsoft.Knowzy.WPF.Messages;
using Microsoft.Win32;

namespace Microsoft.Knowzy.WPF.ViewModels
{
    public sealed class EditItemViewModel : Screen
    {
        private readonly string _imagesDirectory = AppDomain.CurrentDomain.BaseDirectory + "Assets\\";
        private Models.ItemViewModel _item;
        private DevelopmentStatus _status;
        private string _rawMaterial;
        private string _name;
        private string _id;
        private string _engineer;
        private DateTime _developmentStartDate;
        private DateTime _expectedCompletionDate;
        private string _supplyManagementContact;
        private string _notes;
        private Uri _imageSource;
        private Microsoft.Knowzy.Domain.Review[] _reviews;
        private readonly IEventAggregator _eventAggregator;

        public EditItemViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DisplayName = Localization.Resources.Title_Shell;
        }

        public Models.ItemViewModel Item
        {
            get => _item;
            set
            {
                _item = value;
                Engineer = _item.Engineer;
                Id = _item.Id;
                Name = _item.Name;
                RawMaterial = _item.RawMaterial;
                DevelopmentStartDate = _item.DevelopmentStartDate;
                ExpectedCompletionDate = _item.ExpectedCompletionDate;
                SupplyManagementContact = _item.SupplyManagementContact;
                Notes = _item.Notes;
                Status = _item.Status;
                ImageSource = new Uri(_imagesDirectory + Item.ImageSource);
            }
        }

        public DateTime DevelopmentStartDate
        {
            get => _developmentStartDate;
            set
            {
                if (value.Equals(_developmentStartDate)) return;
                _developmentStartDate = value;
                NotifyOfPropertyChange(() => DevelopmentStartDate);
            }
        }

        public DateTime ExpectedCompletionDate
        {
            get => _expectedCompletionDate;
            set
            {
                if (value.Equals(_expectedCompletionDate)) return;
                _expectedCompletionDate = value;
                NotifyOfPropertyChange(() => ExpectedCompletionDate);
            }
        }

        public string SupplyManagementContact
        {
            get => _supplyManagementContact;
            set
            {
                if (value == _supplyManagementContact) return;
                _supplyManagementContact = value;
                NotifyOfPropertyChange(() => SupplyManagementContact);
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value == _notes) return;
                _notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        public DevelopmentStatus Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public string RawMaterial
        {
            get => _rawMaterial;
            set
            {
                if (value == _rawMaterial) return;
                _rawMaterial = value;
                NotifyOfPropertyChange(() => RawMaterial);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public string Engineer
        {
            get => _engineer;
            set
            {
                if (value == _engineer) return;
                _engineer = value;
                NotifyOfPropertyChange(() => Engineer);
            }
        }

        public Uri ImageSource
        {
            get => _imageSource;
            set
            {
                if (value == _imageSource) return;
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        //public Microsoft.Knowzy.Domain.Review[] Reviews
        //{
        //    get => _reviews;
        //    set
        //    {
        //        if (value == _reviews) return;
        //        _reviews = (Microsoft.Knowzy.Domain.Review[]) value.Clone();
        //        NotifyOfPropertyChange(() => Reviews);
        //    }
        //}



        public void UploadPhoto()
        {
            var openFileDialog = new OpenFileDialog {Filter = "Image Files(*.BMP;*.JPG;*.PNG;*.GIF)|*.BMP;*.JPG;*.GIF;*.PNG"};
            if (openFileDialog.ShowDialog() == true)
            {
                var storeFilePath = _imagesDirectory + Path.GetFileName(openFileDialog.FileName);
                if (!File.Exists(storeFilePath))
                {
                    File.Copy(openFileDialog.FileName, storeFilePath);
                }
                ImageSource = new Uri(storeFilePath);
            }
        }

        public void CloseEditWindow()
        {
            TryClose();
        }

        public void SaveAndCloseEditWindow()
        {
            var previousStatus = _item.Status;
            _item.Engineer = Engineer;
            _item.Id = Id;
            _item.Name = Name;
            _item.RawMaterial = RawMaterial;
            _item.DevelopmentStartDate = DevelopmentStartDate;
            _item.ExpectedCompletionDate = ExpectedCompletionDate;
            _item.SupplyManagementContact = SupplyManagementContact;
            _item.Notes = Notes;
            _item.Status = Status;
            _item.ImageSource = ImageSource.LocalPath.Replace(_imagesDirectory, string.Empty);
            //changeCH _item.Reviews = Reviews
            _eventAggregator.PublishOnUIThread(new UpdateLanesMessage(_item, previousStatus));
            CloseEditWindow();
        }
    }
}
