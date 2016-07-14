using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;
using System.ComponentModel;
using System.Reflection;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(User);
            InstantiateUserAttribute[] instantiateUserAttributes =
                (InstantiateUserAttribute[])Attribute.GetCustomAttributes(type, typeof(InstantiateUserAttribute));

            MatchParameterWithPropertyAttribute[] matchParameter =
                (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(type.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));

            var proper = type.GetProperty(matchParameter[0].Property);

            DefaultValueAttribute[] propertyAttributes =
                (DefaultValueAttribute[])Attribute.GetCustomAttributes(proper, typeof(DefaultValueAttribute));

            int defaultValue = (int)propertyAttributes[0].Value;

            User[] users = new User[3];
            for (int i = 0; i < users.Length; i++)
            {
                users[i] = new User(instantiateUserAttributes[i].id);
                if (users[i].Id == 0)
                    users[i].Id = defaultValue;
                users[i].FirstName = instantiateUserAttributes[i].firstName;
                users[i].LastName = instantiateUserAttributes[i].lastName;
                Console.WriteLine(users[i]);
            }

            var field = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)[0];
            IntValidatorAttribute[] idAttributes =
                (IntValidatorAttribute[])Attribute.GetCustomAttributes(field, typeof(IntValidatorAttribute));
            int min = idAttributes[0].Min;
            int max = idAttributes[0].Max;

            var firstName = type.GetProperty("FirstName");
            StringValidatorAttribute[] firstNamePropertyAttr =
               (StringValidatorAttribute[])Attribute.GetCustomAttributes(firstName, typeof(StringValidatorAttribute));
            int maxFirstNameLenght = firstNamePropertyAttr[0].Lenght;

            var lastName = type.GetProperty("LastName");
            StringValidatorAttribute[] lastNamePropertyAttr =
               (StringValidatorAttribute[])Attribute.GetCustomAttributes(lastName, typeof(StringValidatorAttribute));
            int maxLastNameLenght = lastNamePropertyAttr[0].Lenght;

            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].FirstName.Length > maxFirstNameLenght)
                    Console.WriteLine("Wrong FirstName!");
                if (users[i].LastName.Length > maxLastNameLenght)
                    Console.WriteLine("Wrong LastName!");
                if(users[i].Id<min|| users[i].Id>max)
                    Console.WriteLine("Wrong Id!");
            }


        }
    }
}
