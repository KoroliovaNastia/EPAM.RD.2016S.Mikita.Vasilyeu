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

namespace Storage
{
    public class UserStorage : IUserStorage
    {
        public ICustomerEnumerator iterator { get; private set; }
        public IUserValidator validator { get; private set; }
        public List<User> Users { get; set; }

        public UserStorage(ICustomerEnumerator iterator = null, IUserValidator validator = null) : base()
        {
            if (iterator != null)
                this.iterator = iterator;
            if (validator != null)
                this.validator = validator;
            Users = new List<User>();
        }

        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            if (!validator.Validate(user))
                throw new ArgumentException();
            user.Id = iterator.GetNext();
            Users.Add(user);
            return user.Id;
        }

        public void Delete(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            User userToDelete = Users.SingleOrDefault(u => u.Id == entity.Id);
            if (userToDelete != null)
                Users.Remove(userToDelete);
        }

        public IEnumerable<User> GetAll()
        {
            return Users.ToList();
        }

        public int[] GetByPredicate(Func<User, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException();
            List<User> foundUsers = Users.Where(predicate).ToList();
            int[] ids = null;
            if (foundUsers != null && foundUsers.Count != 0)
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
            using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["Path"], FileMode.Create))
            {
                formatter.Serialize(fs, Users);
            }
        }

        public void Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["Path"]))
            {
                List<User> users = (List<User>)formatter.Deserialize(sr);
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
        }
    }
}
