using DAL.Interface;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Configuration;
using NLog;
using System.Diagnostics;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private static readonly Logger logger;
        private static readonly BooleanSwitch loggerSwitch;

        private ICustomerEnumerator iterator;
        private IUserValidator validator;
        private List<DalUser> users;

        static UserRepository()
        {
            loggerSwitch = new BooleanSwitch("Data", "DataAccess module");
            logger = LogManager.GetCurrentClassLogger();
        }

        public UserRepository()
        {
            InitializeRepository(new EvenEnumerator(), new SimpleUserValidator());
        }

        public UserRepository(ICustomerEnumerator iterator, IUserValidator validator)
        {
            InitializeRepository(iterator, validator);
        }

        public int Add(DalUser user)
        {
            if (user == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            if (!validator.Validate(user))
            {
                ArgumentException exeption = new ArgumentException(nameof(user) + " is invalid");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            user.Id = iterator.GetNext();
            users.Add(user);
            if(loggerSwitch.Enabled)
                logger.Info($"User {user} Added!");
            return user.Id;
        }

        public void Delete(DalUser user)
        {
            if (user == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            DalUser userToDelete = users.SingleOrDefault(u => u.Id == user.Id);
            if (userToDelete == null)
            {
                ArgumentException exeption = new ArgumentException(nameof(user) + " doesn't exist");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            users.Remove(userToDelete);
            if (loggerSwitch.Enabled)
                logger.Info($"User {user} Removed!");
        }

        public IEnumerable<DalUser> GetAll()
        {
            return users.ToList();
        }

        public int[] GetByPredicate(Func<DalUser, bool> predicate)
        {
            if (predicate == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(predicate) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            List<DalUser> foundUsers = users.Where(predicate).ToList();
            int[] ids = null;
            if (foundUsers.Count != 0)
            {
                ids = new int[foundUsers.Count];
                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i] = foundUsers[i].Id;
                }
            }
            return ids;
        }

        public void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<DalUser>));
            string path;
            try
            {
                path = ConfigurationManager.AppSettings["Path"];
            }
            catch (ConfigurationErrorsException ex)
            {
                if (loggerSwitch.Enabled)
                    logger.Error($"App.Config exception! " + ex.Message);
                throw;
            }
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fs, users);
            }
            if (loggerSwitch.Enabled)
                logger.Info($"User storage saved to XML!");
        }

        public void Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<DalUser>));
            string path;
            try
            {
                path = ConfigurationManager.AppSettings["Path"];
            }
            catch (ConfigurationErrorsException ex)
            {
                if (loggerSwitch.Enabled)
                    logger.Error($"App.Config exception! " + ex.Message);
                throw;
            }
            using (StreamReader sr = new StreamReader(path))
            {
                List<DalUser> users = (List<DalUser>)formatter.Deserialize(sr);
                foreach (var user in users)
                {
                    this.users.Add(user);
                }
                iterator.SetCurrent(users.Last().Id);
            }
            if (loggerSwitch.Enabled)
                logger.Info($"User storage loaded from XML!");
        }


        private void InitializeRepository(ICustomerEnumerator iterator, IUserValidator validator)
        {
            if (iterator == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(iterator) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            if (validator == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(validator) + " is null");
                if (loggerSwitch.Enabled)
                    logger.Error(exeption.Message);
                throw exeption;
            }
            this.iterator = iterator;
            this.validator = validator;
            users = new List<DalUser>();
        }
    }
}
