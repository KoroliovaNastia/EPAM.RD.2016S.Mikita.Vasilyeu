using System;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        private int id;
        private int guid;
        private string firstName;
        private string lastName;


        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName, int guid) 
            : base(id, firstName, lastName)
        {
            this.guid = guid;
        }

    }
}
