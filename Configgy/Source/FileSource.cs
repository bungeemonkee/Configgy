using System;
using System.IO;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that looks for files with the extension '.config', '.json', or '.xml' in the application base directory and have the same name as the requested value.
    /// </summary>
    public class FileSource : ValueSourceAttributeBase
    {
        private readonly string[] _fileExtensions;

        /// <summary>
        /// Creates a default FileSource that looks for files with the extensions '.conf', '.json', or '.xml'.
        /// </summary>
        public FileSource()
            : this(".conf", ".json", ".xml")
        { }

        /// <summary>
        /// Creates a FileSource that looks for files with the given extensions.
        /// </summary>
        public FileSource(params string[] fileExtensions)
        {
            _fileExtensions = fileExtensions ?? new string[0];
        }

        /// <summary>
        /// Get the raw configuration value from the source.
        /// </summary>
        /// <param name="valueName">The name of the value to get.</param>
        /// <param name="property">If there is a property on the <see cref="Config"/> instance that matches the requested value name then this will contain the reference to that property.</param>
        /// <param name="value">The value found in the source.</param>
        /// <returns>True if the config value was found in the source, false otherwise.</returns>
        public override bool Get(string valueName, PropertyInfo property, out string value)
        {
            var baseDirectory = AppContext.BaseDirectory;

            foreach (var extension in _fileExtensions)
            {
                var path = Path.Combine(baseDirectory, valueName + extension);
                try
                {
                    value = File.ReadAllText(path);
                    return true;
                }
                catch (FileNotFoundException)
                {
                    // Ignore files that are not found, but other errors are actual errors
                }
            }

            value = null;
            return false;
        }
    }
}
