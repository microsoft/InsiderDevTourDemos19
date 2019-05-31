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
using Microsoft.Knowzy.Domain.Enums;

namespace Microsoft.Knowzy.Domain
{
    //public class Review
    //{
    //    public string Name { get; set; }
    //    public string Customer { get; set; }
    //    public string Comment { get; set; }
        
    //    public Review(string name, string customer, string comment)
    //    {
    //        Name = name;
    //        Customer = customer;
    //        Comment = comment;
    //    }
    //}
    public class Product
    {
        public string Id { get; set; }
        public string Engineer { get; set; }
        public string Name { get; set; }
        public string RawMaterial { get; set; }
        public DevelopmentStatus Status { get; set; }
        public DateTime DevelopmentStartDate { get; set; }
        public DateTime ExpectedCompletionDate { get; set; }
        public string SupplyManagementContact { get; set; }
        public string Notes { get; set; }
        public string ImageSource { get; set; }
        //public Review[] Reviews { get; set; }
    }
}
