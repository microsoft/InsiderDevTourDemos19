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

using Microsoft.Knowzy.Common.Contracts;
using Microsoft.Knowzy.Common.Contracts.Helpers;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.DataProvider
{
    public class JsonDataProvider : IDataProvider
    {
        private readonly IConfigurationService _configuration;
        private readonly IFileHelper _fileHelper;
        private readonly IJsonHelper _jsonHelper;

        public JsonDataProvider(IConfigurationService configuration, IFileHelper fileHelper, IJsonHelper jsonHelper)
        {
            _configuration = configuration;
            _fileHelper = fileHelper;
            _jsonHelper = jsonHelper;
        }

        public Product[] GetData()
        {
            var jsonFilePath = _configuration.Configuration.JsonFilePath;
            

            Product[] products = _jsonHelper.Deserialize<Product[]>(_fileHelper.ReadTextFile(jsonFilePath));
            return products;

            //debug code here//
            //string lines = "";
            //foreach (Product p in products)
            //{
            //    for (int i = 0; i<5; i++)
            //    {
            //        lines += p?.Reviews?[i].Customer + "\t" + p?.Reviews?[i].Comment + "\n";
            //    }
            //    //lines += p.Reviews?[0]?.Customer + "\t" + p.Reviews?[0] + "\n";
            //}
            //_fileHelper.WriteTextFile("C:/Users/nionsong/Desktop/output.txt", lines);

            //return products;
            //end debug code//
        }

        public Review[] GetDataReviews()
        {
            var jsonFilePath2 = _configuration.Configuration.JsonFilePathReviews;

            return _jsonHelper.Deserialize<Review[]>(_fileHelper.ReadTextFile(jsonFilePath2));

        }

        public Sentiment[] GetDataSentiments()
        {
            var jsonFilePath3 = _configuration.Configuration.JsonFilePathSentiments;

            Sentiment[] sentiments = _jsonHelper.Deserialize<Sentiment[]>(_fileHelper.ReadTextFile(jsonFilePath3));

            //debug code here//
            string lines = "";
            foreach (Sentiment s in sentiments)
            {
                lines += s?.Name + "\t" + s?.Positive + "\t" + s?.Negative + "\n";
            }
            _fileHelper.WriteTextFile("C:/Users/nionsong/Desktop/output_sentiments.txt", lines);

            return sentiments;
            //end debug code//

        }

        public void SetData(Product[] products)
        {
            var jsonFilePath = _configuration.Configuration.JsonFilePath;

            _fileHelper.WriteTextFile(jsonFilePath, _jsonHelper.Serialize(products));
        }

        //public void SetDataReviews(Review[] reviews)
        //{
        //    text
        //}
    }
}
