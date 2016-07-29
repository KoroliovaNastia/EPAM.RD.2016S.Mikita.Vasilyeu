using BLL;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using NetworkConfig;
using BLL.Models;
using System.Threading.Tasks;
using System.Linq;
using DAL;
using BLL.Service;

namespace DomainConfig
{
    public class DomainServiceLoader : MarshalByRefObject
    {
        public BaseUserService LoadService(ServiceConfigInfo configInfo)
        {
            BaseUserService service;
            UserServiceCommunicator communicator;
            IUserRepository rep = new UserRepository();
            switch (configInfo.Type)
            {
                case ServiceType.Master:
                    {
                        Sender<BllUser> sender = new Sender<BllUser>();
                        communicator = new UserServiceCommunicator(sender);
                        service = new MasterUserService(rep);
                    }
                    break;
                case ServiceType.Slave:
                    {
                        Receiver<BllUser> receiver = 
                            new Receiver<BllUser>(configInfo.IpEndPoint.Address, configInfo.IpEndPoint.Port);
                        communicator = new UserServiceCommunicator(receiver);
                        service = new SlaveUserService(rep);
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown ServiceType!");
            }
            service.AddCommunicator(communicator);

            return service;
        }

    }
}