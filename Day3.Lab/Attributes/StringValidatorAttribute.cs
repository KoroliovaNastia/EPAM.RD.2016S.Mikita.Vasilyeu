using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringValidatorAttribute : Attribute
    {
        public int Lenght { get; set; }

        public StringValidatorAttribute(int length)
        {
            Lenght = length;
        }
    }
}
