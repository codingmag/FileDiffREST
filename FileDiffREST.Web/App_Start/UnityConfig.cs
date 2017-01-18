//-----------------------------------------------------------------------
// <copyright file="UnityConfig.cs" company=".">
//     -
// </copyright>
// <author>Mete Bulutay</author>
//-----------------------------------------------------------------------

namespace FileDiffREST.Web
{
    using Business.Interfaces;
    using Business.Services;
    using Microsoft.Practices.Unity;
    using System.Web.Http;
    using Unity.WebApi;
    
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IRetrieveService, RetrieveService>();
            container.RegisterType<IDiffService, DiffService>();
            container.RegisterType<IUploadService, UploadService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}