using System;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public int externalId { get; set; }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName, int externalId) 
            : base(id, firstName, lastName)
        {
            this.externalId = externalId;
        }

    }
}
