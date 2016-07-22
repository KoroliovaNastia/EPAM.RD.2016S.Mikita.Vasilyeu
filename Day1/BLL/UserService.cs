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
        private IMode mode;

        private static readonly Logger logger;
        private static readonly BooleanSwitch loggerSwitch;

        static UserService()
        {
            logger = LogManager.GetCurrentClassLogger();
            loggerSwitch = new BooleanSwitch("Data", "DataAccess module");
        }

        public UserService()
        {
            InitializeService(new UserRepository(), Master.Instance);
        }

        public UserService(IMode mode)
        {
            InitializeService(new UserRepository(), mode);
        }

        public UserService(IUserRepository storage, IMode mode)
        {
            InitializeService(storage, mode);
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
            mode.AddNotify();
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
            mode.DeleteNotify();
            storage.Delete(user.ToDalUser());
        }

        public List<BllUser> GetAllUsers()
        {
            return storage.GetAll().Select(user => user.ToBllUser()).ToList();
        }

        public int[] SearchForUsers(Func<BllUser, bool> criteria)
        {
            if (ReferenceEquals(criteria, null))
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(criteria) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            Func<DalUser, bool> predicate = user => criteria.Invoke(user.ToBllUser());
            return storage.GetByPredicate(predicate);
        }

        public void Save()
        {
            mode.SaveNotify();
            storage.Save();
        }

        public void Load()
        {
            storage.Load();
        }

        private void InitializeService(IUserRepository storage, IMode mode)
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
            this.mode = mode;
            this.storage = storage;
        }
    }
}
