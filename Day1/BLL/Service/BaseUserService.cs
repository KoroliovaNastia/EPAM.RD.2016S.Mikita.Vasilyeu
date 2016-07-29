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

namespace BLL.Service
{
    public abstract class BaseUserService : MarshalByRefObject
    {
        public UserServiceCommunicator Communicator { get; set ; }
        protected IUserRepository storage;
        protected static readonly Logger logger;
        protected static readonly BooleanSwitch loggerSwitch;
        protected ReaderWriterLockSlim storageLock = new ReaderWriterLockSlim();

        static BaseUserService()
        {
            logger = LogManager.GetCurrentClassLogger();
            loggerSwitch = new BooleanSwitch("Data", "DataAccess module");
        }
        protected BaseUserService() : this(new UserRepository()) { }

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
            storageLock.EnterWriteLock();
            try
            {
                if (ReferenceEquals(user, null))
                {
                    ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                    if (loggerSwitch.Enabled)
                        logger.Error(exeption.Message);
                    throw exeption;
                }
                return NotifyAdd(user);
            }
            finally
            {
                storageLock.ExitWriteLock();
            }

        }

        public void Delete(BllUser user)
        {
            storageLock.EnterWriteLock();
            try
            {
                if (ReferenceEquals(user, null))
                {
                    ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                    if (loggerSwitch.Enabled)
                        logger.Error(exeption.Message);
                    throw exeption;
                }
                NotifyDelete(user);
            }
            finally
            {
                storageLock.ExitWriteLock();
            }

        }

        public virtual List<int> SearchForUsers(Func<BllUser, bool> criteria)
        {
            storageLock.EnterReadLock();
            try
            {
                if (ReferenceEquals(criteria, null))
                {
                    ArgumentNullException exeption = new ArgumentNullException(nameof(criteria) + " is null");
                    if (loggerSwitch.Enabled)
                        logger.Error(exeption.Message);
                    throw exeption;
                }
                Func<DalUser, bool> predicate = user => criteria.Invoke(user.ToBllUser());
                return storage.GetByPredicate(predicate).ToList();
            }
            finally
            {
                storageLock.ExitReadLock();
            }

        }

        public virtual void AddCommunicator(UserServiceCommunicator communicator)
        {
            if (communicator == null)
                return;
            Communicator = communicator;
        }

        protected abstract int NotifyAdd(BllUser user);
        protected abstract void NotifyDelete(BllUser user);
        public abstract void Save();
        public abstract void Load();
    }
}
