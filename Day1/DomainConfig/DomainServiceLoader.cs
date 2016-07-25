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
        //public UserService LoadService(string type)
        //{
        //    switch (type.ToLower())
        //    {
        //        case "master":
        //            return new UserService();
        //        case "slave":
        //            return new UserService(new Slave());
        //        default:
        //            return null;
        //    }
        //}

        public UserService LoadService(ServiceConfigInfo configInfo)
        {
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //Console.WriteLine("Assemblies: ");
            //foreach (var assembly in assemblies)
            //{
            //    Console.WriteLine(assembly.FullName);
            //}
            //temporary way to initialize components
            //INumGenerator generator = new EvenIdGenerator();
            //ValidatorBase<User> validator = new SimpleUserValidator();
            //IUserXmlFileWorker worker = null;
            //if (configuration.FilePath != null)
            //{
            //    worker = new UserXmlFileWorker();
            //}
            //IRepository<User> repository = new UserRepository(worker, configuration.FilePath);
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
            master.Mode.Communicator.Connect(slaveConfigurations.Where(c => c.IpEndPoint != null)
                                                           .Select(c => c.IpEndPoint));

        }

    }
}