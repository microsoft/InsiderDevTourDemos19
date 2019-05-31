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
using Autofac;
using Caliburn.Micro;
using Microsoft.Knowzy.WPF.ViewModels;
using Microsoft.Knowzy.Configuration;
using Microsoft.Knowzy.Common.Contracts;
using Microsoft.Knowzy.DataProvider;
using System.Threading;
using Microsoft.Knowzy.Authentication;
using Microsoft.Knowzy.Common.Contracts.Helpers;
using Microsoft.Knowzy.WPF.Helpers;

namespace Microsoft.Knowzy.WPF
{
    public class AppBootstrapper : BootstrapperBase
    {
        IContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();

            builder.RegisterType<ShellViewModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<EditItemViewModel>().SingleInstance();
            builder.RegisterType<AboutViewModel>().SingleInstance();
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<KanbanViewModel>().SingleInstance();
            builder.RegisterType<ListProductsViewModel>().SingleInstance();
            builder.RegisterType<ReviewsViewModel>().SingleInstance();
            builder.RegisterType<SentimentsViewModel>().SingleInstance();

            builder.RegisterType<JsonDataProvider>().As<IDataProvider>().SingleInstance();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().SingleInstance();
            builder.RegisterType<Authentication.AuthenticationService>().As<IAuthenticationService>().SingleInstance();

            builder.RegisterType<FileHelper>().As<IFileHelper>().SingleInstance();
            builder.RegisterType<JsonHelper>().As<IJsonHelper>().SingleInstance();

            _container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.Resolve(service);
            if (instance != null)
            {
                return instance;
            }
            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}