using System;
using System.IO;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that looks for files with the extension '.config', '.json', or '.xml' in the application base directory and have the same name as the requested value.
    /// </summary>
    public class FileSource : IValueSource
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
        /// <returns>The raw configuration value or null if there isn't one in this source.</returns>
        public string GetRawValue(string valueName, PropertyInfo property)
        {
            foreach (var extension in _fileExtensions)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, valueName + extension);
                try
                {
                    return File.ReadAllText(path);
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
