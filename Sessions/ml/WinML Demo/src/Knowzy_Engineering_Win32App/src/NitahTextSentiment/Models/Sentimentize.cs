using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Windows;
using Microsoft.Knowzy.WPF;
using System.Reflection;

//namespace NitahTextSentiment.Models
//{
//    public class Sentimentize
//    {
//        private SentimentPolarityModel _model;

//        public Sentimentize()
//        {
//            InitializeAsync();
//        }

//        public  async void InitializeAsync()
//        {
//            var storageFile = await StorageFile.GetFileFromPathAsync(Directory.GetCurrentDirectory() + "\\Models\\SentimentPolarity.onnx");

//            _model = await SentimentPolarityModel.CreateFromStreamAsync(storageFile);
//        }

//        public async Task<string> Evaluate(string user_input)
//        {
//            SentimentPolarityInput input = new SentimentPolarityInput();

//            var text = user_input;


//            var words = text.Trim().Split(' ');

//            Console.WriteLine("Words: ");
//            words.ToList().ForEach(x => Console.WriteLine(x));
//            if (words.Count() < 5)
//                return "Unknown";

//            Dictionary<string, float> wordCounts = words
//                .GroupBy(word => word)
//                .ToDictionary(
//                    kvp => kvp.Key,
//                    kvp => (float)kvp.Count());

//            input.input = wordCounts;

//            Console.WriteLine("\nDictionary Input to Model");
//            Console.WriteLine( "\nWord Count Dict: ");
//            wordCounts.ToList().ForEach(x => Console.Write("\n" + x.Key + "\t" + x.Value.ToString() + "\n"));
//            Console.Write(input.input);
//            Console.Write("\nYES!\n");

//            var output = await _model.EvaluateAsync(input);

//            Console.WriteLine("\nOutput is: \n");
//            Console.Write(output.classLabel?.GetAsVectorView()?[0] + "output class label vector view!!!\n");


//            switch (output.classLabel?.GetAsVectorView()?[0])
//            {
//                case "Pos":
//                    return "Positive";

//                case "Neg":
//                    return "Negative";

//                default:
//                    return "Unknown";
//            }
//        }
//    }
//}



namespace NitahTextSentiment.Models
{

    public class Sentimentize
    {    

        private SentimentPolarityModel _model;
        private StorageFile _storageFile;
        private string _modelName;

        public string txtInput { get; set; }
        public string txtOutput { get; set; }

        public Sentimentize()
        {            
            _modelName = "SentimentPolarity.onnx";               
        }

        public async Task InitializeModelAsync()
        {            
            StorageFile tempStorageFile;
            var entryAssembly = Assembly.GetEntryAssembly().Location;
            int index = entryAssembly.LastIndexOf("\\");
            string modelPath = $"{entryAssembly.Substring(0, index)}\\Models\\{_modelName}";
            tempStorageFile = await StorageFile.GetFileFromPathAsync(modelPath);            
            _storageFile = tempStorageFile;
            this._model = await SentimentPolarityModel.CreateFromStreamAsync(_storageFile);
        }
       
        public async Task<string> Evaluate()
        {
            SentimentPolarityInput input = new SentimentPolarityInput();

            var text = txtInput;
            var words = text.Trim().Split(' ');
            if (words.Count() < 5)
                return "Unknown";
            Dictionary<string, float> wordCounts = words
                .GroupBy(word => word)
                .ToDictionary( 
                    kvp => kvp.Key,
                    kvp => (float)kvp.Count());
            input.input = wordCounts;              

            var output = await this._model.EvaluateAsync(input);                       
            Console.Write(output.classLabel?.GetAsVectorView()?[0]);            

            switch (output.classLabel?.GetAsVectorView()?[0])
            {
                case "Pos":
                    txtOutput = "Positive";
                    break;
                case "Neg":
                    txtOutput = "Negative";
                    break;
                default:
                    txtOutput = "Unknown";
                    break;
            }

            return txtOutput;
        }
    }
}
