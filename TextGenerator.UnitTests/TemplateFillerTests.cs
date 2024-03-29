﻿using FluentAssertions;
using System.Text;
using TextGenerator.Application.Models;
using TextGenerator.Application.Services;

namespace TextGenerator.UnitTests
{
    [TestClass]
    public class TemplateFillerTests
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
        [DataRow("  ")]
        [DataRow("      ")]

        public void TemplateFiller_GenerateText_ShouldThrowIfTemplateNullOrWhitespace(string template)
        {
            // Arrange
            TemplateFiller service = new();

            InvitationEmail model = new();

            // Act
            var exception = Assert.ThrowsException<ArgumentNullException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.ParamName.Should().Be(nameof(template));
            exception.Message.Should().Be("Template may not be null or an empty string. (Parameter 'template')");
        }

        [TestMethod]
        [DataRow("Test template without placeholder.")]
        [DataRow("Test template with wrong {{placeholder}}.")]
        [DataRow("Test template with wrong #{{}}.")]
        [DataRow("Test template with wrong #{{test_placeholder}}.")]
        public void TemplateFiller_GenerateText_ShouldThrowIfWrongTemplate(string template)
        {
            // Arrange
            TemplateFiller service = new();

            InvitationEmail model = new();

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Template does not include any valid placeholder. Accepted placeholder pattern is: '#{{placeholderName}}'.");
        }

        [TestMethod]
        public void TemplateFiller_GenerateText_ShouldThrowIfDataModelIsNull()
        {
            // Arrange
            TemplateFiller service = new();

            var template = "Test template with correct #{{placeholder}}.";
            InvitationEmail? model = null ;

            // Act
            var exception = Assert.ThrowsException<ArgumentNullException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.ParamName.Should().Be(nameof(model));
        }

        [TestMethod]
        public void TemplateFiller_GenerateText_ShouldThrowIfPlaceHolderValueIsNull()
        {
            // Arrange
            TemplateFiller service = new();

            var template = GetTemplateString();
            InvitationEmail? model = new InvitationEmail();

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Incorrect property: property '{nameof(model.Name)}' not found or has incorrect value.");
        }

        [TestMethod]
        [DataRow("", "", "", "Name")]
        [DataRow("  ", "", "", "Name")]
        [DataRow("John Brown", "", "Common Street 16", "City")]
        [DataRow("John Brown", "   ", "Common Street 16", "City")]
        [DataRow("John Brown", "Mardit", "", "Line1")]
        [DataRow("John Brown", "Mardit", "  ", "Line1")]
        public void TemplateFiller_GenerateText_ShouldThrowIfPlaceHolderValueIsEmpty(string name, string city, string line1, string placeholder )
        {
            // Arrange
            TemplateFiller service = new();

            var template = GetTemplateString();
            InvitationEmail? model = new InvitationEmail
            {
                Name = name,
                Address = new Address
                {
                    City = city,
                    Line1 = line1
                }
            };

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Value of the placeholder '{placeholder}' cannot be empty.");
        }

        [TestMethod]
        [DataRow("Age")]
        [DataRow("Income")]
        [DataRow("HeightInMeters")]
        [DataRow("HasWife")]
        public void TemplateFiller_GenerateText_ShouldThrowIfValueType(string placeholder)
        {
            // Arrange
            TemplateFiller service = new();

            var template = $"#{{{{{placeholder}}}}}";
            var model = new WrongModel
            {
                Username = "Test",
                Password = Encoding.ASCII.GetBytes("TestPassword"),
                Age = 42,
                Income = 987098M,
                HeightInMeters = 1.8F,
                ChildrenNames = new List<string> { "John", "Alice" },
                HasWife = true,
            };

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Incorrect property: property '{placeholder}' not found or has incorrect value.");
        }

        [TestMethod]
        [DataRow("Password")]
        [DataRow("ChildrenNames")]
        public void TemplateFiller_GenerateText_ShouldThrowIfCollection(string placeholder)
        {
            // Arrange
            TemplateFiller service = new();

            var template = $"#{{{{{placeholder}}}}}";
            var model = new WrongModel
            {
                Password = Encoding.ASCII.GetBytes("TestPassword"),
                ChildrenNames = new List<string> { "John", "Alice" },
            };

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Incorrect property: property '{placeholder}' has incorrect value.");
        }

        [TestMethod]
        [DataRow("paSSword")]
        [DataRow("Children Names")]
        [DataRow(" ")]
        [DataRow(" a  ")]
        [DataRow("test")]

        public void TemplateFiller_GenerateText_ShouldThrowIfPropertyNotFound(string placeholder)
        {
            // Arrange
            TemplateFiller service = new();

            var template = $"#{{{{{placeholder}}}}}";
            var model = new WrongModel();

            // Act
            var exception = Assert.ThrowsException<ArgumentException>(() => service.GenerateText(template, model));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"Incorrect property: property '{placeholder.Trim()}' not found or has incorrect value.");
        }

        [TestMethod]
        public void TemplateFiller_GenerateText_ShouldReplacePlaceholdersWithValues()
        {
            // Arrange
            TemplateFiller service = new();

            var template = GetTemplateString();
            InvitationEmail? model = GetInvitationEmailModel();
            var expectedResult = GetExpectedResult();

            // Act
            var result = service.GenerateText(template, model);

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be(expectedResult);
        }

        private string GetTemplateString()
        {
            return @"Hello #{{ Name }},


We will be glad to see you in our office in #{{    Address.City}} at #{{ Address.Line1 }}.

Looking forward to meeting with you!


Best,
Our company.";
        }

        private string GetExpectedResult()
        {
            return @"Hello John Doe,


We will be glad to see you in our office in Budapest at Main Street, 1.

Looking forward to meeting with you!


Best,
Our company.";
        }

        private InvitationEmail GetInvitationEmailModel()
        {
            return new InvitationEmail
            {
                Name = "John Doe",
                Address = new Address()
                {
                    City = "Budapest",
                    Line1 = "Main Street, 1"
                }
            };
        }
    }

    internal class WrongModel 
    {
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public int Age { get; set; }
        public decimal Income { get; set; }
        public float HeightInMeters { get; set; }
        public List<string> ChildrenNames { get; set; }
        public bool HasWife { get; set; }
    }
}
