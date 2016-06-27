using System;

namespace Configgy
{
    /// <summary>
    /// Associates help text with a configuration property or class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HelpAttribute : Attribute
    {
        /// <summary>
        /// The help text for this property or class.
        /// </summary>
        public string HelpText { get; }

        public HelpAttribute(string helpText)
        {
            HelpText = helpText;
        }
    }
}
