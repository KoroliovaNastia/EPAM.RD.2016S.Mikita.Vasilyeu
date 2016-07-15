using Storage.Interface;
using Storage.Models;
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

namespace Storage
{
    public class UserStorage : IUserStorage
    {
        public ICustomerEnumerator Iterator { get; }
        public IUserValidator Validator { get; }
        public List<User> Users { get; set; }

        public static BooleanSwitch LoggerSwitch { get; private set; } 
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UserStorage(ICustomerEnumerator iterator, IUserValidator validator)
        {
            if (iterator == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(iterator) + " is null");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            if (validator == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(validator) + " is null");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            Iterator = iterator;
            Validator = validator;
            Users = new List<User>();
            LoggerSwitch = new BooleanSwitch("Data", "DataAccess module");
        }

        public int Add(User user)
        {
            if (user == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            if (!Validator.Validate(user))
            {
                ArgumentException exeption = new ArgumentException(nameof(user) + " is invalid");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            user.Id = Iterator.GetNext();
            Users.Add(user);
            if(LoggerSwitch.Enabled)
                Logger.Info($"User {user} Added!");
            return user.Id;
        }

        public void Delete(User user)
        {
            if (user == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(user) + " is null");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            User userToDelete = Users.SingleOrDefault(u => u.Id == user.Id);
            if (userToDelete == null)
            {
                ArgumentException exeption = new ArgumentException(nameof(user) + " doesn't exist");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            Users.Remove(userToDelete);
            if (LoggerSwitch.Enabled)
                Logger.Info($"User {user} Removed!");
        }

        public IEnumerable<User> GetAll()
        {
            return Users.ToList();
        }

        public int[] GetByPredicate(Func<User, bool> predicate)
        {
            if (predicate == null)
            {
                ArgumentNullException exeption = new ArgumentNullException(nameof(predicate) + " is null");
                if (LoggerSwitch.Enabled)
                    Logger.Error(exeption.Message);
                throw exeption;
            }
            List<User> foundUsers = Users.Where(predicate).ToList();
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
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            string path;
            try
            {
                path = ConfigurationManager.AppSettings["Path"];
            }
            catch (ConfigurationErrorsException ex)
            {
                if (LoggerSwitch.Enabled)
                    Logger.Error($"App.Config exception! " + ex.Message);
                throw;
            }
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fs, Users);
            }
            if (LoggerSwitch.Enabled)
                Logger.Info($"User storage saved to XML!");
        }

        public void Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            string path;
            try
            {
                path = ConfigurationManager.AppSettings["Path"];
            }
            catch (ConfigurationErrorsException ex)
            {
                if (LoggerSwitch.Enabled)
                    Logger.Error($"App.Config exception! " + ex.Message);
                throw;
            }
            using (StreamReader sr = new StreamReader(path))
            {
                List<User> users = (List<User>)formatter.Deserialize(sr);
                foreach (var user in users)
                {
                    Users.Add(user);
                }
                Iterator.Current = users.Last().Id;
            }
            if (LoggerSwitch.Enabled)
                Logger.Info($"User storage loaded from XML!");
        }
    }
}
