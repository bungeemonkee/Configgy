using System;

namespace Configgy.Source
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlternateNameAttribute : Attribute
    {
        public string AlternateName { get; }

        public AlternateNameAttribute(string alternateName)
        {
            AlternateName = alternateName;
        }
    }
}
