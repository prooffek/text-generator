using FluentAssertions;
using TextGenerator.Application.Services;

namespace Generator.IntergationTests
{
    [TestClass]
    public class TextFileReaderTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void TextFileReader_ReadFile_ShouldThrowIfPathIsNullOrEmpty(string path)
        {
            // Arrange
            var reader = new TextFileReader();

            // Act
            var exception = Assert.ThrowsException<NullReferenceException>(() => reader.ReadFile(path));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("File path cannot be null or empty.");
        }

        [TestMethod]
        [DataRow("file")]
        [DataRow("file.")]
        [DataRow("file . txt")]
        [DataRow("file.csv")]
        public void TextFileReader_ReadFile_ShouldThrowIfIncorrectExtension(string path)
        {
            // Arrange
            var reader = new TextFileReader();

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => reader.ReadFile(path));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Incorrect file extension: only .txt files are accepted.");
        }

        [TestMethod]
        public void TextFileReader_ReadFile_ShouldThrowIfFileNotFound()
        {
            // Arrange
            var reader = new TextFileReader();
            var path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("incorrect", "path.txt"));

            // Act
            var exception = Assert.ThrowsException<FileNotFoundException>(() => reader.ReadFile(path));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"File {path} not found.");
        }

        [TestMethod]
        public void TextFileReader_ReadFile_ShouldThrowIfTemplateEmpty()
        {
            // Arrange
            var reader = new TextFileReader();
            var path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Templates", "EmptyTemplate.txt"));

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => reader.ReadFile(path));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"File {path} is empty.");
        }

        [TestMethod]
        public void TextFileReader_ReadFile_ShouldReadTheWholeText()
        {
            // Arrange
            var reader = new TextFileReader();
            var path = Path.Combine("Templates", "InvitationEmailTemplate.txt");
            var expectedResult = GetTemplateContent();

            // Act
            var result = reader.ReadFile(path);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedResult);
        }

        private string GetTemplateContent()
        {
            return @"Hello #{{Name}},


We will be glad to see you in our office in #{{Address.City}} at #{{Address.Line1}}.

Looking forward to meeting with you!


Best,
Our company.";
        }
    }
}
