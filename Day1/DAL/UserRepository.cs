using DAL.Interface;
using Storage;
using Storage.Interface;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly AbstractUserStorage storage;

        public UserRepository(AbstractUserStorage storage)
        {
            this.storage = storage;
        }

        public int Add(User user)
        {
            return storage.Add(user);
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            User userToDelete = storage.Users.SingleOrDefault(u => u.Id == user.Id);
            if (userToDelete != null)
                storage.Users.Remove(userToDelete);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return storage.Users.ToList();
        }

        public int[] SearchForUsers(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException();
            List<User> foundUsers = storage.Users.Where(criteria).ToList();
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

        public void WriteToXmlFile()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            //XmlSerializer formatter = new XmlSerializer(typeof(User[]));
            using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["Path"], FileMode.Create))
            {
                formatter.Serialize(fs, storage.Users);
            }
        }

        public void ReadFromXmlFile()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            //XmlSerializer formatter = new XmlSerializer(typeof(User[]));

            using (StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["Path"]))
            {
                List<User> users = (List<User>)formatter.Deserialize(sr);
                //User[] users = (User[])formatter.Deserialize(fs);

                foreach (var user in users)
                {
                    storage.Users.Add(user);
                }
            }

        }
    }
}
