﻿using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig
{
    public static class UserServiceCreator
    {
        public static UserService CreateService(ServiceConfiguration configuration)
        {

            var domain = AppDomain.CreateDomain(configuration.Name, null, null);
            var type = typeof(DomainServiceLoader);
            var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
            Console.WriteLine("Creating service " + configuration.Name);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"");

            return loader.LoadService(path, configuration);
        }
    }
}
