using BLL;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using BLL.Modes;

namespace DomainConfig
{
    public class DomainServiceLoader : MarshalByRefObject
    {
        public UserService LoadService(string assemblyString, ServiceConfiguration configuration)
        {
            //ServiceConfigurator includes Service dll so we don't need to Load in explicitly
            //var assembly = Assembly.LoadFrom(assemblyString); 
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Console.WriteLine("Assemblies: ");
            foreach (var assembly in assemblies)
            {
                Console.WriteLine(assembly.FullName);
            }
            //temporary way to initialize components
            //INumGenerator generator = new EvenIdGenerator();
            //ValidatorBase<User> validator = new SimpleUserValidator();
            //IUserXmlFileWorker worker = null;
            //if (configuration.FilePath != null)
            //{
            //    worker = new UserXmlFileWorker();
            //}


            //IRepository<User> repository = new UserRepository(worker, configuration.FilePath);

            switch (configuration.Type)
            {
                case ServiceType.Master: return new UserService();
                case ServiceType.Slave: return new UserService(new Slave());
                default:
                    return null;
            }
        }


    }
}