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
using Microsoft.Knowzy.Common.Contracts.Helpers;

namespace Microsoft.Knowzy.WPF.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string ActualPath => AppDomain.CurrentDomain.BaseDirectory;

        public string ReadTextFile(string filePath)
        {
            using (var reader = new StreamReader(ActualPath + filePath))
            {
                return reader.ReadToEnd();
            }
        }

        public void WriteTextFile(string filePath, string content)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(content);
            }
        }
    }
}
