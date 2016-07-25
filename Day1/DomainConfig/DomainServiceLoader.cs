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

namespace DomainConfig
{
    public class DomainServiceLoader : MarshalByRefObject
    {
        public UserService LoadService(ServiceConfigInfo configInfo)
        {
            UserService result = null;
            UserServiceCommunicator communicator = null;
            switch (configInfo.Type)
            {
                case ServiceType.Master:
                    {
                        Sender<BllUser> sender = new Sender<BllUser>();
                        communicator = new UserServiceCommunicator(sender);
                        result = new UserService();
                    }
                    break;
                case ServiceType.Slave:
                    {
                        Receiver<BllUser> receiver = 
                            new Receiver<BllUser>(configInfo.IpEndPoint.Address, configInfo.IpEndPoint.Port);
                        communicator = new UserServiceCommunicator(receiver);
                        result = new UserService(new Slave());
                        Task task = receiver.AcceptConnection();
                        task.ContinueWith((t) => communicator.RunReceiver());
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown ServiceType");
            }
            result.Mode.AddCommunicator(communicator);
            //result.Name = AppDomain.CurrentDomain.FriendlyName;

            return result;
        }

        public void ConnectMaster(UserService master, IEnumerable<ServiceConfigInfo> slaveConfigurations)
        {
            //Console.WriteLine(RemotingServices.IsTransparentProxy(master));
            var b = slaveConfigurations.Where(c => c.IpEndPoint != null).Select(c => c.IpEndPoint);
            master.Mode.Communicator.Connect(slaveConfigurations.Where(c => c.IpEndPoint != null)
                                                           .Select(c => c.IpEndPoint));

        }

    }
}