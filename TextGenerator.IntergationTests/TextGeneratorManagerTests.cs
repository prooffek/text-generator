using FluentAssertions;
using TextGenerator.Application.Interfaces;
using TextGenerator.Application.Models;
using TextGenerator.Application.Services;

namespace TextGenerator.IntergationTests
{
    [TestClass]
    public class TextGeneratorManagerTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        public void TextGeneratorManager_Handle_ShouldReturnInvitationEmailText()
        {
            // Arrange
            IFileReader reader = new TextFileReader();
            ITemplateFiller templateFiller = new TemplateFiller();
            ITemplateAccessor templateAccessor = new TemplateAccessor(reader);
            ITextGeneratorManager manager = new TextGeneratorManager(templateAccessor, templateFiller);

            InvitationEmail model = new InvitationEmail
            {
                Name = "John Black",
                Address = new Address
                {
                    City = "Berlin",
                    Line1 = "Henrichstrasse, 14"
                }
            };

            string expectedResult = GetExpectedResult(model);

            // Act
            var response = manager.Handle(model);

            // Assert
            response.Should().NotBeNullOrWhiteSpace();
            response.Should().Be(expectedResult);
        }

        private string GetExpectedResult(InvitationEmail model)
        {
            return @$"Hello {model.Name},


We will be glad to see you in our office in {model.Address.City} at {model.Address.Line1}.

Looking forward to meeting with you!


Best,
Our company.";
        }
    }
}
