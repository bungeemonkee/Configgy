using System;
using System.IO;

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

        /// <inheritdoc cref="IValueSource.Get"/>
        public override bool Get(IConfigProperty property, out string value)
        {
            var baseDirectory = AppContext.BaseDirectory;

            foreach (var extension in _fileExtensions)
            {
                var path = Path.Combine(baseDirectory, property.ValueName + extension);
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
