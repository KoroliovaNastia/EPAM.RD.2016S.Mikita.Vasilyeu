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
        public UserService LoadService(string type)
        {
            switch (type.ToLower())
            {
                case "master":
                    return new UserService();
                case "slave":
                    return new UserService(new Slave());
                default:
                    return null;
            }
        }

    }
}