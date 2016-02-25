using System;
using System.IO;
using System.Reflection;

namespace Configgy.Source
{
    /// <summary>
    /// A value source that looks for files with the extension '.config', '.json', or '.xml' in the application base directory and have the same name as the requested value.
    /// </summary>
    public class FileSource : IValueSource
    {
        private readonly string[] _fileExtensions;

        public FileSource()
            : this(".conf", ".json", ".xml")
        { }

        public FileSource(params string[] fileExtensions)
        {
            _fileExtensions = fileExtensions ?? new string[0];
        }

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
