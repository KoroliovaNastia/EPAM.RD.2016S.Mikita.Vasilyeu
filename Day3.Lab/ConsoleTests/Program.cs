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
            var instantiateUserAttributes =
                (InstantiateUserAttribute[])Attribute.GetCustomAttributes(type, typeof(InstantiateUserAttribute));
            var matchParameter =
                 (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(type.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));
            var proper = type.GetProperty(matchParameter[0].Property);
            var propertyAttributes =
                (DefaultValueAttribute[])Attribute.GetCustomAttributes(proper, typeof(DefaultValueAttribute));
            int defaultValue = (int)propertyAttributes[0].Value;

            var advUserType = typeof(AdvancedUser);
            var instantiateAdvancedUserAttributes =
                (InstantiateAdvancedUserAttribute)Assembly.GetAssembly(advUserType).GetCustomAttribute(typeof(InstantiateAdvancedUserAttribute));
            var matchParameterAdvUser =
                (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(advUserType.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));
            var properAdv = advUserType.GetProperty(matchParameterAdvUser[1].Property);
            var propertyAttributesAdv =
                (DefaultValueAttribute[])Attribute.GetCustomAttributes(properAdv, typeof(DefaultValueAttribute));
            int defaultValueAdv = (int)propertyAttributesAdv[0].Value;


            var users = new User[4];
            for (int i = 0; i < users.Length; i++)
            {
                if (i == 3)
                {
                    int externalId = instantiateAdvancedUserAttributes.externalId;
                    users[i] = new AdvancedUser(instantiateAdvancedUserAttributes.id, externalId == 0 ? defaultValueAdv : externalId);
                    users[i].FirstName = instantiateAdvancedUserAttributes.firstName;
                    users[i].LastName = instantiateAdvancedUserAttributes.lastName;
                }
                else
                {
                    users[i] = new User(instantiateUserAttributes[i].id);
                    users[i].FirstName = instantiateUserAttributes[i].firstName;
                    users[i].LastName = instantiateUserAttributes[i].lastName;
                }
                if (users[i].Id == 0)
                    users[i].Id = defaultValue;
                Console.WriteLine(users[i]);
            }

            var field = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)[0];
            var idAttributes =
                (IntValidatorAttribute[])Attribute.GetCustomAttributes(field, typeof(IntValidatorAttribute));
            int min = idAttributes[0].Min;
            int max = idAttributes[0].Max;

            var firstName = type.GetProperty("FirstName");
            var firstNamePropertyAttr =
               (StringValidatorAttribute[])Attribute.GetCustomAttributes(firstName, typeof(StringValidatorAttribute));
            int maxFirstNameLenght = firstNamePropertyAttr[0].Lenght;

            var lastName = type.GetProperty("LastName");
            var lastNamePropertyAttr =
               (StringValidatorAttribute[])Attribute.GetCustomAttributes(lastName, typeof(StringValidatorAttribute));
            int maxLastNameLenght = lastNamePropertyAttr[0].Lenght;

            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].FirstName.Length > maxFirstNameLenght)
                    Console.WriteLine("Wrong FirstName!");
                if (users[i].LastName.Length > maxLastNameLenght)
                    Console.WriteLine("Wrong LastName!");
                if (users[i].Id < min || users[i].Id > max)
                    Console.WriteLine("Wrong Id!");
            }
        }
    }
}
