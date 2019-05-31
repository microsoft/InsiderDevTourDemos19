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
using Microsoft.Knowzy.WPF.Messages;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Knowzy.WPF.Helpers;
using NitahTextSentiment.Models;
using System;

namespace Microsoft.Knowzy.WPF.ViewModels
{
    public sealed class ReviewsViewModelCopy : Screen
    {
        private readonly MainViewModel _mainViewModel;

        public ReviewsViewModelCopy(MainViewModel mainViewModel, IEventAggregator eventAggregator)
        {
            _mainViewModel = mainViewModel;
            DisplayName = Localization.Resources.ReviewsView_Tab;
        }

        public ObservableCollection<ItemViewModel> DevelopmentItems => _mainViewModel.DevelopmentItems;

        public ObservableCollection<ReviewViewModel> DevItems2 => _mainViewModel.DevItems2;

#pragma warning disable IDE1006 // Naming Styles
        public IEventAggregator eventAggregator => _mainViewModel._eventAggregator;
#pragma warning restore IDE1006 // Naming Styles


        public void SortProducts(object sortField, bool isSortAscending)
        {
            var field = sortField as string;
            if (string.IsNullOrEmpty(field)) return;

            var view = CollectionViewSource.GetDefaultView(DevItems2);

            var sortDirection = isSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(field, sortDirection));
        }


        public async void GO()
        {
            //list of reviews (devitems2)
            //then pass each review comment through Sentimentize (iteration) and
            //count pos and neg results for each review.name
            //map name, pos, neg to a Sentiment/SentimentViewModel for each review.name
            //and finally create devitems3 list based on these SentimentViewModels

            var s = new Sentimentize();
            await s.InitializeModelAsync();

            Console.WriteLine(s);

            Dictionary<string, List<int>> answer = new Dictionary<string, List<int>>();

            foreach(ReviewViewModel rvm in DevItems2)
            {
                var n = rvm.Name;
                var c = rvm.Comment;
                s.txtInput = c;
                var useless_unused = await s.Evaluate();
                var output = s.txtOutput;   //this is "Positive" or "Negative" or "Unknown"

                if (!answer.ContainsKey(n))
                {
                    var newzerolist = new List<int>();
                    newzerolist.Add(0); newzerolist.Add(0);
                    answer.Add(n, newzerolist);
                }

                if (output == "Positive")
                {
                    answer[n][0] += 1;
                }
                else if (output == "Negative")
                {
                    answer[n][1] += 1;
                }
            }

            //answer is now fully populated, mapping names to pos/neg lists
            //now map dictionary to the list of SentimentViewModels

            _mainViewModel.UpdateSentimentDevItems(answer);



            //Console.WriteLine(s.Evaluate("I love this nose soooo much!"));
            //s.txtInput = input;
            //Console.WriteLine("Entering Sentimentize.cs");
            //var useless = await s.Evaluate();
            ////Console.WriteLine(s.Evaluate("I love this nose soooo much!"));

            //txt.Wait();

            //FileHelper fh = new FileHelper();
            //fh.WriteTextFile("C:/Users/nionsong/Desktop/output_2.txt", txt);
            //Console.WriteLine("\nBack in Reviews Model.cs\n");
            //Console.WriteLine("THIS IS THE RESULT: " + s.txtOutput + "\n");
            //Console.WriteLine("\nEND RESULT\n" + s.txtOutput.GetType() + "\n^^is type of Result\n");

            eventAggregator.PublishOnUIThread(new OpenSentimentMessage());
        }


    }
}
