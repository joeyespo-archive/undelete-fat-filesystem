using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Undelete
{
    /// <summary>
    /// Defines an attribute used to specify the length of an array on a field within a structure.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArrayLengthAttribute : Attribute
    {
        private int arrayLength;

        public ArrayLengthAttribute(int arrayLength)
        {
            this.arrayLength = arrayLength;
        }

        public int ArrayLength
        {
            get
            {
                return arrayLength;
            }
        }
    }
}
