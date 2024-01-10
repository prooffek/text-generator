using FluentAssertions;
using Moq;
using TextGenerator.Application.Interfaces;
using TextGenerator.Application.Models;
using TextGenerator.Application.Services;

namespace TextGenerator.UnitTests
{
    [TestClass]
    public class TemplateAccessorTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [TestMethod]
        public void TemplateAccessor_GetTemplateForDataModel_ShouldThrowIfTemplatePathNotFound()
        {
            var reader = new Mock<IFileReader>();
            reader.Setup(m => m.ReadFile(It.IsAny<string>()));

            ITemplateAccessor accessor = new TemplateAccessor(reader.Object);
            Type modelType = typeof(Address);

            // Act
            var exception1 = Assert.ThrowsException<ArgumentException>(() => accessor.GetTemplateForDataModel(modelType));
            var exception2 = Assert.ThrowsException<ArgumentException>(() => accessor.GetTemplateForDataModel<Address>());

            // Assert
            exception1.Should().NotBeNull();
            exception1.Message.Should().Be($"No template assigned to model '{modelType.Name}'");

            exception2.Should().NotBeNull();
            exception2.Message.Should().Be($"No template assigned to model '{modelType.Name}'");

            reader.Verify(x => x.ReadFile(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void TemplateAccessor_GetTemplateForDataModel_ShouldReturnTemplateContentOnTypeGiven()
        {
            // Arrange
            var reader = new Mock<IFileReader>();
            reader.Setup(m => m.ReadFile(It.IsAny<string>()))
                .Returns(GetTemplateString())
                .Verifiable();

            ITemplateAccessor accessor = new TemplateAccessor(reader.Object);
            Type modelType = typeof(InvitationEmail);

            // Act
            var result = accessor.GetTemplateForDataModel(modelType);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be(GetTemplateString());
            reader.Verify(x => x.ReadFile(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TemplateAccessor_GetTemplateForDataModel_ShouldReturnTemplateContentOnGenericCall()
        {
            // Arrange
            var reader = new Mock<IFileReader>();
            reader.Setup(m => m.ReadFile(It.IsAny<string>()))
                .Returns(GetTemplateString())
                .Verifiable();

            ITemplateAccessor accessor = new TemplateAccessor(reader.Object);

            // Act
            var result = accessor.GetTemplateForDataModel<InvitationEmail>();

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be(GetTemplateString());
            reader.Verify(x => x.ReadFile(It.IsAny<string>()), Times.Once);
        }

        private string GetTemplateString()
        {
            return @"Hello #{{ Name }},


We will be glad to see you in our office in #{{    Address.City}} at #{{ Address.Line1 }}.

Looking forward to meeting with you!


Best,
Our company.s";
        }
    }
}
