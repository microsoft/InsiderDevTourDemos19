using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
namespace Microsoft.Knowzy.WPF
{
    
    public sealed class SentimentPolarityInput
    {
        public Dictionary<string,float> input;
    }
    
    public sealed class SentimentPolarityOutput
    {
        public TensorString classLabel; // shape(1)
        public IList<Dictionary<string,float>> classProbability;
    }
    
    public sealed class SentimentPolarityModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<SentimentPolarityModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            SentimentPolarityModel learningModel = new SentimentPolarityModel();
            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<SentimentPolarityOutput> EvaluateAsync(SentimentPolarityInput input)
        {
            binding.Bind("input", input.input);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new SentimentPolarityOutput();
            output.classLabel = result.Outputs["classLabel"] as TensorString;
            output.classProbability = result.Outputs["classProbability"] as IList<Dictionary<string,float>>;
            return output;
        }
    }
}
