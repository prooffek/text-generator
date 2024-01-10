using System.Reflection;
using TextGenerator.Application.Interfaces;

namespace TextGenerator.Application.Services
{
    public class TextFileReader : IFileReader
    {
        public string ReadFile(string relativePath)
        {
            ValidateRelativePath(relativePath);

            string fullPath = GetFullPath(relativePath);

            CheckIfFileExists(fullPath);

            using var streamReader = new StreamReader(fullPath);
            var content = streamReader.ReadToEnd();

            ValidateFileContent(content, fullPath);

            return content;
        }

        private string GetFullPath(string relativePath)
        {
            string? pathBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrWhiteSpace(pathBase))
                throw new Exception("Base path not set.");

            return Path.Combine(pathBase, relativePath);
        }

        private void ValidateRelativePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new NullReferenceException("File path cannot be null or empty.");

            if (!path.EndsWith(".txt"))
                throw new ArgumentException("Incorrect file extension: only .txt files are accepted.");
        }

        private void CheckIfFileExists(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} not found.");
        }

        private void ValidateFileContent(string content, string path)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException($"File {path} is empty.");
        }
    }
}
