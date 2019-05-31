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

namespace Microsoft.Knowzy.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        const string ConfigurationFilePath = "Config.json";

        public ConfigurationService(IFileHelper fileHelper, IJsonHelper jsonHelper)
        {
            Configuration = jsonHelper.Deserialize<ConfigurationModel>(fileHelper.ReadTextFile(ConfigurationFilePath));
        }

        public IConfigurationModel Configuration
        {
            get;
        } 
    }
}
