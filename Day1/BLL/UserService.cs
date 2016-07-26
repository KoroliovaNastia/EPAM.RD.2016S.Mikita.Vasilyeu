using BLL.Interface;
using BLL.Models;
using DAL;
using DAL.Interface;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BLL.Mappers;
using BLL.Modes;
using NLog;
using System.Diagnostics;

namespace BLL
{
    [Serializable]
    public class UserService : MarshalByRefObject, IUserService
    {
        private IUserRepository storage;
        private static readonly Logger logger;
        private static readonly BooleanSwitch loggerSwitch;

        public IMode Mode { get; }

        static UserService()
        {
            logger = LogManager.GetCurrentClassLogger();
            loggerSwitch = new BooleanSwitch("Data", "DataAccess module");
        }

        public UserService():this(new UserRepository(), Master.Instance) { }

        public UserService(IMode mode):this(new UserRepository(), mode) { }

        public UserService(IUserRepository storage, IMode mode)
        {
            if (ReferenceEquals(storage, null))
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(storage) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            if (ReferenceEquals(mode, null))
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(mode) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            if (mode.IsActivated)
            {
                ArgumentException exeption = new ArgumentException(nameof(mode) + " can be activate only once");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            mode.Activate();
            this.Mode = mode;
            this.storage = storage;
        }

        public int Add(BllUser user)
        {
            if (ReferenceEquals(user, null))
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            Mode.AddNotify();
            return storage.Add(user.ToDalUser());
        }

        public void Delete(BllUser user)
        {
            if (ReferenceEquals(user, null))
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            Mode.DeleteNotify();
            storage.Delete(user.ToDalUser());
        }

        public List<BllUser> GetAllUsers()
        {
            return storage.GetAll().Select(user => user.ToBllUser()).ToList();
        }

        public int[] SearchForUsers(Func<BllUser, bool> criteria)
        {
            //if (ReferenceEquals(criteria, null))
            //{
            //    ArgumentNullException exeption = new ArgumentNullException(nameof(criteria) + " is null");
            //    if (loggerSwitch.Enabled)
            //        logger.Error(exeption.Message);
            //    throw exeption;
            //}
            //Func<DalUser, bool> predicate = user => criteria.Invoke(user.ToBllUser());
            //return storage.GetByPredicate(predicate);
            return null;
        }

        public void Save()
        {
            Mode.SaveNotify();
            storage.Save();
        }

        public void Load()
        {
            storage.Load();
        }
    }
}
