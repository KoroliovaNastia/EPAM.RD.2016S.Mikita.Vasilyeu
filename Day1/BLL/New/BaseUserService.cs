using BLL;
using BLL.Mappers;
using BLL.Models;
using DAL;
using DAL.DTO;
using DAL.Interface;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace BLL
{
    public abstract class BaseUserService : MarshalByRefObject
    {
        public UserServiceCommunicator Communicator { get; set; }
        protected IUserRepository storage;
        private static readonly Logger logger;
        private static readonly BooleanSwitch loggerSwitch;

        static BaseUserService()
        {
            logger = LogManager.GetCurrentClassLogger();
            loggerSwitch = new BooleanSwitch("Data", "DataAccess module");
        }
        protected BaseUserService() : this(new UserRepository())
        {

        }

        protected BaseUserService(IUserRepository storage)
        {
            if (ReferenceEquals(storage, null))
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(storage) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            this.storage = storage;

        }

        public int Add(BllUser user)
        {
            return AddStrategy(user);
        }

        public void Delete(BllUser user)
        {

            DeleteStrategy(user);
        }

        protected abstract int AddStrategy(BllUser user);
        protected abstract void DeleteStrategy(BllUser user);

        public virtual List<int> SearchForUsers(Func<BllUser, bool> criteria)
        {
            Func<DalUser, bool> predicate = user => criteria.Invoke(user.ToBllUser());
            return storage.GetByPredicate(predicate).ToList();
            //Func<DalUser, bool>[] predicate = new Func<DalUser, bool>[criteria.Length];
            //for (int i = 0; i< predicate.Length; ++i)
            //{
            //    int k = i;
            //    predicate[k] = user => criteria[k].Invoke(user.ToBllUser());
            //}
            //return storage.GetByPredicate(predicate).ToList();
        }

        public virtual void AddCommunicator(UserServiceCommunicator communicator)
        {
            if (communicator == null) return;
            Communicator = communicator;
        }

        public abstract void Save();
        public abstract void Initialize(); // get collection from xml file and get last generated Id
    }
}
