using System;

namespace Configgy.Source
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlternateNameAttribute : Attribute
    {
        public string AlternateName { get; }
        
        public int Priority { get; }

        public AlternateNameAttribute(string alternateName)
        {
            AlternateName = alternateName;
            Priority = 0;
        }

        public AlternateNameAttribute(string alternateName, int priority)
        {
            AlternateName = alternateName;
            Priority = priority;
        }
    }
}
