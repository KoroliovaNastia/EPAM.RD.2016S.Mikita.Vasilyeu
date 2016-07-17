using System;

namespace Attributes
{
    // Should be applied to classes only.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InstantiateUserAttribute : Attribute
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public InstantiateUserAttribute(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public InstantiateUserAttribute(int id, string firstName, string lastName)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
        }
    }
}
