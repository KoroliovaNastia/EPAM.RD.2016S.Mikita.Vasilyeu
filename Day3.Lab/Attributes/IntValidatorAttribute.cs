using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IntValidatorAttribute : Attribute
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public IntValidatorAttribute(int first, int last)
        {
            Min = first;
            Max = last;
        }
    }
}
