using BL.Interface;
using BL.Login;
using DAL.Login;
using DAL.Utilities.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace WebPOS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ILogin, LoginDAL>();
            container.RegisterType<(ILoginBL, LoginBL)>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}