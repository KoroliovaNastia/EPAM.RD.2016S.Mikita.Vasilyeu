using BLL;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using BLL.Modes;
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
            BaseUserService result;
            UserServiceCommunicator communicator;
            IUserRepository rep = new UserRepository();
            switch (configInfo.Type)
            {
                case ServiceType.Master:
                    {
                        Sender<BllUser> sender = new Sender<BllUser>();
                        communicator = new UserServiceCommunicator(sender);
                        result = new MasterUserService(rep);
                    }
                    break;
                case ServiceType.Slave:
                    {
                        Receiver<BllUser> receiver = 
                            new Receiver<BllUser>(configInfo.IpEndPoint.Address, configInfo.IpEndPoint.Port);
                        communicator = new UserServiceCommunicator(receiver);
                        result = new SlaveUserService(rep);
                        Task task = receiver.AcceptConnection();
                        task.ContinueWith((t) => communicator.RunReceiver());
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown ServiceType!");
            }
            result.AddCommunicator(communicator);

            return result;
        }

        public void ConnectMaster(MasterUserService master, IEnumerable<ServiceConfigInfo> slaveConfigurations)
        {
            master.Communicator.Connect(slaveConfigurations.Where(c => c.IpEndPoint != null)
                                                           .Select(c => c.IpEndPoint));
        }

    }
}